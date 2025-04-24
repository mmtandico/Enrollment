using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCrypt.Net;
using MySql.Data.MySqlClient;
using SendGrid;
using System.Security.Cryptography;
using SendGrid.Helpers.Mail;

namespace Enrollment_System
{
    public partial class FormResetPass : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
        private readonly string _sendGridApiKey = ConfigurationManager.AppSettings["SendGridApiKey"];

        public FormResetPass()
        {
            InitializeComponent();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void BtnVerify_Click(object sender, EventArgs e)
        {
            string email = TxtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please enter your email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    
                    string query = "SELECT is_verified FROM Users WHERE email = @Email";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        object result = cmd.ExecuteScalar();

                        if (result == null || Convert.ToBoolean(result) == false)
                        {
                            MessageBox.Show("Email not found or not verified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                  
                    string otp = GenerateSecureOTP();
                    string upsertQuery = "UPDATE Users SET otp_code = @OTP, otp_expiry = NOW() + INTERVAL 5 MINUTE WHERE email = @Email";

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
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task<bool> SendOTPEmail(string email, string otp)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                var client = new SendGridClient(_sendGridApiKey);
                var from = new EmailAddress("enrollment.test101@gmail.com", "Enrollment System");
                var to = new EmailAddress(email);
                var subject = "Your OTP Code for Password Reset";

                string plainTextContent = $"Dear Student,\n\n" +
                                          $"You have requested to reset your password at PAMBAYANG DALUBHASAAN NG MARILAO. " +
                                          $"Your OTP code for resetting your password is: *{otp}*\n" +
                                          $"This OTP will expire in 5 minutes.\n\n" +
                                          "Please use the OTP to proceed with resetting your password. If you did not request this change, please disregard this message.\n\n" +
                                          "Thank you for using our system. If you need further assistance, please reach out to our support team.\n\n" +
                                          "Best regards,\n" +
                                          "Enrollment Team\nPAMBAYANG DALUBHASAAN NG MARILAO";

                string htmlContent = $"<strong>Dear Student,</strong><br><br>" +
                                     $"You have requested to reset your password at <strong>PAMBAYANG DALUBHASAAN NG MARILAO</strong>. " +
                                     $"Your OTP code for resetting your password is: <span style='font-size:18px; color: #ff6600; font-weight: bold;'>{otp}</span><br>" +
                                     $"This OTP will expire in 5 minutes.<br><br>" +
                                     "Please use the OTP to proceed with resetting your password. If you did not request this change, please disregard this message.<br><br>" +
                                     "Thank you for using our system. If you need further assistance, please reach out to our support team.<br><br>" +
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

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            string email = TxtEmail.Text.Trim();
            string otp = TxtOTP.Text.Trim();
            string newPassword = TxtNewPassword.Text.Trim();

            if (string.IsNullOrEmpty(otp) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Please fill in both the OTP and new password fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                   
                    string query = "SELECT otp_code, otp_expiry FROM Users WHERE email = @Email";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
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
                                MessageBox.Show("Email not found or OTP is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                   
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    string updateQuery = "UPDATE Users SET password_hash = @Password, otp_code = NULL, otp_expiry = NULL WHERE email = @Email";

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Password has been successfully reset.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ChkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            TxtNewPassword.PasswordChar = ChkShowPass.Checked ? '\0' : '*';
        }

        private void LblBackLogin_Click(object sender, EventArgs e)
        {
            FormLogin formlogin = new FormLogin();
            formlogin.ShowDialog();
        }
    }
}
