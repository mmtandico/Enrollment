using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Collections.Generic;


namespace Enrollment_System
{
    public partial class FormPersonalInfo : Form
    {
        private readonly string connectionString = DatabaseConfig.ConnectionString;

        private Image[] images;
        private int currentImageIndex = 0;
        private long loggedInUserId;
        private bool fieldsLocked = true;
        private long? currentGuardianId = null;
        private string _viewingStudentNo = null;
        private long _currentStudentId = 0;
        public bool IsViewMode { get; set; } = false;
        

        public FormPersonalInfo(string studentNo = null)
        {
            InitializeComponent();
            _viewingStudentNo = studentNo;
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            InitializeForm();

            TxtStudentLRN.MaxLength = 12;
            TxtAge.MaxLength = 3;
            TxtPhoneNo.MaxLength = 11;
            TxtGuardianContact.MaxLength = 11;
            TxtZipcode.MaxLength = 4;

            // Only one assignment for TxtStudentLRN.Leave
            
           // TxtStudentLRN.Leave += TxtStudentLRN_Leave;

            TxtAge.KeyPress += TxtAge_KeyPress;
            TxtAge.Leave += TxtAge_Leave;

            TxtPhoneNo.KeyPress += TxtPhoneNo_KeyPress;
            TxtPhoneNo.Leave += TxtPhoneNo_Leave;

            TxtGuardianContact.KeyPress += TxtGuardianContact_KeyPress;
            TxtGuardianContact.Leave += TxtGuardianContact_Leave;

            TxtZipcode.KeyPress += TxtZipcode_KeyPress;
            TxtZipcode.Leave += TxtZipcode_Leave;
            TxtStudentLRN.KeyPress -= TxtStudentLRN_KeyPress; 
            TxtStudentLRN.KeyPress += TxtStudentLRN_KeyPress; 
        }

