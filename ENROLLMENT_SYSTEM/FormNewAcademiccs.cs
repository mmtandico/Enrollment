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
using System.Text.RegularExpressions;

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
            set { TxtCourse.Text = value; } 
        }

        public FormNewAcademiccs()
        {
            InitializeComponent();
            PicBoxID.Image = Properties.Resources.PROFILE;
            PicBoxID.SizeMode = PictureBoxSizeMode.StretchImage;
            loggedInUserId = SessionManager.UserId;
            
        }

        private bool IsValidAcademicYear(string year)
        {
            if (string.IsNullOrWhiteSpace(year))
                return false;

            // More forgiving pattern that allows various separators and optional spaces
            if (!Regex.IsMatch(year, @"^\d{4}[-\s]\d{4}$"))
                return false;

            // Clean the input by removing any whitespace
            string cleanYear = year.Replace(" ", "").Replace("-", "");

            // Should have exactly 8 digits now (4 + 4)
            if (cleanYear.Length != 8 || !cleanYear.All(char.IsDigit))
                return false;

            // Extract the years
            int startYear, endYear;
            if (!int.TryParse(cleanYear.Substring(0, 4), out startYear) ||
                !int.TryParse(cleanYear.Substring(4, 4), out endYear))
                return false;

            // Validate year ranges (adjust as needed)
            const int minYear = 2000;
            const int maxYear = 2100;

            if (startYear < minYear || startYear > maxYear ||
                endYear < minYear || endYear > maxYear)
                return false;

            // End year should be exactly +1 from start year
            return endYear == startYear + 1;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF Files|*.pdf",
                Title = "Select Grade PDF"
            })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    byte[] fileData = File.ReadAllBytes(openFileDialog.FileName);
                    string fileName = Path.GetFileName(openFileDialog.FileName);

                    string savedFileName = SavePdfToDatabase(fileName, fileData);
                    if (savedFileName != null)
                    {
                        // Create a clickable link label
                        LinkLabel linkLabel = new LinkLabel();
                        linkLabel.Text = savedFileName;
                        linkLabel.LinkClicked += (s, args) => ViewPdfFromDatabase();

                        // Position the link label where you want it (replace with your actual UI element)
                        linkLabel.Location = new Point(TxtGradePdfPath.Location.X, TxtGradePdfPath.Location.Y);
                        linkLabel.Size = TxtGradePdfPath.Size;

                        // Remove the existing textbox if needed
                        this.Controls.Remove(TxtGradePdfPath);

                        // Add the link label to your form
                        this.Controls.Add(linkLabel);

                        MessageBox.Show("PDF uploaded and saved successfully.");
                    }
                }
            }
        }

        private void ViewPdfFromDatabase()
        {
            int studentId = SessionManager.StudentId;

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = @"SELECT grade_pdf 
                     FROM student_enrollments 
                     WHERE student_id = @studentId 
                     ORDER BY enrollment_id DESC 
                     LIMIT 1";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@studentId", studentId);
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        byte[] fileData = (byte[])result;
                        string tempPath = Path.Combine(Path.GetTempPath(), "grade_preview.pdf");

                        // Clean up any existing temp file
                        if (File.Exists(tempPath))
                        {
                            File.Delete(tempPath);
                        }

                        File.WriteAllBytes(tempPath, fileData);

                        try
                        {
                            System.Diagnostics.Process.Start(tempPath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error opening PDF: {ex.Message}");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No grade PDF found for this student.");
                    }
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

        private string SavePdfToDatabase(string fileName, byte[] fileData)
        {
            int studentId = SessionManager.StudentId;

            if (studentId <= 0)
            {
                MessageBox.Show("No logged-in student.");
                return null;
            }

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Get the latest enrollment_id for this student
                string getEnrollmentQuery = @"SELECT enrollment_id 
                                  FROM student_enrollments 
                                  WHERE student_id = @studentId 
                                  ORDER BY enrollment_id DESC 
                                  LIMIT 1";

                using (var getCmd = new MySqlCommand(getEnrollmentQuery, conn))
                {
                    getCmd.Parameters.AddWithValue("@studentId", studentId);
                    object result = getCmd.ExecuteScalar();

                    if (result != null)
                    {
                        int enrollmentId = Convert.ToInt32(result);

                        // Update both grade_pdf and grade_pdf_path fields
                        string updateQuery = @"UPDATE student_enrollments 
                                   SET grade_pdf = @pdfData,
                                       grade_pdf_path = @fileName
                                   WHERE enrollment_id = @enrollmentId";

                        using (var updateCmd = new MySqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.Add("@pdfData", MySqlDbType.LongBlob).Value = fileData;
                            updateCmd.Parameters.AddWithValue("@fileName", fileName);
                            updateCmd.Parameters.AddWithValue("@enrollmentId", enrollmentId);
                            updateCmd.ExecuteNonQuery();
                        }
                        return fileName;
                    }
                    else
                    {
                        MessageBox.Show("No enrollment found for this student.");
                        return null;
                    }
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

                if (!IsValidAcademicYear(TxtSchoolYear.Text))
                {
                    MessageBox.Show("Please enter a valid School year in format YYYY-YYYY (e.g., 2023-2024).\n" +
                                  "The second year must be exactly one year after the first.",
                                  "Invalid Academic Year",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
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

                    string semester = CmbSem.SelectedItem.ToString();
                    string yearLevel = CmbYrLvl.SelectedItem.ToString();

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

                        // Insert into academic history with previous_section only
                        string insertHistoryQuery = @"
                        INSERT INTO academic_history (
                            enrollment_id, previous_section
                        ) VALUES (
                            @EnrollmentID, @PreviousSection
                        )";

                        ExecuteQuery(conn, insertHistoryQuery,
                            new MySqlParameter("@EnrollmentID", EnrollmentId),
                            new MySqlParameter("@PreviousSection", TxtPreviousSection.Text)

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
                            (@StudentID, @CourseID, @AcademicYear, @Semester, @YearLevel, 'Payment Pending')"; // Changed status to 'Payment Pending'

                        ExecuteQuery(conn, insertQuery,
                            new MySqlParameter("@StudentID", studentId),
                            new MySqlParameter("@CourseID", courseId),
                            new MySqlParameter("@AcademicYear", TxtSchoolYear.Text),
                            new MySqlParameter("@Semester", semester),
                            new MySqlParameter("@YearLevel", yearLevel)
                        );

                        // Get the newly created enrollment ID
                        long newEnrollmentId = GetLastInsertId(conn);

                        // Calculate total units for this enrollment
                        int totalUnits = CalculateTotalUnits(courseId, yearLevel, semester);

                        // Calculate fees
                        decimal tuitionFee = totalUnits * 150m; // 150 per unit
                        decimal miscFee = 800m; // Fixed miscellaneous fee
                        decimal totalAmountDue = tuitionFee + miscFee;

                        // Create initial payment record
                        string insertPaymentQuery = @"
                        INSERT INTO payments 
                            (enrollment_id, total_units, total_amount_due, amount_paid, is_unifast, payment_method, payment_date)
                        VALUES 
                            (@EnrollmentID, @TotalUnits, @TotalAmountDue, 0, 0, 'Pending', NULL)";

                        ExecuteQuery(conn, insertPaymentQuery,
                            new MySqlParameter("@EnrollmentID", newEnrollmentId),
                            new MySqlParameter("@TotalUnits", totalUnits),
                            new MySqlParameter("@TotalAmountDue", totalAmountDue)
                           
                        );

                        long paymentId = GetLastInsertId(conn);

                        // Insert payment breakdowns
                        string insertBreakdownQuery = @"
                        INSERT INTO payment_breakdowns 
                            (payment_id, fee_type, amount)
                        VALUES 
                            (@PaymentID, @FeeType, @Amount)";

                        // Tuition breakdown
                        ExecuteQuery(conn, insertBreakdownQuery,
                            new MySqlParameter("@PaymentID", paymentId),
                            new MySqlParameter("@FeeType", "Tuition"),
                            new MySqlParameter("@Amount", tuitionFee)
                        );

                        // Miscellaneous breakdown
                        ExecuteQuery(conn, insertBreakdownQuery,
                            new MySqlParameter("@PaymentID", paymentId),
                            new MySqlParameter("@FeeType", "Miscellaneous"),
                            new MySqlParameter("@Amount", miscFee)
                        );

                        MessageBox.Show("New enrollment created successfully! Payment pending.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private long GetLastInsertId(MySqlConnection conn)
        {
            using (var cmd = new MySqlCommand("SELECT LAST_INSERT_ID()", conn))
            {
                return Convert.ToInt64(cmd.ExecuteScalar());
            }
        }

        private int CalculateTotalUnits(int courseId, string yearLevel, string semester)
        {
            int totalUnits = 0;

            string query = @"
                SELECT SUM(s.units) 
                FROM course_subjects cs
                JOIN subjects s ON cs.subject_id = s.subject_id
                WHERE cs.course_id = @CourseId 
                AND cs.year_level = @YearLevel
                AND cs.semester = @Semester";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseId", courseId);
                    cmd.Parameters.AddWithValue("@YearLevel", yearLevel);
                    cmd.Parameters.AddWithValue("@Semester", semester);

                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        totalUnits = Convert.ToInt32(result);
                    }
                }
            }

            return totalUnits;
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
        }

        private void ClearNewEnrollmentFields()
        {
            TxtSchoolYear.Clear();
            TxtSchoolYear.ForeColor = Color.Black;

            TxtPreviousSection.Clear();
            TxtPreviousSection.ForeColor = Color.Black;

            CmbSem.SelectedIndex = -1;
            CmbYrLvl.SelectedIndex = -1;

            CmbCourse.SelectedIndex = -1;
            CmbCourse.Text = "";
        }

        private void LoadUserBasicInfo()
        {
            try
            {
                TxtSchoolYear.Text = "e.g. 20**-20**";
                TxtSchoolYear.ForeColor = Color.Gray;

                TxtPreviousSection.Text = "e.g. BSIT-22-A";
                TxtPreviousSection.ForeColor = Color.Gray;

                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    
                        string query = @"SELECT
                            s.student_id, 
                            IFNULL(s.student_no, '') AS student_no,
                            IFNULL(s.first_name, '') AS first_name,
                            IFNULL(s.middle_name, '') AS middle_name,
                            IFNULL(s.last_name, '') AS last_name,
                            s.profile_picture,
                            IFNULL(c.course_name, '') AS course_name
                        FROM students s
                        LEFT JOIN student_enrollments se ON s.student_id = se.student_id
                        LEFT JOIN courses c ON se.course_id = c.course_id
                        WHERE s.user_id = @UserID
                        ORDER BY se.enrollment_id DESC
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
                                SetComboBoxSelection(CmbCourse, reader["course_name"].ToString());
                                CmbCourse.Enabled = false;
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

        public class ComboboxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
            public override string ToString() => Text;
        }

        private void InitializeComboBoxes()
        {
            CmbSem.Items.Clear();
            CmbSem.Items.Add("1st Sem");
            CmbSem.Items.Add("2nd Sem");

            CmbYrLvl.Items.Clear();
            CmbYrLvl.Items.Add("1st Year");
            CmbYrLvl.Items.Add("2nd Year");
            CmbYrLvl.Items.Add("3rd Year");
            CmbYrLvl.Items.Add("4th Year");
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
                    s.profile_picture,
                    ah.previous_section -- Fetch previous_section from academic_history
                FROM student_enrollments se
                JOIN courses c ON se.course_id = c.course_id
                JOIN students s ON se.student_id = s.student_id
                LEFT JOIN academic_history ah ON se.enrollment_id = ah.enrollment_id -- Join academic_history table
                WHERE se.enrollment_id = @EnrollmentId";

                    Console.WriteLine("SQL Query: " + query); 
                    Console.WriteLine("Enrollment ID: " + EnrollmentId);
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
                                    SetComboBoxSelection(CmbSem, semester);
                                }

                                // Set year level
                                if (!reader.IsDBNull(reader.GetOrdinal("year_level")))
                                {
                                    string yearLevel = reader["year_level"].ToString();
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

                                // Debugging: Log the previous section value
                                Console.WriteLine("Previous Section: " + reader["previous_section"]); // Debugging previous_section

                                // Set previous section (from academic_history)
                                if (!reader.IsDBNull(reader.GetOrdinal("previous_section")))
                                {
                                    TxtPreviousSection.Text = reader["previous_section"].ToString();
                                }
                                else
                                {
                                    TxtPreviousSection.Text = "No previous section available"; // Handle null or empty
                                }
                            }
                            else
                            {
                                Console.WriteLine("No data found for Enrollment ID: " + EnrollmentId); // Log if no data is found
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

            
            int index = comboBox.FindStringExact(value);
            if (index >= 0)
            {
                comboBox.SelectedIndex = index;
                return;
            }

            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (comboBox.Items[i].ToString().Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }

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
            if (TxtPreviousSection.Text == "e.g. BSIT-22-A")
            {
                TxtPreviousSection.Text = "";
                TxtPreviousSection.ForeColor = Color.Black;
            }
        }

        private void TxtPreviewSection_Leave_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtPreviousSection.Text))
            {
                TxtPreviousSection.Text = "e.g. BSIT-22-A";
                TxtPreviousSection.ForeColor = Color.Gray;
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
