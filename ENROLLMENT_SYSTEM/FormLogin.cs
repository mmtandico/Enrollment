﻿using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using BCrypt.Net;

namespace Enrollment_System
{
    public partial class FormLogin : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";


        public FormLogin()
        {
            InitializeComponent();
        }

        private void ChkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            TxtPass.PasswordChar = ChkShowPass.Checked ? '\0' : '*';
        }

        private void Btn_Login_Click(object sender, EventArgs e)
        {
            string email = TxtEmail.Text.Trim().ToLower();
            string password = TxtPass.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter Email and Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"SELECT u.user_id, u.password_hash, u.role, 
                                   s.first_name, s.last_name, s.student_id
                            FROM Users u 
                            LEFT JOIN students s ON u.user_id = s.user_id 
                            WHERE LOWER(u.email) = @Email AND u.is_verified = TRUE";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = reader.GetInt32("user_id");
                                string storedHash = reader["password_hash"].ToString();
                                string role = reader["role"].ToString();
                                string firstName = reader["first_name"]?.ToString() ?? "";
                                string lastName = reader["last_name"]?.ToString() ?? "";

                                int studentId = reader["student_id"] != DBNull.Value ?
                                       reader.GetInt32("student_id") : 0;

                                if (BCrypt.Net.BCrypt.Verify(password, storedHash))
                                {
                                    
                                    SessionManager.Login(userId, email, role, firstName, lastName, studentId);

                                    MessageBox.Show($"Welcome, {firstName} {lastName}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Hide();

                                    if (role == "admin")
                                    {
                                        new FormDatabaseInfo().Show(); 
                                    }
                                    else
                                    {
                                        new FormHome().Show(); 
                                    }

                                }
                                else
                                {
                                    MessageBox.Show("Incorrect Password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid Email or Account Not Verified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void BtnReg_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormRegister().Show();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
