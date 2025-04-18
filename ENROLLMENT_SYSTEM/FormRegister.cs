using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using BCrypt.Net;
using System.Configuration;

namespace Enrollment_System
{
    public partial class FormRegister : Form
    {
        private string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";

        private readonly string _sendGridApiKey = ConfigurationManager.AppSettings["SendGridApiKey"];

        public FormRegister()
        {
            InitializeComponent();
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 8 &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit);
                  // password.Any(char.IsUpper) &&
                  // password.Any(ch => "!@#$%^&*()_+{}:<>?".Contains(ch));
        }

        private string GenerateSecureOTP()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] otpBytes = new byte[4];
                rng.GetBytes(otpBytes);
                int otp = Math.Abs(BitConverter.ToUInt16(otpBytes, 0) % 900000) + 100000;
                return otp.ToString();
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

                    string otpQuery = "SELECT otp_code, otp_expiry FROM Users WHERE email = @Email AND is_verified = FALSE";
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
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);
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

                var client = new SendGridClient(_sendGridApiKey);
                //string apiKey = Environment.GetEnvironmentVariable("MY_API_KEY");
                //var client = new SendGridClient(apiKey);
                var from = new EmailAddress("enrollment.test101@gmail.com", "Enrollment System");
                var to = new EmailAddress(email);
                var subject = "Your OTP Code";
                
                string plainTextContent = $"Dear student,\n\n" +
                                          $"Your OTP code for enrollment at PAMBAYANG DALUBHASAAN NG MARILAO is: *{otp}*\n" +
                                          $"This OTP will expire in 5 minutes.\n\n" +
                                          "Thank you for choosing us for your educational journey. If you have any questions, please reach out to our enrollment team.\n\n" +
                                          "Best regards,\n" +
                                          "Enrollment Team\nPAMBAYANG DALUBHASAAN NG MARILAO";

                
                string htmlContent = $"<strong>Dear student,</strong><br><br>" +
                                     $"Your OTP code for enrollment at <strong>PAMBAYANG DALUBHASAAN NG MARILAO</strong> is: " +
                                     $"<span style='font-size:18px; color: #ff6600; font-weight: bold;'>{otp}</span><br>" + 
                                     $"This OTP will expire in 5 minutes.<br><br>" +
                                     "Thank you for choosing us for your educational journey. If you have any questions, please reach out to our enrollment team.<br><br>" +
                                     "<strong>Best regards,</strong><br>" +
                                     "Enrollment Team<br>PAMBAYANG DALUBHASAAN NG MARILAO";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

                var response = await client.SendEmailAsync(msg);

                return response.StatusCode == System.Net.HttpStatusCode.Accepted;
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
            this.Hide();
            new FormLogin().Show();
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
