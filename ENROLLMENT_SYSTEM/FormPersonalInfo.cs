﻿using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Enrollment_System
{
    public partial class FormPersonalInfo : Form
    {
        private string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
        private Image[] images;
        private int currentImageIndex = 0;
        private long loggedInUserId;
        private object panel11;

        public FormPersonalInfo()
        {
            InitializeComponent();

            
            if (!SessionManager.IsLoggedIn)
            {
                MessageBox.Show("Session expired. Please log in again.", "Session Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                new FormLogin().Show();
                this.Close();
                return;
            }

            loggedInUserId = SessionManager.UserId;
            images = new Image[]
            {
                Properties.Resources.P9700277_1_1024x576,
                Properties.Resources.Graduation_2019_1_1536x1024,
                Properties.Resources.BGIMAGE1,
                Properties.Resources.BGIMAGE2
            };

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = images[currentImageIndex];

            timer1.Interval = 3000;
            timer1.Start();
            pictureBox2.Image = Properties.Resources.PROFILE;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

            this.Load += FormPersonalInfo_Load;
        }

        private void FormPersonalInfo_Load(object sender, EventArgs e)
        {
            LoadUserData();
        }

        private void LoadUserData()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                  
                    string query = @"
                SELECT 
                    s.student_id, 
                    IFNULL(s.student_lrn, '') AS student_lrn, 
                    IFNULL(s.first_name, '') AS first_name, 
                    IFNULL(s.middle_name, '') AS middle_name, 
                    IFNULL(s.last_name, '') AS last_name, 
                    IFNULL(s.birth_date, '2000-01-01') AS birth_date, 
                    IFNULL(s.age, 0) AS age, 
                    IFNULL(s.sex, 'Unknown') AS sex, 
                    IFNULL(s.civil_status, '') AS civil_status, 
                    IFNULL(s.nationality, '') AS nationality, 
                    IFNULL(c.phone_no, '') AS phone_no, 
                    IFNULL(u.email, '') AS email, 
                    IFNULL(a.block_street, '') AS block_street, 
                    IFNULL(a.subdivision, '') AS subdivision, 
                    IFNULL(a.barangay, '') AS barangay, 
                    IFNULL(a.city, '') AS city, 
                    IFNULL(a.province, '') AS province, 
                    IFNULL(a.zipcode, '') AS zipcode, 
                    s.profile_picture 
                FROM students s 
                LEFT JOIN users u ON s.user_id = u.user_id 
                LEFT JOIN contact_info c ON s.user_id = c.user_id 
                LEFT JOIN addresses a ON s.user_id = a.user_id 
                WHERE s.user_id = @UserID";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                
                                TxtStudentID.Text = reader["student_id"].ToString();
                                TxtStudentLRN.Text = reader["student_lrn"].ToString();
                                TxtFirstName.Text = reader["first_name"].ToString();
                                TxtMiddleName.Text = reader["middle_name"].ToString();
                                TxtLastName.Text = reader["last_name"].ToString();
                                DateBirthPicker.Value = Convert.ToDateTime(reader["birth_date"]);
                                TxtAge.Text = reader["age"].ToString();
                                TxtCivilStatus.Text = reader["civil_status"].ToString();
                                TxtNational.Text = reader["nationality"].ToString();
                                ChkMale.Checked = reader["sex"].ToString() == "Male";
                                ChkFemale.Checked = reader["sex"].ToString() == "Female";
                                TxtPhoneNo.Text = reader["phone_no"].ToString();
                                TxtEmail.Text = reader["email"].ToString();
                                TxtBStreet.Text = reader["block_street"].ToString();
                                TxtSubCom.Text = reader["subdivision"].ToString();
                                TxtBrgy.Text = reader["barangay"].ToString();
                                TxtCity.Text = reader["city"].ToString();
                                TxtProvince.Text = reader["province"].ToString();
                                TxtZipcode.Text = reader["zipcode"].ToString();

                                if (reader["profile_picture"] != DBNull.Value)
                                {
                                    byte[] imageBytes = (byte[])reader["profile_picture"];
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        pictureBox2.Image = Image.FromStream(ms);
                                    }
                                }
                                else
                                {
                                    pictureBox2.Image = Properties.Resources.PROFILE;
                                }
                                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                            }
                            else
                            {
                                MessageBox.Show("No data found for this user.", "Data Fetch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string checkStudentQuery = "SELECT COUNT(*) FROM students WHERE user_id = @UserID";
                    using (var checkCmd = new MySqlCommand(checkStudentQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        int studentExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (studentExists == 0) 
                        {
                            string insertStudentQuery = "INSERT INTO students (user_id, first_name, middle_name, last_name, birth_date, age, sex, civil_status, nationality) " +
                                                        "VALUES (@UserID, '', '', '', '', '', '', '', '')";
                            using (var insertCmd = new MySqlCommand(insertStudentQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    string studentQuery = @"
                UPDATE students 
                SET 
                    student_id = @StudentID,
                    student_lrn = @StudentLRN, 
                    first_name = @FirstName, 
                    middle_name = @MiddleName, 
                    last_name = @LastName, 
                    birth_date = @BirthDate, 
                    age = @Age, 
                    sex = @Sex,
                    civil_status = @CivilStatus,
                    nationality = @Nationality
                    
                WHERE user_id = @UserID";

                    ExecuteQuery(conn, studentQuery,
                        new MySqlParameter("@StudentID", TxtStudentID.Text),
                        new MySqlParameter("@UserID", loggedInUserId),
                        new MySqlParameter("@StudentLRN", TxtStudentLRN.Text),
                        new MySqlParameter("@FirstName", TxtFirstName.Text),
                        new MySqlParameter("@MiddleName", TxtMiddleName.Text),
                        new MySqlParameter("@LastName", TxtLastName.Text),
                        new MySqlParameter("@BirthDate", DateBirthPicker.Value),
                        new MySqlParameter("@Age", TxtAge.Text),
                        new MySqlParameter("@Sex", ChkMale.Checked ? "Male" : "Female"),
                        new MySqlParameter("@CivilStatus", TxtCivilStatus.Text),
                        new MySqlParameter("@Nationality", TxtNational.Text)
                        
                    );

                   
                    string contactQuery = @"
                INSERT INTO contact_info (user_id, phone_no) 
                VALUES (@UserID, @PhoneNo) 
                ON DUPLICATE KEY UPDATE phone_no = @PhoneNo";

                    ExecuteQuery(conn, contactQuery,
                        new MySqlParameter("@UserID", loggedInUserId),
                        new MySqlParameter("@PhoneNo", TxtPhoneNo.Text)
                    );

                   
                    string addressQuery = @"
                INSERT INTO addresses (user_id, block_street, subdivision, barangay, city, province, zipcode) 
                VALUES (@UserID, @BlockStreet, @Subdivision, @Barangay, @City, @Province, @Zipcode) 
                ON DUPLICATE KEY UPDATE 
                    block_street = @BlockStreet, 
                    subdivision = @Subdivision, 
                    barangay = @Barangay, 
                    city = @City, 
                    province = @Province, 
                    zipcode = @Zipcode";

                    ExecuteQuery(conn, addressQuery,
                        new MySqlParameter("@UserID", loggedInUserId),
                        new MySqlParameter("@BlockStreet", TxtBStreet.Text),
                        new MySqlParameter("@Subdivision", TxtSubCom.Text),
                        new MySqlParameter("@Barangay", TxtBrgy.Text),
                        new MySqlParameter("@City", TxtCity.Text),
                        new MySqlParameter("@Province", TxtProvince.Text),
                        new MySqlParameter("@Zipcode", TxtZipcode.Text)
                    );

                    MessageBox.Show("Information saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUserData(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            SetEnabledRecursive(groupBox1, false);
            SetEnabledRecursive(groupBox2, false);
            SetEnabledRecursive(groupBox3, false);
            MessageBox.Show("Fields have been saved and locked. They are now unclickable.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void BtnUpload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox2.Image = Image.FromFile(openFileDialog.FileName);
                    pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

                    byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);

                    SaveProfilePicture(imageBytes);
                }
            }
        }

        private void SaveProfilePicture(byte[] imageBytes)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE students SET profile_picture = @ProfilePicture WHERE user_id = @UserID";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        cmd.Parameters.AddWithValue("@ProfilePicture", imageBytes);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Profile picture updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving profile picture: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            

            ToggleFields(true);
            MessageBox.Show("Fields are now unlocked for editing.", "Edit Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);

            SetEnabledRecursive(groupBox1, true);
            SetEnabledRecursive(groupBox2, true);
            SetEnabledRecursive(groupBox3, true);
            MessageBox.Show("Fields are now unlocked for editing.", "Edit Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
    }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out?", "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SessionManager.Logout();
                new FormLogin().Show();
                this.Close();
            }
        }
        private void BtnExit_Click(object sender, EventArgs e) => Application.Exit();
        private void BtnHome_Click(object sender, EventArgs e) => NavigateTo(new FormHome());
        private void BtnCourses_Click(object sender, EventArgs e) => NavigateTo(new FormCourse());
        private void BtnEnrollment_Click(object sender, EventArgs e) => NavigateTo(new FormEnrollment());
        private void BtnDataBase_Click(object sender, EventArgs e) => NavigateTo(new FormDatabaseInfo());

        private void timer1_Tick(object sender, EventArgs e)
        {
            currentImageIndex = (currentImageIndex + 1) % images.Length;
            pictureBox1.Image = images[currentImageIndex];
        }

        private void ToggleFields(bool enabled)
        {
            foreach (var groupBox in new[] { groupBox1, groupBox2, groupBox3 })
            {
                foreach (Control control in groupBox.Controls)
                {
                    if (control is TextBox || control is ComboBox || control is DateTimePicker)
                    {
                        control.Enabled = enabled;
                        control.BackColor = enabled ? SystemColors.Window : SystemColors.ControlLight;
                    }
                }
            }
        }

        private void ExecuteQuery(MySqlConnection conn, string query, params MySqlParameter[] parameters)
        {
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddRange(parameters);
                cmd.ExecuteNonQuery();
            }
        }


        private void NavigateTo(Form form)
        {
            this.Close();
            form.Show();
        }


        private void SetEnabledRecursive(Control control, bool enabled)
        {
            // If the control is one of the input types, set its Enabled state and adjust the BackColor.
            if (control is TextBox || control is ComboBox || control is DateTimePicker)
            {
                control.Enabled = enabled;
                control.BackColor = enabled ? SystemColors.Window : SystemColors.ControlLight;
            }

            // Recursively process all child controls.
            foreach (Control child in control.Controls)
            {
                SetEnabledRecursive(child, enabled);
            }
        }


    }
}
