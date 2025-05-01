using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Collections.Generic;

namespace Enrollment_System
{
    public partial class FormNewAcademiccs : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
        private readonly long loggedInUserId;

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

            CmbCourse.EnabledChanged += (s, e) =>
            {
                CmbCourse.BackColor = CmbCourse.Enabled ? SystemColors.Window : SystemColors.Control;
            };
        }

        private bool IsValidAcademicYear(string year)
        {
            if (string.IsNullOrWhiteSpace(year)) return false;
            if (!Regex.IsMatch(year, @"^\d{4}[-\s]\d{4}$")) return false;

            string cleanYear = year.Replace(" ", "").Replace("-", "");
            if (cleanYear.Length != 8 || !cleanYear.All(char.IsDigit)) return false;

            int startYear, endYear;
            if (!int.TryParse(cleanYear.Substring(0, 4), out startYear) ||
                !int.TryParse(cleanYear.Substring(4, 4), out endYear)) return false;

            const int minYear = 2000, maxYear = 2100;
            return startYear >= minYear && startYear <= maxYear &&
                   endYear >= minYear && endYear <= maxYear &&
                   endYear == startYear + 1;
        }

        private void ExitButton_Click(object sender, EventArgs e) => this.Close();

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog { Filter = "PDF Files|*.pdf", Title = "Select Grade PDF" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    byte[] fileData = File.ReadAllBytes(openFileDialog.FileName);
                    string fileName = Path.GetFileName(openFileDialog.FileName);

                    if (SavePdfToDatabase(fileName, fileData) != null)
                    {
                        UpdatePdfDisplay(fileName);
                        MessageBox.Show("PDF uploaded and saved successfully.");
                    }
                }
            }
        }

        private void ViewPdfFromDatabase()
        {
            if (SessionManager.TempGradePdf != null && string.IsNullOrEmpty(EnrollmentId))
            {
                ShowTempPdf();
                return;
            }

            if (!string.IsNullOrEmpty(EnrollmentId))
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "SELECT grade_pdf FROM student_enrollments WHERE enrollment_id = @enrollmentId", conn))
                    {
                        cmd.Parameters.AddWithValue("@enrollmentId", EnrollmentId);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            ShowPdf((byte[])result);
                        }
                        else
                        {
                            MessageBox.Show("No grade PDF found for this enrollment.",
                                          "Information",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private void ShowTempPdf()
        {
            try
            {
                string tempPath = Path.Combine(Path.GetTempPath(), "temp_grade_preview.pdf");
                if (File.Exists(tempPath)) File.Delete(tempPath);
                File.WriteAllBytes(tempPath, SessionManager.TempGradePdf);
                System.Diagnostics.Process.Start(tempPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening temporary PDF: {ex.Message}");
            }
        }

        private void ShowPdf(byte[] pdfData)
        {
            try
            {
                string tempPath = Path.Combine(Path.GetTempPath(), "grade_preview.pdf");
                if (File.Exists(tempPath)) File.Delete(tempPath);
                File.WriteAllBytes(tempPath, pdfData);
                System.Diagnostics.Process.Start(tempPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening PDF: {ex.Message}");
            }
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog { Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    PicBoxID.Image = Image.FromFile(openFileDialog.FileName);
                    PicBoxID.SizeMode = PictureBoxSizeMode.StretchImage;
                    SaveProfilePicture(File.ReadAllBytes(openFileDialog.FileName));
                }
            }
        }

        private void SaveProfilePicture(byte[] imageBytes)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                using (var cmd = new MySqlCommand(
                    "UPDATE students SET profile_picture = @ProfilePicture WHERE user_id = @UserID", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                    cmd.Parameters.AddWithValue("@ProfilePicture", imageBytes);
                    cmd.ExecuteNonQuery();
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
            if (SessionManager.StudentId <= 0)
            {
                MessageBox.Show("No logged-in student.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                if (string.IsNullOrEmpty(EnrollmentId))
                {
                    SessionManager.TempGradePdf = fileData;
                    SessionManager.TempGradePdfName = fileName;
                    MessageBox.Show("Grades will be saved when you complete the enrollment form.",
                                  "Temporary Storage",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                    return fileName;
                }

                using (var cmd = new MySqlCommand(
                    @"UPDATE student_enrollments 
                      SET grade_pdf = @pdfData, grade_pdf_path = @fileName
                      WHERE enrollment_id = @enrollmentId", conn))
                {
                    cmd.Parameters.Add("@pdfData", MySqlDbType.LongBlob).Value = fileData;
                    cmd.Parameters.AddWithValue("@fileName", fileName);
                    cmd.Parameters.AddWithValue("@enrollmentId", EnrollmentId);
                    cmd.ExecuteNonQuery();
                }
                return fileName;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    long studentId = GetStudentId(conn);
                    if (studentId == -1) return;

                    int courseId = GetCourseIdFromText(CmbCourse.Text);
                    if (courseId == -1) return;

                    string semester = CmbSem.SelectedItem.ToString();
                    string yearLevel = CmbYrLvl.SelectedItem.ToString();

                    if (!string.IsNullOrEmpty(EnrollmentId))
                    {
                        UpdateExistingEnrollment(conn, studentId, courseId, semester, yearLevel);
                    }
                    else
                    {
                        CreateNewEnrollment(conn, studentId, courseId, semester, yearLevel);
                    }

                    MessageBox.Show("Enrollment saved successfully!", "Success",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving enrollment: " + ex.Message, "Database Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(CmbCourse.Text))
            {
                ShowValidationError("Please select a course.", CmbCourse);
                return false;
            }

            if (!IsValidAcademicYear(TxtSchoolYear.Text))
            {
                MessageBox.Show("Please enter a valid School year in format YYYY-YYYY (e.g., 2023-2024).\n" +
                                "The second year must be exactly one year after the first.",
                                "Invalid Academic Year",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                TxtSchoolYear.Focus();
                return false;
            }

            if (CmbSem.SelectedIndex == -1)
            {
                ShowValidationError("Please select a semester.", CmbSem);
                return false;
            }

            if (CmbYrLvl.SelectedIndex == -1)
            {
                ShowValidationError("Please select a year level.", CmbYrLvl);
                return false;
            }

            bool isFirstYearFirstSem = CmbSem.SelectedIndex == 0 && CmbYrLvl.SelectedIndex == 0;

            if (!isFirstYearFirstSem)
            {
                if (string.IsNullOrWhiteSpace(TxtPreviousSection.Text) ||
                    TxtPreviousSection.Text == "e.g. BSIT-22-A")
                {
                    ShowValidationError("Please provide the previous section.", TxtPreviousSection);
                    return false;
                }
            }

            return true;
        }


        private void ShowValidationError(string message, Control control)
        {
            MessageBox.Show(message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
        }

        private long GetStudentId(MySqlConnection conn)
        {
            using (var cmd = new MySqlCommand("SELECT student_id FROM students WHERE user_id = @UserID", conn))
            {
                cmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                object result = cmd.ExecuteScalar();
                if (result == null)
                {
                    MessageBox.Show("Student record not found.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                return Convert.ToInt64(result);
            }
        }

        private void UpdateExistingEnrollment(MySqlConnection conn, long studentId, int courseId, string semester, string yearLevel)
        {
            // Check for duplicate enrollment
            string checkDuplicateQuery = @"
                SELECT COUNT(*) FROM student_enrollments 
                WHERE student_id = @StudentID AND course_id = @CourseID 
                AND academic_year = @AcademicYear AND semester = @Semester
                AND year_level = @YearLevel AND enrollment_id != @EnrollmentID";

            using (var cmd = new MySqlCommand(checkDuplicateQuery, conn))
            {
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@CourseID", courseId);
                cmd.Parameters.AddWithValue("@AcademicYear", TxtSchoolYear.Text);
                cmd.Parameters.AddWithValue("@Semester", semester);
                cmd.Parameters.AddWithValue("@YearLevel", yearLevel);
                cmd.Parameters.AddWithValue("@EnrollmentID", EnrollmentId);

                if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                {
                    MessageBox.Show("You are already enrolled in this course for the selected term.",
                                  "Duplicate Enrollment",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }
            }

            // Update existing enrollment
            ExecuteQuery(conn,
                @"UPDATE student_enrollments
                SET course_id = @CourseID, academic_year = @AcademicYear,
                    semester = @Semester, year_level = @YearLevel, status = 'Payment Pending'
                WHERE enrollment_id = @EnrollmentID",
                new MySqlParameter("@CourseID", courseId),
                new MySqlParameter("@AcademicYear", TxtSchoolYear.Text),
                new MySqlParameter("@Semester", semester),
                new MySqlParameter("@YearLevel", yearLevel),
                new MySqlParameter("@EnrollmentID", EnrollmentId)
            );

            // Save academic history
            ExecuteQuery(conn,
                @"INSERT INTO academic_history (enrollment_id, previous_section)
                VALUES (@EnrollmentID, @PreviousSection)",
                new MySqlParameter("@EnrollmentID", EnrollmentId),
                new MySqlParameter("@PreviousSection", TxtPreviousSection.Text)
            );

            // Handle PDF if exists in temporary storage
            if (SessionManager.TempGradePdf != null)
            {
                ExecuteQuery(conn,
                    @"UPDATE student_enrollments 
                    SET grade_pdf = @pdfData, grade_pdf_path = @fileName
                    WHERE enrollment_id = @enrollmentId",
                    new MySqlParameter("@pdfData", SessionManager.TempGradePdf),
                    new MySqlParameter("@fileName", SessionManager.TempGradePdfName),
                    new MySqlParameter("@enrollmentId", EnrollmentId)
                );

                SessionManager.TempGradePdf = null;
                SessionManager.TempGradePdfName = null;
            }

            RefreshPdfDisplay(conn);
        }

        private bool IsCurrentlyEnrolled(MySqlConnection conn, long studentId)
        {
            using (var cmd = new MySqlCommand(
                @"SELECT COUNT(*) FROM student_enrollments 
                    WHERE student_id = @StudentID 
                    AND status NOT IN ('Payment Pending', 'Rejected', 'Completed', 'Dropped')", conn))
            {
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private void CreateNewEnrollment(MySqlConnection conn, long studentId, int courseId, string semester, string yearLevel)
        {
            // Check if student has any active enrollment (excluding Completed/Dropped statuses)
            if (IsCurrentlyEnrolled(conn, studentId))
            {
                MessageBox.Show("You have an active enrollment. Please contact administration for course changes.",
                              "Enrollment Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            // Check for duplicate enrollment in the same term
            string checkDuplicateQuery = @"
                SELECT COUNT(*) FROM student_enrollments 
                WHERE student_id = @StudentID
                AND academic_year = @AcademicYear 
                AND semester = @Semester
                AND year_level = @YearLevel
                AND status NOT IN ('Dropped')";  // Allow new enrollment if previous was Dropped

            using (var cmd = new MySqlCommand(checkDuplicateQuery, conn))
            {
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@AcademicYear", TxtSchoolYear.Text);
                cmd.Parameters.AddWithValue("@Semester", semester);
                cmd.Parameters.AddWithValue("@YearLevel", yearLevel);

                if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                {
                    MessageBox.Show("You are already enrolled for the selected term.",
                                  "Duplicate Enrollment",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }
            }

            // Create new enrollment
            ExecuteQuery(conn,
                @"INSERT INTO student_enrollments 
                (student_id, course_id, academic_year, semester, year_level, status)
                VALUES (@StudentID, @CourseID, @AcademicYear, @Semester, @YearLevel, 'Payment Pending')",
                new MySqlParameter("@StudentID", studentId),
                new MySqlParameter("@CourseID", courseId),
                new MySqlParameter("@AcademicYear", TxtSchoolYear.Text),
                new MySqlParameter("@Semester", semester),
                new MySqlParameter("@YearLevel", yearLevel)
            );

            long newEnrollmentId = GetLastInsertId(conn);
            EnrollmentId = newEnrollmentId.ToString();

            // Save academic history
            ExecuteQuery(conn,
                @"INSERT INTO academic_history (enrollment_id, previous_section)
                 VALUES (@EnrollmentID, @PreviousSection)",
                new MySqlParameter("@EnrollmentID", newEnrollmentId),
                new MySqlParameter("@PreviousSection", TxtPreviousSection.Text)
            );

            // Calculate and save payment information
            SavePaymentInfo(conn, newEnrollmentId, courseId, yearLevel, semester);

            // Handle PDF if exists in temporary storage
            if (SessionManager.TempGradePdf != null)
            {
                ExecuteQuery(conn,
                    @"UPDATE student_enrollments 
                    SET grade_pdf = @pdfData, grade_pdf_path = @fileName
                    WHERE enrollment_id = @enrollmentId",
                    new MySqlParameter("@pdfData", SessionManager.TempGradePdf),
                    new MySqlParameter("@fileName", SessionManager.TempGradePdfName),
                    new MySqlParameter("@enrollmentId", newEnrollmentId)
                );

                SessionManager.TempGradePdf = null;
                SessionManager.TempGradePdfName = null;
            }

            RefreshPdfDisplay(conn);
        }

        private void SavePaymentInfo(MySqlConnection conn, long enrollmentId, int courseId, string yearLevel, string semester)
        {
            int totalUnits = CalculateTotalUnits(courseId, yearLevel, semester);
            decimal tuitionFee = totalUnits * 150m;
            decimal miscFee = 800m;
            decimal totalAmountDue = tuitionFee + miscFee;

            // Create payment record
            ExecuteQuery(conn,
                @"INSERT INTO payments 
                (enrollment_id, total_units, total_amount_due, amount_paid, is_unifast, payment_method, payment_date)
                VALUES (@EnrollmentID, @TotalUnits, @TotalAmountDue, 0, 0, 'Payment Pending', NULL)",
                new MySqlParameter("@EnrollmentID", enrollmentId),
                new MySqlParameter("@TotalUnits", totalUnits),
                new MySqlParameter("@TotalAmountDue", totalAmountDue)
            );

            long paymentId = GetLastInsertId(conn);

            // Create payment breakdowns
            ExecuteQuery(conn,
                @"INSERT INTO payment_breakdowns (payment_id, fee_type, amount)
                VALUES (@PaymentID, @FeeType, @Amount)",
                new MySqlParameter("@PaymentID", paymentId),
                new MySqlParameter("@FeeType", "Tuition"),
                new MySqlParameter("@Amount", tuitionFee)
            );

            ExecuteQuery(conn,
                @"INSERT INTO payment_breakdowns (payment_id, fee_type, amount)
                VALUES (@PaymentID, @FeeType, @Amount)",
                new MySqlParameter("@PaymentID", paymentId),
                new MySqlParameter("@FeeType", "Miscellaneous"),
                new MySqlParameter("@Amount", miscFee)
            );
        }

        private void RefreshPdfDisplay(MySqlConnection conn)
        {
            using (var cmd = new MySqlCommand(
                "SELECT grade_pdf_path FROM student_enrollments WHERE enrollment_id = @enrollmentId", conn))
            {
                cmd.Parameters.AddWithValue("@enrollmentId", EnrollmentId);
                object result = cmd.ExecuteScalar();
                string pdfName = (result != null && result != DBNull.Value)
                    ? result.ToString()
                    : "No grade PDF uploaded";

                TxtGradePdfPath.Text = pdfName;
                UpdatePdfDisplay(pdfName);
            }
        }

        private void UpdatePdfDisplay(string fileName)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdatePdfDisplay), fileName);
                return;
            }

            TxtGradePdfPath.Text = fileName;

            // Remove existing link label
            var existingLink = this.Controls.OfType<LinkLabel>().FirstOrDefault(l => l.Tag?.ToString() == "pdfLink");
            if (existingLink != null)
            {
                this.Controls.Remove(existingLink);
                existingLink.Dispose();
            }

            if (!string.IsNullOrWhiteSpace(fileName) &&
                !fileName.Equals("No grade PDF uploaded", StringComparison.OrdinalIgnoreCase))
            {
                var linkLabel = new LinkLabel
                {
                    Text = "View PDF",
                    Location = new Point(
                        TxtGradePdfPath.Left,
                        TxtGradePdfPath.Bottom + 5),
                    Width = TxtGradePdfPath.Width,
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Tag = "pdfLink",
                    LinkColor = Color.Blue,
                    VisitedLinkColor = Color.Purple,
                    ActiveLinkColor = Color.Red,
                    LinkBehavior = LinkBehavior.HoverUnderline
                };
                linkLabel.LinkClicked += (s, args) => ViewPdfFromDatabase();

                this.Controls.Add(linkLabel);
                linkLabel.BringToFront();
                this.PerformLayout();
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
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(
                    @"SELECT SUM(s.units) FROM course_subjects cs
                      JOIN subjects s ON cs.subject_id = s.subject_id
                      WHERE cs.course_id = @CourseId 
                      AND cs.year_level = @YearLevel
                      AND cs.semester = @Semester", conn))
                {
                    cmd.Parameters.AddWithValue("@CourseId", courseId);
                    cmd.Parameters.AddWithValue("@YearLevel", yearLevel);
                    cmd.Parameters.AddWithValue("@Semester", semester);

                    object result = cmd.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToInt32(result) : 0;
                }
            }
        }

        private void FormNewAcademiccs_Load(object sender, EventArgs e)
        {
            InitializeComboBoxes();
            LoadCourseList();
            SetDefaultAcademicYear();

            if (!string.IsNullOrEmpty(EnrollmentId))
            {
                LoadExistingEnrollmentData();
            }
            else
            {
                ClearNewEnrollmentFields();
                LoadUserBasicInfo();

                if (SessionManager.TempGradePdf != null)
                {
                    TxtGradePdfPath.Text = SessionManager.TempGradePdfName;
                    UpdatePdfDisplay(SessionManager.TempGradePdfName);
                }
            }

            if (!string.IsNullOrEmpty(SessionManager.SelectedCourse))
            {
                int index = CmbCourse.FindStringExact(SessionManager.SelectedCourse);
                if (index >= 0) CmbCourse.SelectedIndex = index;
                SessionManager.SelectedCourse = null;
            }
        }

        private void ClearNewEnrollmentFields()
        {
            TxtPreviousSection.Clear();
            TxtPreviousSection.ForeColor = Color.Black;
            CmbSem.SelectedIndex = -1;
            CmbYrLvl.SelectedIndex = -1;
            CmbCourse.SelectedIndex = -1;
            CmbCourse.Text = "";

            var existingLink = this.Controls.OfType<LinkLabel>().FirstOrDefault(l => l.Tag?.ToString() == "pdfLink");
            if (existingLink != null) this.Controls.Remove(existingLink);
        }

        private void LoadUserBasicInfo()
        {
            try
            {
                TxtPreviousSection.Text = "e.g. BSIT-22-A";
                TxtPreviousSection.ForeColor = Color.Gray;

                using (var conn = new MySqlConnection(connectionString))
                using (var cmd = new MySqlCommand(
                    @"SELECT s.student_no, s.first_name, s.middle_name, s.last_name, 
                      s.profile_picture, c.course_name, se.status
                      FROM students s
                      LEFT JOIN student_enrollments se ON s.student_id = se.student_id
                      LEFT JOIN courses c ON se.course_id = c.course_id
                      WHERE s.user_id = @UserID
                      ORDER BY se.enrollment_id DESC LIMIT 1", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@UserID", loggedInUserId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            TxtStudentNo.Text = reader["student_no"].ToString();
                            TxtFirstName.Text = reader["first_name"].ToString();
                            TxtMiddleName.Text = reader["middle_name"].ToString();
                            TxtLastName.Text = reader["last_name"].ToString();
                            SetComboBoxSelection(CmbCourse, reader["course_name"].ToString());

                            if (reader["profile_picture"] != DBNull.Value)
                            {
                                PicBoxID.Image = Image.FromStream(
                                    new MemoryStream((byte[])reader["profile_picture"]));
                            }

                            if (reader["status"] != DBNull.Value)
                            {
                                string status = reader["status"].ToString();
                                if (status != "Payment Pending" && status != "Rejected")
                                {
                                    CmbCourse.Enabled = false;
                                    CmbCourse.BackColor = SystemColors.Control;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user data: " + ex.Message,
                              "Database Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void LoadCourseList()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                using (var cmd = new MySqlCommand("SELECT course_name FROM courses ORDER BY course_name", conn))
                {
                    conn.Open();
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
                            MessageBox.Show("No courses found in the database.",
                                          "Data Error",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading course list: " + ex.Message,
                              "Database Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void InitializeComboBoxes()
        {
            CmbSem.Items.Clear();
            CmbSem.Items.AddRange(new object[] { "1st Sem", "2nd Sem" });

            CmbYrLvl.Items.Clear();
            CmbYrLvl.Items.AddRange(new object[] { "1st Year", "2nd Year", "3rd Year", "4th Year" });
        }

        private void LoadExistingEnrollmentData()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                using (var cmd = new MySqlCommand(
                    @"SELECT se.academic_year, se.semester, se.year_level, c.course_name,
                      s.first_name, s.middle_name, s.last_name, s.student_no, s.profile_picture,
                      ah.previous_section, se.grade_pdf_path, se.status
                      FROM student_enrollments se
                      JOIN courses c ON se.course_id = c.course_id
                      JOIN students s ON se.student_id = s.student_id
                      LEFT JOIN academic_history ah ON se.enrollment_id = ah.enrollment_id
                      WHERE se.enrollment_id = @EnrollmentId", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@EnrollmentId", EnrollmentId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Set basic fields
                            TxtSchoolYear.Text = reader["academic_year"].ToString();
                            SetComboBoxSelection(CmbSem, reader["semester"].ToString());
                            SetComboBoxSelection(CmbYrLvl, reader["year_level"].ToString());
                            SetComboBoxSelection(CmbCourse, reader["course_name"].ToString());

                            string status = reader["status"].ToString();
                            if (status != "Payment Pending" && status != "Rejected")
                            {
                                CmbCourse.Enabled = false;
                                CmbCourse.BackColor = SystemColors.Control;
                            }

                            // Set student info
                            TxtStudentNo.Text = reader["student_no"].ToString();
                            TxtFirstName.Text = reader["first_name"].ToString();
                            TxtMiddleName.Text = reader["middle_name"].ToString();
                            TxtLastName.Text = reader["last_name"].ToString();

                            // Set profile picture
                            if (reader["profile_picture"] != DBNull.Value)
                            {
                                PicBoxID.Image = Image.FromStream(
                                    new MemoryStream((byte[])reader["profile_picture"]));
                            }

                            // Set previous section
                            TxtPreviousSection.Text = reader.IsDBNull(reader.GetOrdinal("previous_section"))
                                ? "No previous section available"
                                : reader["previous_section"].ToString();

                            // Set PDF path
                            string pdfPath = reader.IsDBNull(reader.GetOrdinal("grade_pdf_path"))
                                ? "No grade PDF uploaded"
                                : reader["grade_pdf_path"].ToString();

                            TxtGradePdfPath.Text = pdfPath;
                            UpdatePdfDisplay(pdfPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading enrollment data: " + ex.Message,
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void SetComboBoxSelection(ComboBox comboBox, string value)
        {
            if (string.IsNullOrEmpty(value)) return;

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

        private void SetDefaultAcademicYear()
        {
            int currentYear = DateTime.Now.Year;
            int startYear = DateTime.Now.Month >= 1 ? currentYear : currentYear - 1;
            TxtSchoolYear.Text = $"{startYear}-{startYear + 1}";
            TxtSchoolYear.ForeColor = Color.Black;
        }

        private int GetCourseIdFromText(string courseText)
        {
            if (string.IsNullOrWhiteSpace(courseText)) return -1;

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand("SELECT course_id FROM courses WHERE course_name = @CourseName", conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@CourseName", courseText);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
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

        // Empty methods preserved to prevent Visual Studio 2015 errors
        private void TxtCourse_TextChanged(object sender, EventArgs e) { }
        private void TxtPreviewSection_TextChanged(object sender, EventArgs e) { }
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
        private void TxtSchoolYear_Enter(object sender, EventArgs e) { }
        private void TxtSchoolYear_Leave(object sender, EventArgs e) { }
        public void SetText(string message) { }
    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public override string ToString() => Text;
    }
}