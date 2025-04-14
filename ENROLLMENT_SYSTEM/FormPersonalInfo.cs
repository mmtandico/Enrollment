using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Enrollment_System
{
    public partial class FormPersonalInfo : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
        private Image[] images;
        private int currentImageIndex = 0;
        private long loggedInUserId;
        //private object panel11;
        private bool fieldsLocked = true;
        private long? currentGuardianId = null;


        public FormPersonalInfo()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            UIHelper.ApplyAdminVisibility(BtnDataBase);

            if (!string.IsNullOrEmpty(SessionManager.LastName) && !string.IsNullOrEmpty(SessionManager.FirstName))
            {
                LblWelcome.Text = $"{SessionManager.LastName}, {SessionManager.FirstName[0]}.";
            }
            else if (!string.IsNullOrEmpty(SessionManager.LastName))
            {
                LblWelcome.Text = $"{SessionManager.LastName}";
            }
            else if (!string.IsNullOrEmpty(SessionManager.FirstName))
            {
                LblWelcome.Text = $"{SessionManager.FirstName[0]}.";
            }
            else
            {
                LblWelcome.Text = "";
            }

            this.Activated += FormPersonalInfo_Activated;
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

        private void FormPersonalInfo_Activated(object sender, EventArgs e)
        {
            LoadUserData();
        }
        private void FormPersonalInfo_Load(object sender, EventArgs e)
        {
            LoadUserData();
            if (fieldsLocked)
            {
                ToggleFields(false);
            }
        }

        private void LoadUserData()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string checkUserQuery = "SELECT COUNT(*) FROM students WHERE user_id = @UserID";
                    using (var checkCmd = new MySqlCommand(checkUserQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (userExists == 0)
                        {
                            string insertQuery = @"
                                INSERT INTO students (user_id,student_no, student_lrn, first_name, middle_name, last_name, birth_date, age, sex, civil_status, nationality) 
                                VALUES (@UserID,'', '', '', '', '', '2000-01-01', 0, 'Male', '', '')";

                            using (var insertCmd = new MySqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    string query = @"
                        SELECT 
                            s.student_id, 
                            IFNULL(s.student_no, '') AS student_no, 
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
                            s.profile_picture,
                            IFNULL(g.first_name, '') AS guardian_first_name,
                            IFNULL(g.last_name, '') AS guardian_last_name, 
                            IFNULL(g.middle_name, '') AS guardian_middle_name,
                            IFNULL(g.contact_number, '' )AS guardian_contact, 
                            sg.relationship,
                            IFNULL(g.guardian_id, 0) AS guardian_id
                        FROM students s
                        LEFT JOIN users u ON s.user_id = u.user_id
                        LEFT JOIN contact_info c ON s.student_id = c.student_id
                        LEFT JOIN addresses a ON s.student_id = a.student_id
                        LEFT JOIN student_guardians sg ON s.student_id = sg.student_id
                        LEFT JOIN parents_guardians g ON sg.guardian_id = g.guardian_id
                        WHERE s.user_id = @UserID
                        ORDER BY s.student_id DESC LIMIT 1";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                TxtStudentNo.Text = reader["student_no"].ToString();
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
                                TxtGuardianFirstName.Text = reader["guardian_first_name"].ToString();
                                TxtGuardianLastName.Text = reader["guardian_last_name"].ToString();
                                TxtGuardianMiddleName.Text = reader["guardian_middle_name"].ToString();
                                TxtGuardianContact.Text = reader["guardian_contact"].ToString();
                                TxtGuardianRelation.Text = reader["relationship"].ToString();
                                currentGuardianId = Convert.ToInt64(reader["guardian_id"]);

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
                    long studentId = -1;


                    string checkStudentQuery = "SELECT student_id FROM students WHERE user_id = @UserID";
                    using (var checkCmd = new MySqlCommand(checkStudentQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        object result = checkCmd.ExecuteScalar();

                        if (result == null)
                        {

                            string insertStudentQuery = @"
                                INSERT INTO students (user_id, student_no, student_lrn, first_name, middle_name, last_name, birth_date, age, sex, civil_status, nationality) 
                                VALUES (@UserID, '', '', '', '', '2000-01-01', '', '', '', '')";

                            using (var insertCmd = new MySqlCommand(insertStudentQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                                insertCmd.ExecuteNonQuery();


                                studentId = Convert.ToInt64(new MySqlCommand("SELECT LAST_INSERT_ID()", conn).ExecuteScalar());
                            }
                        }
                        else
                        {

                            studentId = Convert.ToInt64(result);
                        }
                    }


                    string studentQuery = @"
                        INSERT INTO students (student_id, user_id, student_no, student_lrn, first_name, middle_name, last_name, birth_date, age, sex, civil_status, nationality) 
                        VALUES (@StudentID, @UserID, @StudentNo, @StudentLRN, @FirstName, @MiddleName, @LastName, @BirthDate, @Age, @Sex, @CivilStatus, @Nationality)
                        ON DUPLICATE KEY UPDATE 
                            student_no = VALUES(student_no), 
                            student_lrn = VALUES(student_lrn), 
                            first_name = VALUES(first_name), 
                            middle_name = VALUES(middle_name), 
                            last_name = VALUES(last_name), 
                            birth_date = VALUES(birth_date), 
                            age = VALUES(age), 
                            sex = VALUES(sex), 
                            civil_status = VALUES(civil_status), 
                            nationality = VALUES(nationality)";

                    ExecuteQuery(conn, studentQuery,
                        new MySqlParameter("@StudentID", studentId),
                        new MySqlParameter("@UserID", loggedInUserId),
                        new MySqlParameter("@StudentNo", TxtStudentNo.Text),
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
                        INSERT INTO contact_info(student_id, phone_no)
                        VALUES(@StudentID, @PhoneNo)
                        ON DUPLICATE KEY UPDATE phone_no = @PhoneNo";

                    ExecuteQuery(conn, contactQuery,
                        new MySqlParameter("@StudentID", studentId),
                        new MySqlParameter("@PhoneNo", TxtPhoneNo.Text)
                    );


                    string addressQuery = @"
                        INSERT INTO addresses (student_id, block_street, subdivision, barangay, city, province, zipcode) 
                        VALUES (@StudentID, @BlockStreet, @Subdivision, @Barangay, @City, @Province, @Zipcode) 
                        ON DUPLICATE KEY UPDATE 
                            block_street = VALUES(block_street), 
                            subdivision = VALUES(subdivision), 
                            barangay = VALUES(barangay), 
                            city = VALUES(city), 
                            province = VALUES(province), 
                            zipcode = VALUES(zipcode)";

                    ExecuteQuery(conn, addressQuery,
                        new MySqlParameter("@StudentID", studentId),
                        new MySqlParameter("@BlockStreet", TxtBStreet.Text),
                        new MySqlParameter("@Subdivision", TxtSubCom.Text),
                        new MySqlParameter("@Barangay", TxtBrgy.Text),
                        new MySqlParameter("@City", TxtCity.Text),
                        new MySqlParameter("@Province", TxtProvince.Text),
                        new MySqlParameter("@Zipcode", TxtZipcode.Text)
                    );


                    // Guardian update/insert
                    if (currentGuardianId > 0)
                    {
                        // Update existing guardian
                        string guardianQuery = @"
                            UPDATE parents_guardians 
                            SET first_name = @GuardianFirstName, 
                                last_name = @GuardianLastName, 
                                middle_name = @GuardianMiddleName, 
                                contact_number = @GuardianContact
                            WHERE guardian_id = @GuardianID";

                        ExecuteQuery(conn, guardianQuery,
                            new MySqlParameter("@GuardianFirstName", TxtGuardianFirstName.Text),
                            new MySqlParameter("@GuardianLastName", TxtGuardianLastName.Text),
                            new MySqlParameter("@GuardianMiddleName", TxtGuardianMiddleName.Text),
                            new MySqlParameter("@GuardianContact", TxtGuardianContact.Text),
                            new MySqlParameter("@GuardianID", currentGuardianId)
                        );
                    }
                    else
                    {
                        // Insert new guardian
                        string guardianQuery = @"
                            INSERT INTO parents_guardians 
                                (first_name, last_name, middle_name, contact_number) 
                            VALUES 
                                (@GuardianFirstName, @GuardianLastName, @GuardianMiddleName, @GuardianContact)";

                        ExecuteQuery(conn, guardianQuery,
                            new MySqlParameter("@GuardianFirstName", TxtGuardianFirstName.Text),
                            new MySqlParameter("@GuardianLastName", TxtGuardianLastName.Text),
                            new MySqlParameter("@GuardianMiddleName", TxtGuardianMiddleName.Text),
                            new MySqlParameter("@GuardianContact", TxtGuardianContact.Text)
                        );

                        // Get the new guardian ID
                        currentGuardianId = Convert.ToInt64(new MySqlCommand("SELECT LAST_INSERT_ID()", conn).ExecuteScalar());
                    }

                    // Update student-guardian relationship
                    string studentGuardianQuery = @"
                        INSERT INTO student_guardians 
                            (student_id, guardian_id, relationship) 
                        VALUES 
                            (@StudentID, @GuardianID, @Relationship)
                        ON DUPLICATE KEY UPDATE 
                            relationship = VALUES(relationship)";

                    ExecuteQuery(conn, studentGuardianQuery,
                        new MySqlParameter("@StudentID", studentId),
                        new MySqlParameter("@GuardianID", currentGuardianId),
                        new MySqlParameter("@Relationship", TxtGuardianRelation.Text)
                    );

                    MessageBox.Show("Information updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadUserData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            SessionManager.FirstName = TxtFirstName.Text;
            SessionManager.LastName = TxtLastName.Text;
            LblWelcome.Text = $"{SessionManager.LastName}, {(string.IsNullOrWhiteSpace(SessionManager.FirstName) ? "" : SessionManager.FirstName[0] + ".")}";
            SetEnabledRecursive(groupBox1, false);
            SetEnabledRecursive(groupBox2, false);
            SetEnabledRecursive(groupBox3, false);
            SetEnabledRecursive(groupBox4, false);
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
            //MessageBox.Show("Fields are now unlocked for editing.", "Edit Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);

            SetEnabledRecursive(groupBox1, true);
            SetEnabledRecursive(groupBox2, true);
            SetEnabledRecursive(groupBox3, true);
            SetEnabledRecursive(groupBox4, true);

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
        private void BtnHome_Click(object sender, EventArgs e) => SwitchForm(new FormHome());
        private void BtnCourses_Click(object sender, EventArgs e) => SwitchForm(new FormCourse());
        private void BtnEnrollment_Click(object sender, EventArgs e) => SwitchForm(new FormEnrollment());
        private void BtnDataBase_Click(object sender, EventArgs e) => SwitchForm(new FormDatabaseInfo());

        private void timer1_Tick(object sender, EventArgs e)
        {
            currentImageIndex = (currentImageIndex + 1) % images.Length;
            pictureBox1.Image = images[currentImageIndex];
        }

        private void ToggleFields(bool enabled)
        {
            fieldsLocked = !enabled;

            foreach (var groupBox in new[] { groupBox1, groupBox2, groupBox3, groupBox4 })
            {
                foreach (Control control in groupBox.Controls)
                {
                    if (control is TextBox || control is ComboBox || control is DateTimePicker || control is CheckBox || control is Panel)
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

        private void SwitchForm(Form newForm)
        {
            this.Hide();
            newForm.Show();
            this.Close();
        }

        private void SetEnabledRecursive(Control control, bool enabled)
        {
            if (control is TextBox || control is ComboBox || control is DateTimePicker || control is CheckBox || control is Panel)
            {
                control.Enabled = enabled;
                control.BackColor = enabled ? SystemColors.Window : SystemColors.ControlLight;
            }

            foreach (Control child in control.Controls)
            {
                SetEnabledRecursive(child, enabled);
            }
        }

        private void ChkMale_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ChkMale.Checked)
            {
                ChkFemale.Checked = false;
            }
        }

        private void ChkFemale_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ChkFemale.Checked)
            {
                ChkMale.Checked = false;
            }
        }

        private void DateBirthPicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime birthDate = DateBirthPicker.Value;
            int age = DateTime.Now.Year - birthDate.Year;

            if (DateTime.Now.Month < birthDate.Month || (DateTime.Now.Month == birthDate.Month && DateTime.Now.Day < birthDate.Day))
            {
                age--;
            }

            TxtAge.Text = age.ToString();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void TxtStudentNo_Click(object sender, EventArgs e)
        {

        }

        private void TxtStudentLRN_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtStudentLRN_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtStudentLRN.Text))
            {
                if (string.IsNullOrWhiteSpace(TxtStudentNo.Text) || !TxtStudentNo.Text.StartsWith($"PDM-{DateTime.Now.Year}-"))
                {
                    try
                    {
                        using (var conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();

                            string currentYear = DateTime.Now.Year.ToString();
                            string query = $@"
                    SELECT IFNULL(MAX(CAST(SUBSTRING(student_no, 10, 6) AS UNSIGNED)), 0) + 1 
                    FROM students 
                    WHERE student_no LIKE 'PDM-{currentYear}-%'";

                            using (var cmd = new MySqlCommand(query, conn))
                            {
                                int nextIdNumber = Convert.ToInt32(cmd.ExecuteScalar());
                                TxtStudentNo.Text = $"PDM-{currentYear}-{nextIdNumber:D6}";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error generating student number: " + ex.Message,
                                      "Database Error",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);

                        int nextIdNumber = GetNextStudentIdNumber();
                        TxtStudentNo.Text = $"PDM-{DateTime.Now.Year}-{nextIdNumber:D6}";
                    }
                }
            }
        }

        private static int studentIdCounter = 0;
        private int GetNextStudentIdNumber()
        {
            return ++studentIdCounter;
        }
    }
}