        private void InitializeForm()
        {
            UIHelper.ApplyAdminVisibility(BtnDataBase);
            WelcomeGreetings();

            this.Activated += FormPersonalInfo_Activated;
            if (!SessionManager.IsLoggedIn)
            {
                MessageBox.Show("Session expired. Please log in again.", "Session Expired",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            this.Load -= FormPersonalInfo_Load;
            this.Load += FormPersonalInfo_Load;

            TxtFirstName.KeyPress += TextBox_KeyPress;
            TxtMiddleName.KeyPress += TextBox_KeyPress;
            TxtLastName.KeyPress += TextBox_KeyPress;
            //CmbCivilStatus.KeyPress += TextBox_KeyPress;
            TxtNational.KeyPress += TextBox_KeyPress;
            TxtBStreet.KeyPress += TextBox_KeyPress;
            TxtSubCom.KeyPress += TextBox_KeyPress;
            TxtBrgy.KeyPress += TextBox_KeyPress;
            TxtCity.KeyPress += TextBox_KeyPress;
            TxtProvince.KeyPress += TextBox_KeyPress;
            TxtGuardianFirstName.KeyPress += TextBox_KeyPress;
            TxtGuardianMiddleName.KeyPress += TextBox_KeyPress;
            TxtGuardianLastName.KeyPress += TextBox_KeyPress;
            TxtGuardianRelation.KeyPress += TextBox_KeyPress;
            if (string.IsNullOrWhiteSpace(TxtNational.Text))
            {
                TxtNational.Text = "Filipino";
            }
        }

        private void WelcomeGreetings()
        {
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
        }

        private void FormPersonalInfo_Activated(object sender, EventArgs e)
        {
            //LoadUserData();
        }

        private void FormPersonalInfo_Load(object sender, EventArgs e)
        {
            if (IsViewMode)
            {
                this.ControlBox = false;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.WindowState = FormWindowState.Normal;

                this.Size = new Size(1366, 768);
                this.Scale(new SizeF(0.8f, 0.8f));

                Rectangle screen = Screen.PrimaryScreen.WorkingArea;
                this.Location = new Point(
                    (screen.Width - this.Width) / 2,
                    (screen.Height - this.Height) / 2
                );
            }

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

                    if (IsViewMode && !string.IsNullOrEmpty(_viewingStudentNo))
                    {
                        LoadStudentByStudentNo(conn);
                    }
                    else
                    {
                        LoadLoggedInUser(conn);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user data: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStudentByStudentNo(MySqlConnection conn)
        {
            string query = @"
                SELECT 
                    s.student_id, 
                    s.student_no, 
                    s.student_lrn, 
                    s.first_name, 
                    s.middle_name, 
                    s.last_name, 
                    s.birth_date, 
                    s.age, 
                    s.sex, 
                    s.civil_status, 
                    s.nationality, 
                    c.phone_no, 
                    u.email, 
                    a.block_street, 
                    a.subdivision, 
                    a.barangay, 
                    a.city, 
                    a.province, 
                    a.zipcode, 
                    s.profile_picture,
                    g.first_name AS guardian_first_name,
                    g.last_name AS guardian_last_name, 
                    g.middle_name AS guardian_middle_name,
                    g.contact_number AS guardian_contact, 
                    sg.relationship,
                    g.guardian_id
                FROM students s
                LEFT JOIN users u ON s.user_id = u.user_id
                LEFT JOIN contact_info c ON s.student_id = c.student_id
                LEFT JOIN addresses a ON s.student_id = a.student_id
                LEFT JOIN student_guardians sg ON s.student_id = sg.student_id
                LEFT JOIN parents_guardians g ON sg.guardian_id = g.guardian_id
                WHERE s.student_no = @StudentNo
                ORDER BY s.student_id DESC LIMIT 1";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@StudentNo", _viewingStudentNo);
                LoadStudentData(cmd);
            }
        }

        private void LoadLoggedInUser(MySqlConnection conn)
        {
            string checkUserQuery = "SELECT COUNT(*) FROM students WHERE user_id = @UserID";
            using (var checkCmd = new MySqlCommand(checkUserQuery, conn))
            {
                checkCmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (userExists == 0)
                {
                    CreateDefaultStudentRecord(conn);
                }
            }

            string query = @"
                SELECT 
                    s.student_id, 
                    s.student_no, 
                    s.student_lrn, 
                    s.first_name, 
                    s.middle_name, 
                    s.last_name, 
                    s.birth_date, 
                    s.age, 
                    s.sex, 
                    s.civil_status, 
                    s.nationality, 
                    c.phone_no, 
                    u.email, 
                    a.block_street, 
                    a.subdivision, 
                    a.barangay, 
                    a.city, 
                    a.province, 
                    a.zipcode, 
                    s.profile_picture,
                    g.first_name AS guardian_first_name,
                    g.last_name AS guardian_last_name, 
                    g.middle_name AS guardian_middle_name,
                    g.contact_number AS guardian_contact, 
                    sg.relationship,
                    g.guardian_id
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
                LoadStudentData(cmd);
            }
        }

        private void CreateDefaultStudentRecord(MySqlConnection conn)
        {
            string insertQuery = @"
                INSERT INTO students (user_id, student_no, student_lrn, first_name, middle_name, last_name, birth_date, age, sex, civil_status, nationality) 
                VALUES (@UserID, NULL, NULL, '', '', '', '2000-01-01', 0, 'Male', '', '')";

            using (var insertCmd = new MySqlCommand(insertQuery, conn))
            {
                insertCmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                insertCmd.ExecuteNonQuery();
            }
        }

        private void LoadStudentData(MySqlCommand cmd)
        {
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    _currentStudentId = Convert.ToInt64(reader["student_id"]);

                    TxtStudentNo.Text = reader["student_no"].ToString();
                    TxtStudentLRN.Text = reader["student_lrn"].ToString();
                    TxtFirstName.Text = reader["first_name"].ToString();
                    TxtMiddleName.Text = reader["middle_name"].ToString();
                    TxtLastName.Text = reader["last_name"].ToString();
                    DateBirthPicker.Value = Convert.ToDateTime(reader["birth_date"]);
                    TxtAge.Text = reader["age"].ToString();
                    CmbCivilStatus.Text = reader["civil_status"].ToString();
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
                    currentGuardianId = reader["guardian_id"] != DBNull.Value ?
                        Convert.ToInt64(reader["guardian_id"]) : (long?)null;

                    string nationality = reader["nationality"].ToString();
                    TxtNational.Text = string.IsNullOrWhiteSpace(nationality) ? "Filipino" : nationality;

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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Convert text to proper case first
            TxtFirstName.Text = ToProperCase(TxtFirstName.Text);
            TxtMiddleName.Text = ToProperCase(TxtMiddleName.Text);
            TxtLastName.Text = ToProperCase(TxtLastName.Text);
            TxtNational.Text = ToProperCase(TxtNational.Text);
            TxtBStreet.Text = ToProperCase(TxtBStreet.Text);
            TxtSubCom.Text = ToProperCase(TxtSubCom.Text);
            TxtBrgy.Text = ToProperCase(TxtBrgy.Text);
            TxtCity.Text = ToProperCase(TxtCity.Text);
            TxtProvince.Text = ToProperCase(TxtProvince.Text);
            TxtGuardianFirstName.Text = ToProperCase(TxtGuardianFirstName.Text);
            TxtGuardianMiddleName.Text = ToProperCase(TxtGuardianMiddleName.Text);
            TxtGuardianLastName.Text = ToProperCase(TxtGuardianLastName.Text);
            TxtGuardianRelation.Text = ToProperCase(TxtGuardianRelation.Text);

            if (string.IsNullOrWhiteSpace(TxtNational.Text))
            {
                TxtNational.Text = "Filipino";
            }
            else
            {
                TxtNational.Text = ToProperCase(TxtNational.Text);
            }

            if (!ValidateRequiredFields())
            {
                return; 
            }

            if (!ValidateLRN(false) || !ValidateAge() ||
             !ValidatePhoneNumber(TxtPhoneNo, "Phone number") ||
             !ValidatePhoneNumber(TxtGuardianContact, "Guardian contact number") ||
             !ValidateZipCode())
            {
                return;
            }

            if (!ValidateLRN(true))
            {
                return;
            }

            if (IsViewMode && !SessionManager.IsAdminOrSuperAdmin)
            {
                MessageBox.Show("You don't have permission to edit student information.",
                    "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Save data to database
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string studentQuery = @"
                UPDATE students 
                SET student_no = @StudentNo,
                    student_lrn = @StudentLRN,
                    first_name = @FirstName,
                    middle_name = @MiddleName,
                    last_name = @LastName,
                    birth_date = @BirthDate,
                    age = @Age,
                    sex = @Sex,
                    civil_status = @CivilStatus,
                    nationality = @Nationality
                WHERE student_id = @StudentID";

                    ExecuteQuery(conn, studentQuery,
                        new MySqlParameter("@StudentID", _currentStudentId),
                        new MySqlParameter("@StudentNo", TxtStudentNo.Text),
                        new MySqlParameter("@StudentLRN", TxtStudentLRN.Text),
                        new MySqlParameter("@FirstName", TxtFirstName.Text),
                        new MySqlParameter("@MiddleName", TxtMiddleName.Text),
                        new MySqlParameter("@LastName", TxtLastName.Text),
                        new MySqlParameter("@BirthDate", DateBirthPicker.Value),
                        new MySqlParameter("@Age", TxtAge.Text),
                        new MySqlParameter("@Sex", ChkMale.Checked ? "Male" : "Female"),
                        new MySqlParameter("@CivilStatus", CmbCivilStatus.Text),
                        new MySqlParameter("@Nationality", TxtNational.Text)
                    );

                    // Update contact info
                    string contactQuery = @"
                INSERT INTO contact_info (student_id, phone_no)
                VALUES (@StudentID, @PhoneNo)
                ON DUPLICATE KEY UPDATE phone_no = @PhoneNo";

                    ExecuteQuery(conn, contactQuery,
                        new MySqlParameter("@StudentID", _currentStudentId),
                        new MySqlParameter("@PhoneNo", TxtPhoneNo.Text)
                    );

                    // Update address
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
                        new MySqlParameter("@StudentID", _currentStudentId),
                        new MySqlParameter("@BlockStreet", TxtBStreet.Text),
                        new MySqlParameter("@Subdivision", TxtSubCom.Text),
                        new MySqlParameter("@Barangay", TxtBrgy.Text),
                        new MySqlParameter("@City", TxtCity.Text),
                        new MySqlParameter("@Province", TxtProvince.Text),
                        new MySqlParameter("@Zipcode", TxtZipcode.Text)
                    );

                    // Update guardian info
                    UpdateGuardianInfo(conn);

                    MessageBox.Show("Information updated successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadUserData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Update session if not in view mode
            if (!IsViewMode)
            {
                SessionManager.FirstName = TxtFirstName.Text;
                SessionManager.LastName = TxtLastName.Text;
                WelcomeGreetings();
            }

            // Lock fields after saving
            SetEnabledRecursive(groupBox1, false);
            SetEnabledRecursive(groupBox2, false);
            SetEnabledRecursive(groupBox3, false);
            SetEnabledRecursive(groupBox4, false);
            MessageBox.Show("Fields have been saved and locked.", "Saved",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool ValidateRequiredFields()
        {
            var requiredFields = new List<Control>
                {
                    TxtStudentLRN,
                    TxtLastName,
                    TxtFirstName,
                    TxtAge,
                    CmbCivilStatus,
                    TxtNational,
                    TxtPhoneNo,
                    TxtBStreet,
                    TxtBrgy,
                    TxtCity,
                    TxtProvince,
                    TxtZipcode,
                    TxtGuardianLastName,
                    TxtGuardianFirstName,
                    TxtGuardianRelation,
                    TxtGuardianContact
                };

            foreach (Control field in requiredFields)
            {
                // Special handling: if nationality is blank, default to "Filipino"
                if (field == TxtNational)
                {
                    if (string.IsNullOrWhiteSpace(TxtNational.Text))
                    {
                        TxtNational.Text = "Filipino";
                    }
                    continue; // Skip validation for this field
                }

                // TextBox validation
                if (field is TextBox)
                {
                    TextBox tb = (TextBox)field;
                    if (string.IsNullOrWhiteSpace(tb.Text.Trim()))
                    {
                        string label = (tb.Tag != null) ? tb.Tag.ToString() : tb.Name;
                        MessageBox.Show(label + " is required.",
                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tb.Focus();
                        return false;
                    }
                }

                // ComboBox validation
                if (field is ComboBox)
                {
                    ComboBox cb = (ComboBox)field;
                    if (string.IsNullOrWhiteSpace(cb.Text.Trim()))
                    {
                        string label = (cb.Tag != null) ? cb.Tag.ToString() : cb.Name;
                        MessageBox.Show(label + " is required.",
                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cb.Focus();
                        return false;
                    }
                }
            }

            // Gender check
            if (!ChkMale.Checked && !ChkFemale.Checked)
            {
                MessageBox.Show("Please select a gender.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }


        private void UpdateGuardianInfo(MySqlConnection conn)
        {
            if (currentGuardianId > 0)
            {
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

                currentGuardianId = Convert.ToInt64(new MySqlCommand("SELECT LAST_INSERT_ID()", conn).ExecuteScalar());
            }

            string studentGuardianQuery = @"
                INSERT INTO student_guardians 
                    (student_id, guardian_id, relationship) 
                VALUES 
                    (@StudentID, @GuardianID, @Relationship)
                ON DUPLICATE KEY UPDATE 
                    relationship = VALUES(relationship)";

            ExecuteQuery(conn, studentGuardianQuery,
                new MySqlParameter("@StudentID", _currentStudentId),
                new MySqlParameter("@GuardianID", currentGuardianId),
                new MySqlParameter("@Relationship", TxtGuardianRelation.Text)
            );
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
            // Skip validation when updating from date picker
            TxtAge.Leave -= TxtAge_Leave;  // Temporarily remove the handler

            DateTime birthDate = DateBirthPicker.Value;
            int age = DateTime.Now.Year - birthDate.Year;

            if (DateTime.Now.Month < birthDate.Month ||
                (DateTime.Now.Month == birthDate.Month && DateTime.Now.Day < birthDate.Day))
            {
                age--;
            }

            TxtAge.Text = age.ToString();

            TxtAge.Leave += TxtAge_Leave;  // Reattach the handler
        }

        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void TxtStudentNo_Click(object sender, EventArgs e) { }
        private void TxtStudentLRN_TextChanged(object sender, EventArgs e)
        {
            string digitsOnly = new string(TxtStudentLRN.Text.Where(char.IsDigit).ToArray());

            if (TxtStudentLRN.Text != digitsOnly)
            {
                int cursorPos = TxtStudentLRN.SelectionStart;
                TxtStudentLRN.Text = digitsOnly;
                TxtStudentLRN.SelectionStart = Math.Min(cursorPos, digitsOnly.Length);
            }

            if (TxtStudentLRN.Text.Length > 12)
            {
                TxtStudentLRN.Text = TxtStudentLRN.Text.Substring(0, 12);
                TxtStudentLRN.SelectionStart = 12;
            }
        }

        private void TxtStudentLRN_Leave(object sender, EventArgs e)
        {
            if (!ValidateLRN(true))
            {
                return;
            }

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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsViewMode)
            {
                this.Text = "View Student Information";

                BtnSave.Visible = SessionManager.IsAdminOrSuperAdmin;
                BtnEdit.Visible = SessionManager.IsAdminOrSuperAdmin;
                BtnUpload.Visible = SessionManager.IsAdminOrSuperAdmin;

                BtnExit.Visible = false;
                BtnClose.Visible = true;
                BtnCourses.Visible = false;
                BtnEnrollment.Visible = false;
                BtnHome.Visible = false;
                BtnDataBase.Visible = false;
                BtnPI.Visible = false;
                BtnLogout.Visible = false;
                LblGreetings.Visible = false;
                LblWelcome.Visible = false;
            }
        }

        private void BtnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TxtStudentLRN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; 
                return;
            }

            if (TxtStudentLRN.Text.Length >= 12 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text.Length >= 3 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtPhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text.Length >= 11 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtGuardianContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text.Length >= 11 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtZipcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text.Length >= 4 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private bool ValidateLRN(bool checkDuplicate = true)
        {
            if (string.IsNullOrWhiteSpace(TxtStudentLRN.Text))
            {
                MessageBox.Show("LRN is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtStudentLRN.Focus();
                return false;
            }
            
            bool allDigits = true;
            foreach (char c in TxtStudentLRN.Text)
            {
                if (!char.IsDigit(c))
                {
                    allDigits = false;
                    break;
                }
            }

            if (TxtStudentLRN.Text.Length != 12 || !allDigits)
            {
                MessageBox.Show("LRN must be exactly 12 digits (no letters or symbols).",
                               "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtStudentLRN.Focus();
                return false;
            }

            if (checkDuplicate)
            {
                try
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        if (IsLrnDuplicate(conn, TxtStudentLRN.Text, _currentStudentId))
                        {
                            MessageBox.Show("This LRN already exists. Please enter a LRN.",
                                           "Duplicate LRN", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            TxtStudentLRN.Focus();
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking LRN: " + ex.Message,
                                   "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private bool ValidateAge()
        {
            int age;
            if (!int.TryParse(TxtAge.Text, out age) || age < 15 || age > 100)
            {
                MessageBox.Show("Age must be a number between 15 and 100 (1-3 digits).",
                               "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //TxtAge.Focus();
                return false;
            }
            return true;
        }

        private bool ValidatePhoneNumber(TextBox textBox, string fieldName)
        {
            string phoneNumber = textBox.Text;

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                MessageBox.Show(fieldName + " is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox.Focus();
                return false;
            }

            if (phoneNumber.Length != 11)
            {
                MessageBox.Show(fieldName + " must be exactly 11 digits (e.g. 09123456789).",
                               "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox.Focus();
                return false;
            }

            if (!phoneNumber.StartsWith("09"))
            {
                MessageBox.Show(fieldName + " must start with '09' (e.g. 09123456789).",
                               "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox.Focus();
                return false;
            }
            
            foreach (char c in phoneNumber)
            {
                if (!char.IsDigit(c))
                {
                    MessageBox.Show(fieldName + " must contain only digits (e.g. 09123456789).",
                                   "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox.Focus();
                    return false;
                }
            }

            return true;
        }

        private bool ValidateZipCode()
        {
            if (string.IsNullOrWhiteSpace(TxtZipcode.Text))
            {
                MessageBox.Show("Zip code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtZipcode.Focus();
                return false;
            }

            // Check if all characters are digits (without LINQ)
            bool allDigits = true;
            foreach (char c in TxtZipcode.Text)
            {
                if (!char.IsDigit(c))
                {
                    allDigits = false;
                    break;
                }
            }

            if (TxtZipcode.Text.Length != 4 || !allDigits)
            {
                MessageBox.Show("Zip code must be exactly 4 digits (e.g. 1234).",
                               "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtZipcode.Focus();
                return false;
            }

            return true;
        }



        private void TxtAge_Leave(object sender, EventArgs e)
        { 
            ValidateAge();
        }

        private void TxtPhoneNo_Leave(object sender, EventArgs e)
        {
            ValidatePhoneNumber(TxtPhoneNo, "Phone number");
        }

        private void TxtGuardianContact_Leave(object sender, EventArgs e)
        {
            ValidatePhoneNumber(TxtGuardianContact, "Guardian contact number");
        }

        private void TxtZipcode_Leave(object sender, EventArgs e)
        {
            ValidateZipCode();
        }

        private string ToProperCase(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            System.Globalization.TextInfo textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(text.ToLower());
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.Text = ToProperCase(textBox.Text);
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && char.IsLetter(e.KeyChar))
            {
                if (textBox.Text.Length == 0 || textBox.Text.EndsWith(" "))
                {
                    e.KeyChar = char.ToUpper(e.KeyChar);
                }
                else
                {
                    e.KeyChar = char.ToLower(e.KeyChar);
                }
            }
        }

        private bool IsLrnDuplicate(MySqlConnection conn, string lrn, long currentStudentId)
        {
            string query = "SELECT COUNT(*) FROM students WHERE student_lrn = @StudentLrn AND student_id != @StudentId";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@StudentLrn", lrn);
                cmd.Parameters.AddWithValue("@StudentId", currentStudentId);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
    }
}