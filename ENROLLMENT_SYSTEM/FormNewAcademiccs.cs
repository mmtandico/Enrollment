using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace Enrollment_System
{
    public partial class FormNewAcademiccs : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
        private long loggedInUserId;
        public string EnrollmentId { get; set; }
        public Dictionary<string, string> StudentData { get; set; }
        public string CourseText


        {
            get { return TxtCourse.Text; }
            set { TxtCourse.Text = value; } // Update TxtCourse with the passed value
        }

        public FormNewAcademiccs()
        {
            InitializeComponent();
            PicBoxID.Image = Properties.Resources.PROFILE;
            PicBoxID.SizeMode = PictureBoxSizeMode.StretchImage;
            loggedInUserId = SessionManager.UserId;
        }



        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF Files|*.pdf"
            })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    byte[] fileData = File.ReadAllBytes(openFileDialog.FileName);
                    string fileName = Path.GetFileName(openFileDialog.FileName);

                    //SavePdfToDatabase(fileName, fileData); // call to save
                    MessageBox.Show("PDF uploaded successfully.");
                }
            }
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    PicBoxID.Image = Image.FromFile(openFileDialog.FileName);
                    PicBoxID.SizeMode = PictureBoxSizeMode.StretchImage;

                    //byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);

                    //SaveProfilePicture(imageBytes);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(CmbCourse.Text))
                {
                    MessageBox.Show("Please select a course.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CmbCourse.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtSchoolYear.Text) || TxtSchoolYear.Text == "e.g. 20**-20**")
                {
                    MessageBox.Show("Please enter a valid academic year.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtSchoolYear.Focus();
                    return;
                }

                if (CmbSem.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a semester.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CmbSem.Focus();
                    return;
                }

                if (CmbYrLvl.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a year level.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CmbYrLvl.Focus();
                    return;
                }

                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    long studentId = -1;

                    // Get the student ID
                    string getStudentQuery = "SELECT student_id FROM students WHERE user_id = @UserID";
                    using (var cmd = new MySqlCommand(getStudentQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        object result = cmd.ExecuteScalar();
                        if (result == null)
                        {
                            MessageBox.Show("Student record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        studentId = Convert.ToInt64(result);
                    }

                    // Get the course ID
                    int courseId = GetCourseIdFromText(CmbCourse.Text);
                    if (courseId == -1)
                    {
                        MessageBox.Show("Invalid course selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Get clean semester and year level values
                    string semester = CmbSem.SelectedItem.ToString().Split(' ')[0]; // Takes "1st" from "1st semester"
                    string yearLevel = CmbYrLvl.SelectedItem.ToString().Split(' ')[0]; // Takes "1st" from "1st year"

                    // Check for duplicate enrollment (only for new enrollments)
                    if (string.IsNullOrEmpty(EnrollmentId))
                    {
                        string checkDuplicateQuery = @"
                            SELECT COUNT(*) 
                            FROM student_enrollments 
                            WHERE student_id = @StudentID 
                            AND course_id = @CourseID 
                            AND academic_year = @AcademicYear 
                            AND semester = @Semester
                            AND year_level = @YearLevel";

                        using (var cmd = new MySqlCommand(checkDuplicateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@StudentID", studentId);
                            cmd.Parameters.AddWithValue("@CourseID", courseId);
                            cmd.Parameters.AddWithValue("@AcademicYear", TxtSchoolYear.Text);
                            cmd.Parameters.AddWithValue("@Semester", semester);
                            cmd.Parameters.AddWithValue("@YearLevel", yearLevel);

                            int duplicateCount = Convert.ToInt32(cmd.ExecuteScalar());
                            if (duplicateCount > 0)
                            {
                                MessageBox.Show("You are already enrolled in this course for the selected term.", "Duplicate Enrollment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                    // Handle either update or insert
                    if (!string.IsNullOrEmpty(EnrollmentId))
                    {
                        // Update existing enrollment
                        string updateQuery = @"
                            UPDATE student_enrollments
                            SET course_id = @CourseID,
                                academic_year = @AcademicYear,
                                semester = @Semester,
                                year_level = @YearLevel,
                                status = 'Pending'
                            WHERE enrollment_id = @EnrollmentID";

                        ExecuteQuery(conn, updateQuery,
                            new MySqlParameter("@CourseID", courseId),
                            new MySqlParameter("@AcademicYear", TxtSchoolYear.Text),
                            new MySqlParameter("@Semester", semester),
                            new MySqlParameter("@YearLevel", yearLevel),
                            new MySqlParameter("@EnrollmentID", EnrollmentId)
                        );

                        MessageBox.Show("Enrollment updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Create new enrollment
                        string insertQuery = @"
                            INSERT INTO student_enrollments 
                                (student_id, course_id, academic_year, semester, year_level, status)
                            VALUES 
                                (@StudentID, @CourseID, @AcademicYear, @Semester, @YearLevel, 'Pending')";

                        ExecuteQuery(conn, insertQuery,
                            new MySqlParameter("@StudentID", studentId),
                            new MySqlParameter("@CourseID", courseId),
                            new MySqlParameter("@AcademicYear", TxtSchoolYear.Text),
                            new MySqlParameter("@Semester", semester),
                            new MySqlParameter("@YearLevel", yearLevel)
                        );

                        MessageBox.Show("New enrollment created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // Update session data
                    SessionManager.FirstName = TxtFirstName.Text;
                    SessionManager.LastName = TxtLastName.Text;

                    // Close the form with OK result
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving enrollment: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormNewAcademiccs_Load(object sender, EventArgs e)
        {
            InitializeComboBoxes();
            LoadCourseList();

            if (!string.IsNullOrEmpty(EnrollmentId))
            {
                // Existing enrollment - load the data
                LoadExistingEnrollmentData();
            }
            else
            {
                // New enrollment - clear fields and load basic user data
                ClearNewEnrollmentFields();
                LoadUserBasicInfo(); // Modified to only load basic info, not enrollment data
            }

            if (!string.IsNullOrEmpty(SessionManager.SelectedCourse))
            {
                int index = CmbCourse.FindStringExact(SessionManager.SelectedCourse);
                if (index >= 0)
                {
                    CmbCourse.SelectedIndex = index;
                }
                SessionManager.SelectedCourse = null;
            }

            // Set placeholder texts
            TxtPreviewSection.Text = "e.g. BSIT-22A";
            TxtPreviewSection.ForeColor = Color.Gray;
        }

        private void ClearNewEnrollmentFields()
        {
            // Clear academic year
            TxtSchoolYear.Clear();
            TxtSchoolYear.ForeColor = Color.Black;

            // Reset semester and year level comboboxes
            CmbSem.SelectedIndex = -1;
            CmbYrLvl.SelectedIndex = -1;

            // Clear course selection
            CmbCourse.SelectedIndex = -1;
            CmbCourse.Text = "";
        }

        private void LoadUserBasicInfo()
        {
            try
            {
                TxtSchoolYear.Text = "e.g. 20**-20**";
                TxtSchoolYear.ForeColor = Color.Gray;

                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"SELECT 
                        s.student_id, 
                        IFNULL(s.student_no, '') AS student_no, 
                        IFNULL(s.first_name, '') AS first_name, 
                        IFNULL(s.middle_name, '') AS middle_name, 
                        IFNULL(s.last_name, '') AS last_name,
                        s.profile_picture
                    FROM students s
                    WHERE s.user_id = @UserID
                    LIMIT 1";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Only load basic student info
                                TxtStudentNo.Text = reader["student_no"].ToString();
                                TxtFirstName.Text = reader["first_name"].ToString();
                                TxtMiddleName.Text = reader["middle_name"].ToString();
                                TxtLastName.Text = reader["last_name"].ToString();

                                // Handle profile picture
                                if (reader["profile_picture"] != DBNull.Value)
                                {
                                    byte[] imageBytes = (byte[])reader["profile_picture"];
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        PicBoxID.Image = Image.FromStream(ms);
                                    }
                                }
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

        private void LoadCourseList()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT course_name FROM courses ORDER BY course_name";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            CmbCourse.Items.Clear();

                            while (reader.Read())
                            {
                                string courseName = reader["course_name"].ToString();
                                if (!string.IsNullOrEmpty(courseName))
                                {
                                    CmbCourse.Items.Add(courseName);
                                }
                            }

                            if (CmbCourse.Items.Count == 0)
                            {
                                MessageBox.Show("No courses found in the database.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading course list: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper class for ComboBox items
        public class ComboboxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
            public override string ToString() => Text;
        }

        private void InitializeComboBoxes()
        {
            // Initialize semester ComboBox
            CmbSem.Items.Clear();
            CmbSem.Items.Add("1st");
            CmbSem.Items.Add("2nd");

            // Initialize year level ComboBox
            CmbYrLvl.Items.Clear();
            CmbYrLvl.Items.Add("1st");
            CmbYrLvl.Items.Add("2nd");
            CmbYrLvl.Items.Add("3rd");
            CmbYrLvl.Items.Add("4th");
        }

        private void LoadUserData()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if student exists
                    string checkUserQuery = "SELECT COUNT(*) FROM students WHERE user_id = @UserID";
                    using (var checkCmd = new MySqlCommand(checkUserQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (userExists == 0)
                        {
                            string insertQuery = @"INSERT INTO students (user_id, student_no, student_lrn, first_name, middle_name, last_name, birth_date, age, sex, civil_status, nationality) 
                                      VALUES (@UserID, '', '', '', '', '', '2000-01-01', 0, 'Male', '', '')";

                            using (var insertCmd = new MySqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // Main query
                    string query = @"SELECT 
                            s.student_id, 
                            IFNULL(s.student_no, '') AS student_no, 
                            IFNULL(s.first_name, '') AS first_name, 
                            IFNULL(s.middle_name, '') AS middle_name, 
                            IFNULL(s.last_name, '') AS last_name, 
                            IFNULL(se.academic_year, '') AS academic_year, 
                            IFNULL(se.semester, '') AS semester, 
                            IFNULL(se.year_level, '') AS year_level, 
                            IFNULL(se.status, '') AS status,
                            IFNULL(c.course_code, '') AS course_code,
                            IFNULL(c.course_name, '') AS course_name,
                            s.profile_picture
                        FROM students s
                        LEFT JOIN student_enrollments se ON s.student_id = se.student_id
                        LEFT JOIN courses c ON se.course_id = c.course_id
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
                                TxtFirstName.Text = reader["first_name"].ToString();
                                TxtMiddleName.Text = reader["middle_name"].ToString();
                                TxtLastName.Text = reader["last_name"].ToString();
                                TxtSchoolYear.Text = reader["academic_year"].ToString();
                                SetComboBoxSelection(CmbCourse, reader["course_name"].ToString());
                                SetComboBoxSelection(CmbSem, reader["semester"].ToString());
                                SetComboBoxSelection(CmbYrLvl, reader["year_level"].ToString());


                                // Handle course selection
                                if (!reader.IsDBNull(reader.GetOrdinal("course_name")))
                                {
                                    string courseName = reader["course_name"].ToString();
                                    int index = CmbCourse.FindStringExact(courseName);
                                    if (index >= 0)
                                    {
                                        CmbCourse.SelectedIndex = index;
                                    }
                                    else
                                    {
                                        CmbCourse.Text = courseName;
                                    }
                                }

                                // Handle profile picture
                                if (reader["profile_picture"] != DBNull.Value)
                                {
                                    byte[] imageBytes = (byte[])reader["profile_picture"];
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        PicBoxID.Image = Image.FromStream(ms);
                                    }
                                }
                                else
                                {
                                    PicBoxID.Image = Properties.Resources.PROFILE;
                                }
                                PicBoxID.SizeMode = PictureBoxSizeMode.StretchImage;
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

        private void LoadExistingEnrollmentData()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            se.academic_year,
                            se.semester,
                            se.year_level,
                            c.course_name,
                            s.first_name,
                            s.middle_name,
                            s.last_name,
                            s.student_no,
                            s.profile_picture
                        FROM student_enrollments se
                        JOIN courses c ON se.course_id = c.course_id
                        JOIN students s ON se.student_id = s.student_id
                        WHERE se.enrollment_id = @EnrollmentId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EnrollmentId", EnrollmentId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Set academic year
                                if (!reader.IsDBNull(reader.GetOrdinal("academic_year")))
                                {
                                    TxtSchoolYear.Text = reader["academic_year"].ToString();
                                }

                                // Set semester
                                if (!reader.IsDBNull(reader.GetOrdinal("semester")))
                                {
                                    string semester = reader["semester"].ToString();
                                    semester = semester.Split(' ')[0]; // Takes first part if value is "1st semester"
                                    SetComboBoxSelection(CmbSem, semester);
                                }

                                // Set year level
                                if (!reader.IsDBNull(reader.GetOrdinal("year_level")))
                                {
                                    string yearLevel = reader["year_level"].ToString();
                                    yearLevel = yearLevel.Split(' ')[0]; // Takes first part if value is "1st year"
                                    SetComboBoxSelection(CmbYrLvl, yearLevel);
                                }

                                // Set course
                                if (!reader.IsDBNull(reader.GetOrdinal("course_name")))
                                {
                                    string courseName = reader["course_name"].ToString();
                                    SetComboBoxSelection(CmbCourse, courseName);
                                }

                                // Set student info
                                TxtStudentNo.Text = reader["student_no"].ToString();
                                TxtFirstName.Text = reader["first_name"].ToString();
                                TxtMiddleName.Text = reader["middle_name"].ToString();
                                TxtLastName.Text = reader["last_name"].ToString();

                                // Handle profile picture
                                if (reader["profile_picture"] != DBNull.Value)
                                {
                                    byte[] imageBytes = (byte[])reader["profile_picture"];
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        PicBoxID.Image = Image.FromStream(ms);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading enrollment data: " + ex.Message,
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetComboBoxSelection(ComboBox comboBox, string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            // Try exact match first
            int index = comboBox.FindStringExact(value);
            if (index >= 0)
            {
                comboBox.SelectedIndex = index;
                return;
            }

            // Try partial match (case insensitive)
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (comboBox.Items[i].ToString().Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }

            // If no match found, set text directly
            comboBox.Text = value;
        }
        public void SetText(string message)
        {
            TxtCourse.Text = message;
        }

        private void TxtCourse_TextChanged(object sender, EventArgs e)
        {

        }

        private int GetCourseIdFromText(string courseText)
        {
            if (string.IsNullOrWhiteSpace(courseText))
                return -1;

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT course_id FROM courses WHERE course_name = @CourseName";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseName", courseText);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
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

        private void TxtPreviewSection_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtPreviewSection_Enter_1(object sender, EventArgs e)
        {
            if (TxtPreviewSection.Text == "e.g. BSIT-22A")
            {
                TxtPreviewSection.Text = "";
                TxtPreviewSection.ForeColor = Color.Black;
            }
        }

        private void TxtPreviewSection_Leave_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtPreviewSection.Text))
            {
                TxtPreviewSection.Text = "e.g. BSIT-22A";
                TxtPreviewSection.ForeColor = Color.Gray;
            }
        }

        private void TxtSchoolYear_Enter(object sender, EventArgs e)
        {
            if (TxtSchoolYear.Text == "e.g. 20**-20**")
            {
                TxtSchoolYear.Text = "";
                TxtSchoolYear.ForeColor = Color.Black;
            }
        }

        private void TxtSchoolYear_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtSchoolYear.Text))
            {
                TxtSchoolYear.Text = "e.g. 20**-20**";
                TxtSchoolYear.ForeColor = Color.Gray;
            }
        }
    }
}
