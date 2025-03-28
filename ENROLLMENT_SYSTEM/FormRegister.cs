using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Enrollment_System
{
    public partial class FormRegister : Form
    {
        private string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";

        public FormRegister()
        {
            InitializeComponent();
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => "!@#$%^&*()_+{}:<>?".Contains(ch));
        }

        private string GenerateSecureOTP()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] otpBytes = new byte[4];
                rng.GetBytes(otpBytes);
                int otp = BitConverter.ToInt32(otpBytes, 0) % 900000 + 100000;
                return Math.Abs(otp).ToString();
            }
        }

        private async void BtnSendOTP_Click(object sender, EventArgs e)
        {
            string email = TxtEmail.Text.Trim();
            string password = TxtPass.Text.Trim();
            string confirmPassword = TxtConfirmPass.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsValidPassword(password))
            {
                MessageBox.Show("Password must be at least 8 characters long, contain uppercase, lowercase, a number, and a special character.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string checkQuery = "SELECT is_verified FROM Users WHERE email = @Email";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        object result = checkCmd.ExecuteScalar();

                        if (result != null && Convert.ToBoolean(result) == true)
                        {
                            MessageBox.Show("Email is already registered and verified. Please log in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string otp = GenerateSecureOTP();

                    string upsertQuery = @"
                        INSERT INTO Users (email, otp_code, otp_expiry, is_verified, role) 
                        VALUES (@Email, @OTP, NOW() + INTERVAL 5 MINUTE, FALSE, 'user') 
                        ON DUPLICATE KEY UPDATE otp_code = @OTP, otp_expiry = NOW() + INTERVAL 5 MINUTE, is_verified = FALSE";

                    using (MySqlCommand cmd = new MySqlCommand(upsertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@OTP", otp);
                        cmd.ExecuteNonQuery();
                    }

                    bool emailSent = await SendOTPEmail(email, otp);
                    if (emailSent)
                    {
                        MessageBox.Show("OTP has been sent to your email.", "OTP Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnReg_Click(object sender, EventArgs e)
        {
            string email = TxtEmail.Text.Trim();
            string password = TxtPass.Text.Trim();
            string otp = TxtOTP.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(otp))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string otpQuery = "SELECT otp_code, COALESCE(otp_expiry, NOW() - INTERVAL 1 MINUTE) AS otp_expiry FROM Users WHERE email = @Email AND is_verified = FALSE";
                    using (MySqlCommand cmd = new MySqlCommand(otpQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedOtp = reader["otp_code"].ToString();
                                DateTime expiryTime = Convert.ToDateTime(reader["otp_expiry"]);

                                if (DateTime.Now > expiryTime)
                                {
                                    MessageBox.Show("OTP has expired. Please request a new one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                if (storedOtp != otp)
                                {
                                    MessageBox.Show("Invalid OTP. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Email not found or already verified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                    string updateQuery = "UPDATE Users SET password_hash = @Password, is_verified = TRUE, otp_code = NULL, otp_expiry = NULL WHERE email = @Email";
                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = email;
                        cmd.Parameters.Add("@Password", MySqlDbType.VarChar).Value = hashedPassword;
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Registration successful! You can now log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    new FormLogin().Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task<bool> SendOTPEmail(string email, string otp)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                string apiKey = "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"; // Replace with your actual key
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("enrollment.test101@gmail.com", "Enrollment System");
                var to = new EmailAddress(email);
                var subject = "Your OTP Code";
                var plainTextContent = $"Your OTP code is: {otp}\nThis OTP will expire in 5 minutes.";
                var htmlContent = $"<strong>{plainTextContent}</strong>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show($"SendGrid Error: {response.StatusCode}", "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending email: " + ex.Message, "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        private void ChkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            TxtPass.PasswordChar = ChkShowPass.Checked ? '\0' : '*';
            TxtConfirmPass.PasswordChar = ChkShowPass.Checked ? '\0' : '*';
        }

        private void BtnLog_Click(object sender, EventArgs e)
        {
            new FormLogin().Show();
            this.Hide();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}