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
using System.Drawing.Drawing2D;

namespace Enrollment_System
{
    public partial class FormEnrollment : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
        //private FormNewAcademiccs formNewAcademiccsInstance = null;

        private FormCourse mainForm;

        public FormEnrollment()
        {
            InitializeComponent();



            this.DoubleBuffered = true;

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            UIHelper.ApplyAdminVisibility(BtnDataBase);
            this.Activated += FormEnrollment_Activated;
            //LoadEnrollmentData();
            DataGridEnrollment.CellClick += DataGridEnrollment_CellClick;
            //DataGridEnrollment.CellContentClick += DataGridEnrollment_CellContentClick;
            DataGridEnrollment.CellMouseEnter += DataGridEnrollment_CellMouseEnter;
            DataGridEnrollment.CellMouseLeave += DataGridEnrollment_CellMouseLeave;

            DataGridEnrollment.Sorted += DataGridNewEnrollment_Sorted;
            DataGridPayment.Sorted += DataGridPayment_Sorted;
            DataGridSubjects.Sorted += DataGridSubjects_Sorted;


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

            foreach (DataGridViewColumn col in DataGridEnrollment.Columns)
            {
                col.Frozen = false;
            }
            DataGridEnrollment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
           // DataGridPayment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridEnrollment.Columns["ColOpen"].Width = 50;
            DataGridEnrollment.Columns["ColClose"].Width = 50;
            DataGridEnrollment.Columns["ColOpen"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridEnrollment.Columns["ColClose"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridEnrollment.RowTemplate.Height = 40;
            DataGridEnrollment.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;


            DataGridEnrollment.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridEnrollment.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewImageColumn colOpen = (DataGridViewImageColumn)DataGridEnrollment.Columns["ColOpen"];
            colOpen.ImageLayout = DataGridViewImageCellLayout.Zoom;

            DataGridViewImageColumn colClose = (DataGridViewImageColumn)DataGridEnrollment.Columns["ColClose"];
            colClose.ImageLayout = DataGridViewImageCellLayout.Zoom;

            foreach (DataGridViewColumn col in DataGridEnrollment.Columns)
            {
                //col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }

            /////////////////////////////////////////
            foreach (DataGridViewColumn col in DataGridPayment.Columns)
            {
                col.Frozen = false;
            }
            DataGridPayment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
           // DataGridPayment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridPayment.Columns["ColPay"].Width = 50;
            DataGridPayment.Columns["ColDelete"].Width = 50;
            DataGridPayment.Columns["ColPay"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPayment.Columns["ColDelete"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPayment.RowTemplate.Height = 40;
            DataGridPayment.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            DataGridPayment.RowTemplate.Height = 40;
            DataGridPayment.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            DataGridPayment.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridPayment.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewImageColumn ColPay = (DataGridViewImageColumn)DataGridEnrollment.Columns["ColPay"];
            colOpen.ImageLayout = DataGridViewImageCellLayout.Zoom;

            DataGridViewImageColumn ColDelete = (DataGridViewImageColumn)DataGridEnrollment.Columns["ColDelete"];
            colClose.ImageLayout = DataGridViewImageCellLayout.Zoom;

            foreach (DataGridViewColumn col in DataGridPayment.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }

            /////////////////////////////////////////////
            foreach (DataGridViewColumn col in DataGridSubjects.Columns)
            {
                col.Frozen = false;
            }
            DataGridSubjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridSubjects.RowTemplate.Height = 40;
            DataGridSubjects.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            DataGridSubjects.RowTemplate.Height = 40;
            DataGridSubjects.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            DataGridSubjects.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridSubjects.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            foreach (DataGridViewColumn col in DataGridSubjects.Columns)
            {
                //col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
        }

        private void DataGridPayment_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (DataGridPayment.Columns[e.ColumnIndex].Name == "ColPay" ||
                DataGridPayment.Columns[e.ColumnIndex].Name == "ColDelete")
            {
                // Set to null to use the column's default image
                e.Value = null;
            }
        }

        private void DataGridPayment_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if ((e.Exception is FormatException || e.Exception is InvalidCastException) &&
                (DataGridPayment.Columns[e.ColumnIndex].Name == "ColPay" ||
                 DataGridPayment.Columns[e.ColumnIndex].Name == "ColDelete" ||
                 DataGridPayment.Columns[e.ColumnIndex].Name == "is_unifast"))
            {
                e.ThrowException = false;
            }
        }

        public void RefreshPaymentData()
        {
            LoadStudentPayments();
        }

        private void DataGridEnrollment_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string columnName = DataGridEnrollment.Columns[e.ColumnIndex].Name;
                if (columnName == "ColOpen" || columnName == "ColClose")
                {
                    DataGridEnrollment.Cursor = Cursors.Hand;
                }
            }
        }

        private void DataGridEnrollment_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string columnName = DataGridEnrollment.Columns[e.ColumnIndex].Name;
                if (columnName == "ColOpen" || columnName == "ColClose")
                {
                    DataGridEnrollment.Cursor = Cursors.Default;
                }
            }
        }


        private void FormEnrollment_Activated(object sender, EventArgs e)
        {
            LoadEnrollmentData();
            LoadStudentPayments();
        }


        private void FormEnrollment_Load(object sender, EventArgs e)
        {
            LoadEnrollmentData();
            LoadStudentPayments();
            DataGridPayment.DataError += DataGridPayment_DataError;

        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnCourses_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormCourse().Show();
        }

        private void SwitchForm(Form newForm)
        {
            this.Hide();
            newForm.Show();
            this.Close();
        }

        private void BtnHome_Click(object sender, EventArgs e)
        {
            SwitchForm(new FormHome());
        }

        private void BtnPI_Click(object sender, EventArgs e)
        {
            SwitchForm(new FormPersonalInfo());
        }

        private void BtnDataBase_Click(object sender, EventArgs e)
        {
            SwitchForm(new FormDatabaseInfo());
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                new FormLogin().Show();
                this.Close();
            }

        }

        private void BtnAddAcademic_Click(object sender, EventArgs e)
        {
            // First check if personal info is complete
            if (!ValidationHelper.IsPersonalInfoComplete(SessionManager.UserId))
            {
                ValidationHelper.ShowValidationError(this);
                SwitchForm(new FormPersonalInfo());
                return;
            }

            string status = GetActiveEnrollmentStatus();

            // Check for existing pending enrollments
            if (!string.IsNullOrEmpty(status))
            {
                MessageBox.Show($"You already have an active enrollment with status: {status}.\n" +
                                "Please wait for it to be processed before creating a new one.",
                                "Pending Enrollment Exists",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            using (FormNewAcademiccs formNewAcademiccs = new FormNewAcademiccs())
            {
                formNewAcademiccs.StartPosition = FormStartPosition.CenterParent;
                DialogResult result = formNewAcademiccs.ShowDialog();

                if (result == DialogResult.OK)
                {
                    LoadEnrollmentData();
                    LoadStudentPayments();
                }
            }
        }

        private string GetActiveEnrollmentStatus()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT status 
                             FROM student_enrollments 
                             WHERE student_id = @StudentId 
                             AND status IN ('Pending', 'Payment Pending', 'Enrolled') 
                             LIMIT 1";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", SessionManager.StudentId);
                        var result = cmd.ExecuteScalar();

                        return result?.ToString(); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking enrollment status: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }


        private bool IsPersonalInfoComplete()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            s.student_no, s.student_lrn, s.first_name, s.last_name, s.birth_date,
                            s.sex, s.nationality, c.phone_no, a.barangay, a.city, a.province,
                            g.first_name AS guardian_first, g.last_name AS guardian_last,
                            g.contact_number AS guardian_contact
                        FROM students s
                        LEFT JOIN contact_info c ON s.student_id = c.student_id
                        LEFT JOIN addresses a ON s.student_id = a.student_id
                        LEFT JOIN student_guardians sg ON s.student_id = sg.student_id
                        LEFT JOIN parents_guardians g ON sg.guardian_id = g.guardian_id
                        WHERE s.user_id = @UserID";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", SessionManager.UserId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Check all required fields - handle both NULL and empty strings
                                if (reader["student_no"] == DBNull.Value || string.IsNullOrWhiteSpace(reader["student_no"].ToString()))
                                    return false;
                                if (reader["student_lrn"] == DBNull.Value || string.IsNullOrWhiteSpace(reader["student_lrn"].ToString()))
                                    return false;
                                if (reader["first_name"] == DBNull.Value || string.IsNullOrWhiteSpace(reader["first_name"].ToString()))
                                    return false;
                                if (reader["last_name"] == DBNull.Value || string.IsNullOrWhiteSpace(reader["last_name"].ToString()))
                                    return false;
                                if (reader["birth_date"] == DBNull.Value)
                                    return false;
                                if (reader["sex"] == DBNull.Value || string.IsNullOrWhiteSpace(reader["sex"].ToString()))
                                    return false;
                                if (reader["phone_no"] == DBNull.Value || string.IsNullOrWhiteSpace(reader["phone_no"].ToString()))
                                    return false;
                                if (reader["barangay"] == DBNull.Value || string.IsNullOrWhiteSpace(reader["barangay"].ToString()))
                                    return false;
                                if (reader["city"] == DBNull.Value || string.IsNullOrWhiteSpace(reader["city"].ToString()))
                                    return false;
                                if (reader["province"] == DBNull.Value || string.IsNullOrWhiteSpace(reader["province"].ToString()))
                                    return false;
                                if (reader["guardian_first"] == DBNull.Value || string.IsNullOrWhiteSpace(reader["guardian_first"].ToString()))
                                    return false;
                                if (reader["guardian_last"] == DBNull.Value || string.IsNullOrWhiteSpace(reader["guardian_last"].ToString()))
                                    return false;
                                if (reader["guardian_contact"] == DBNull.Value || string.IsNullOrWhiteSpace(reader["guardian_contact"].ToString()))
                                    return false;

                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error verifying personal information: " + ex.Message,
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }

            return false;
        }

        private void LoadEnrollmentData()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            se.enrollment_id,
                            s.student_id,
                            s.student_no,
                            s.last_name,
                            s.first_name,
                            s.middle_name,
                            c.course_code,
                            se.academic_year,
                            se.semester,
                            se.year_level,
                            se.status
                        FROM student_enrollments se
                        INNER JOIN students s ON se.student_id = s.student_id
                        INNER JOIN courses c ON se.course_id = c.course_id
                        WHERE s.user_id = @UserID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", SessionManager.UserId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {

                            DataGridEnrollment.Rows.Clear();
                            int rowNumber = 1;

                            while (reader.Read())
                            {
                                DataGridEnrollment.Rows.Add(
                                    reader["enrollment_id"].ToString(),
                                    rowNumber.ToString(),
                                    reader["student_no"].ToString(),
                                    reader["last_name"].ToString(),
                                    reader["first_name"].ToString(),
                                    reader["middle_name"].ToString(),
                                    reader["course_code"].ToString(),
                                    reader["academic_year"].ToString(),
                                    reader["semester"].ToString(),
                                    reader["year_level"].ToString(),
                                    reader["status"].ToString()
                                );
                                rowNumber++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading enrollment data: " + ex.Message);
            }

            foreach (DataGridViewRow row in DataGridEnrollment.Rows)
            {
                string status = row.Cells[10].Value?.ToString()?.ToLower() ?? "";

                if (status == "enrolled" || status == "completed")
                {

                    var editCell = row.Cells["ColOpen"];
                    editCell.ReadOnly = true;
                    editCell.Style.ForeColor = Color.Gray;
                    editCell.Style.BackColor = Color.LightGray;

                    var deleteCell = row.Cells["ColClose"];
                    deleteCell.ReadOnly = true;
                    deleteCell.Style.ForeColor = Color.Gray;
                    deleteCell.Style.BackColor = Color.LightGray;
                }
                else
                {

                    row.Cells["ColOpen"].ReadOnly = false;
                    row.Cells["ColOpen"].Style.ForeColor = Color.Black;
                    row.Cells["ColOpen"].Style.BackColor = Color.White;

                    row.Cells["ColClose"].ReadOnly = false;
                    row.Cells["ColClose"].Style.ForeColor = Color.Black;
                    row.Cells["ColClose"].Style.BackColor = Color.White;
                }
            }
        }

        private void DataGridEnrollment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string columnName = DataGridEnrollment.Columns[e.ColumnIndex].Name;
                if (columnName == "ColOpen" || columnName == "ColClose")
                {
                    return;
                }

                var selectedRow = DataGridEnrollment.Rows[e.RowIndex];

                string studentNo = selectedRow.Cells[2].Value?.ToString() ?? "";
                string lastName = selectedRow.Cells[3].Value?.ToString() ?? "";
                string firstName = selectedRow.Cells[4].Value?.ToString() ?? "";
                string middleName = selectedRow.Cells[5].Value?.ToString() ?? "";
                string courseCode = selectedRow.Cells[6].Value?.ToString() ?? "";
                string academicYear = selectedRow.Cells[7].Value?.ToString() ?? "";
                string semester = selectedRow.Cells[8].Value?.ToString() ?? "";
                string yearLevel = selectedRow.Cells[9].Value?.ToString() ?? "";

                int courseId = GetCourseId(courseCode);
                int totalUnits = 0;

                if (courseId > 0)
                {
                    totalUnits = LoadSubjectsForEnrollment(courseId, yearLevel, semester);
                }

                UpdateStudentInfoPanel(
                    studentNo,
                    $"{firstName} {middleName} {lastName}",
                    courseCode,
                    academicYear,
                    semester,
                    yearLevel,
                    totalUnits
                );
            }
        }


        private void DataGridEnrollment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= DataGridEnrollment.Rows.Count ||
                e.ColumnIndex < 0 || e.ColumnIndex >= DataGridEnrollment.Columns.Count)
            {
                return;
            }

            DataGridViewRow selectedRow = null;
            try
            {
                selectedRow = DataGridEnrollment.Rows[e.RowIndex];
            }
            catch (ArgumentOutOfRangeException)
            {
                LoadEnrollmentData();
                return;
            }

            string enrollmentId = selectedRow.Cells[0].Value?.ToString() ?? "";
            string firstName = selectedRow.Cells[4].Value?.ToString() ?? "";
            string lastName = selectedRow.Cells[3].Value?.ToString() ?? "";
            string studentName = $"{lastName}, {firstName}";

            if (DataGridEnrollment.Columns[e.ColumnIndex].Name == "ColOpen")
            {

                if (selectedRow.Cells["ColOpen"].ReadOnly) return;

                using (FormNewAcademiccs editForm = new FormNewAcademiccs())
                {
                    editForm.EnrollmentId = enrollmentId;
                    editForm.StartPosition = FormStartPosition.CenterParent;

                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadEnrollmentData();
                    }
                }
            }
            else if (DataGridEnrollment.Columns[e.ColumnIndex].Name == "ColClose")
            {

                if (selectedRow.Cells["ColClose"].ReadOnly) return;

                DialogResult confirmResult = MessageBox.Show(
                    $"Are you sure you want to drop {studentName}'s enrollment?",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        if (DeleteEnrollment(enrollmentId))
                        {
                            if (e.RowIndex < DataGridEnrollment.Rows.Count)
                            {
                                DataGridEnrollment.Rows.RemoveAt(e.RowIndex);
                            }
                            else
                            {
                                LoadEnrollmentData();
                            }

                            MessageBox.Show($"Enrollment for {studentName} dropped successfully.",
                                "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to drop enrollment.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error dropping enrollment: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private int GetCourseId(string courseCode)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT course_id FROM courses WHERE course_code = @CourseCode";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CourseCode", courseCode);
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting course ID: " + ex.Message);
                return -1;
            }
        }

        private void UpdateStudentInfoPanel(string studentNo, string fullName, string courseCode, string academicYear, string semester, string yearLevel, int totalUnits)
        {
            LblStudentNo.Text = studentNo;
            LblName.Text = fullName;
            LblCourse.Text = courseCode;
            LblAcademicYear.Text = academicYear;
            LblSemester.Text = semester;
            LblYearLevel.Text = yearLevel;
            LblTotalUnit.Text = totalUnits.ToString();
        }


        private bool DeleteEnrollment(string enrollmentId)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "DELETE FROM student_enrollments WHERE enrollment_id = @EnrollmentId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {

                        cmd.Parameters.AddWithValue("@EnrollmentId", enrollmentId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error deleting enrollment: " + ex.Message);
            }
        }

        private void FormEnrollment_Load_1(object sender, EventArgs e)
        {
            LoadEnrollmentData();

            ///////////////////////////////////
            DataGridEnrollment.AllowUserToResizeColumns = false;
            DataGridEnrollment.AllowUserToResizeRows = false;
            DataGridEnrollment.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn column in DataGridEnrollment.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            int totalCols = DataGridEnrollment.Columns.Count;
            DataGridEnrollment.Columns[totalCols - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridEnrollment.Columns[totalCols - 1].Width = 40;
            DataGridEnrollment.Columns[totalCols - 2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridEnrollment.Columns[totalCols - 2].Width = 40;
            DataGridEnrollment.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridEnrollment.Columns[0].Width = 50;
            DataGridEnrollment.Columns[1].Width = 50;
            DataGridEnrollment.RowTemplate.Height = 35;
            DataGridEnrollment.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrollment.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //DataGridEnrollment.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrollment.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrollment.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrollment.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrollment.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrollment.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            ///////////////////////////////////////////
            DataGridPayment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridPayment.AllowUserToResizeColumns = false;
            DataGridPayment.AllowUserToResizeRows = false;
            DataGridPayment.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn column in DataGridPayment.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            int totalCol = DataGridPayment.Columns.Count;
            DataGridPayment.Columns[totalCol - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPayment.Columns[totalCol - 1].Width = 40;
            DataGridPayment.Columns[totalCol - 2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPayment.Columns[totalCol - 2].Width = 40;
            DataGridPayment.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPayment.Columns[0].Width = 50;
            DataGridPayment.Columns[1].Width = 50;
            DataGridPayment.RowTemplate.Height = 35;
            ///////////////////////////////////////////////
            DataGridSubjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridSubjects.AllowUserToResizeColumns = false;
            DataGridSubjects.AllowUserToResizeRows = false;
            DataGridSubjects.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn column in DataGridSubjects.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            DataGridSubjects.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridSubjects.Columns[0].Width = 50;
            DataGridSubjects.Columns[1].Width = 50;
            //DataGridSubjects.Columns[2].Width = 500;
            //DataGridSubjects.Columns[3].Width = 800;
            //DataGridSubjects.Columns[4].Width = 50;
            //DataGridSubjects.Columns[5].Width = 80;
            //DataGridSubjects.Columns[6].Width = 80;
            //DataGridSubjects.Columns[7].Width = 80;

            DataGridSubjects.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridSubjects.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridSubjects.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridSubjects.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridSubjects.RowTemplate.Height = 35;

            CustomizeDataGrid();
            StyleTwoTabControl();
            StyleDataGridPayment();
            StyleDataGridSubjects();
        }

        private void CustomizeDataGrid()
        {
            DataGridEnrollment.BorderStyle = BorderStyle.None;

            DataGridEnrollment.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridEnrollment.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridEnrollment.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridEnrollment.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridEnrollment.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridEnrollment.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridEnrollment.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridEnrollment.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridEnrollment.EnableHeadersVisualStyles = false;

            DataGridEnrollment.GridColor = Color.BurlyWood;

            DataGridEnrollment.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridEnrollment.RowTemplate.Height = 35;

            DataGridEnrollment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn column in DataGridEnrollment.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }

            /////////////////////////
            DataGridPayment.BorderStyle = BorderStyle.None;

            DataGridPayment.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridPayment.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridPayment.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridPayment.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridPayment.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridPayment.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridPayment.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridPayment.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridPayment.EnableHeadersVisualStyles = false;

            DataGridPayment.GridColor = Color.BurlyWood;

            DataGridPayment.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridPayment.RowTemplate.Height = 35;

            DataGridPayment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn column in DataGridPayment.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }
        }


        private void StyleTwoTabControl()
        {
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.ItemSize = new Size(160, 36);

            Color darkBrown = Color.FromArgb(94, 55, 30);
            Color mediumBrown = Color.FromArgb(139, 69, 19);
            Color lightBrown = Color.FromArgb(210, 180, 140);
            Color goldYellow = Color.FromArgb(218, 165, 32);
            Color cream = Color.FromArgb(253, 245, 230);

            tabControl1.DrawItem += (sender, e) =>
            {
                Graphics g = e.Graphics;
                TabPage currentTab = tabControl1.TabPages[e.Index];
                Rectangle tabRect = tabControl1.GetTabRect(e.Index);
                bool isSelected = tabControl1.SelectedIndex == e.Index;


                if (isSelected)
                {
                    tabRect.Inflate(0, 2);
                    tabRect.Y -= 2;
                }

                if (isSelected)
                {
                    using (var brush = new LinearGradientBrush(
                        tabRect,
                        Color.FromArgb(255, 230, 170),
                        goldYellow,
                        LinearGradientMode.Vertical))
                    {
                        g.FillRectangle(brush, tabRect);
                    }
                }
                else
                {
                    using (var brush = new SolidBrush(mediumBrown))
                    {
                        g.FillRectangle(brush, tabRect);
                    }
                }


                using (var pen = new Pen(isSelected ? goldYellow : darkBrown, isSelected ? 2f : 1f))
                {
                    g.DrawRectangle(pen, tabRect);
                }

                TextRenderer.DrawText(
                    g,
                    currentTab.Text,
                    new Font(tabControl1.Font.FontFamily, 9f,
                            isSelected ? FontStyle.Bold : FontStyle.Regular),
                    tabRect,
                    isSelected ? darkBrown : Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                if (isSelected)
                {
                    using (var pen = new Pen(goldYellow, 2))
                    {
                        int underlineY = tabRect.Bottom - 3;
                        g.DrawLine(pen, tabRect.Left + 10, underlineY,
                                    tabRect.Right - 10, underlineY);
                    }
                }
            };

            foreach (TabPage tab in tabControl1.TabPages)
            {
                tab.BackColor = cream;
                tab.ForeColor = darkBrown;
                tab.Padding = new Padding(8);
                tab.BorderStyle = BorderStyle.FixedSingle;
            }

            tabControl1.BackColor = lightBrown;


            this.BackColor = Color.FromArgb(250, 240, 220);
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {

        }

        private void DataGridPayment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void StyleDataGridPayment()
        {
            DataGridPayment.BorderStyle = BorderStyle.None;

            DataGridPayment.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridPayment.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridPayment.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridPayment.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridPayment.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridPayment.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridPayment.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridPayment.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridPayment.EnableHeadersVisualStyles = false;

            DataGridPayment.GridColor = Color.BurlyWood;

            DataGridPayment.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridPayment.RowTemplate.Height = 35;

            DataGridPayment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn column in DataGridPayment.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }

        }

        private void StyleDataGridSubjects()
        {
            DataGridSubjects.BorderStyle = BorderStyle.None;

            DataGridSubjects.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridSubjects.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridSubjects.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridSubjects.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridSubjects.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridSubjects.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridSubjects.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridSubjects.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridSubjects.EnableHeadersVisualStyles = false;

            DataGridSubjects.GridColor = Color.BurlyWood;

            DataGridSubjects.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridSubjects.RowTemplate.Height = 35;

            DataGridSubjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn column in DataGridSubjects.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }

        }

        private void DataGridPayment_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var status_payment = DataGridPayment.Rows[e.RowIndex].Cells[12].Value?.ToString()?.ToLower();

                var clickedColumn = DataGridPayment.Columns[e.ColumnIndex].Name;

                if (status_payment == "enrolled" && (clickedColumn == "ColPay" || clickedColumn == "ColDelete"))
                {

                    return;
                }
            }

            // Check if the clicked cell is in the column containing the blue button
            if (e.ColumnIndex == DataGridPayment.Columns["ColPay"].Index && e.RowIndex >= 0)
            {
                // Open the FormPayment
                int enrollmentId = Convert.ToInt32(DataGridPayment.Rows[e.RowIndex].Cells["payment_id"].Value);
                FormPayment formPayment = new FormPayment(enrollmentId);
                formPayment.ShowDialog();
            }// Check if the clicked cell is in the delete button column
            else if (e.ColumnIndex == DataGridPayment.Columns["ColDelete"].Index && e.RowIndex >= 0)
            {
                // Confirm deletion with the user
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this row?",
                    "Delete Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    // Remove the row at the specified index
                    DataGridPayment.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //new FormPayment().Show();
        }

        private int LoadSubjectsForEnrollment(int courseId, string yearLevel, string semester)
        {
            int totalUnits = 0;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            cs.course_subject_id,
                            s.subject_code,
                            s.subject_name,
                            s.units,
                            c.course_code,
                            cs.semester AS semester1,
                            cs.year_level AS year_level1
                        FROM course_subjects cs
                        INNER JOIN subjects s ON cs.subject_id = s.subject_id
                        INNER JOIN courses c ON cs.course_id = c.course_id
                        WHERE cs.course_id = @CourseId 
                        AND cs.year_level = @YearLevel
                        AND cs.semester = @Semester";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CourseId", courseId);
                        cmd.Parameters.AddWithValue("@YearLevel", yearLevel);
                        cmd.Parameters.AddWithValue("@Semester", semester);

                        DataTable dt = new DataTable();
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }

                        DataGridSubjects.Columns.Clear();
                        DataGridSubjects.Rows.Clear();

                        DataGridSubjects.Columns.Add("course_subject_id", "ID");
                        DataGridSubjects.Columns.Add("No.", "No.");
                        DataGridSubjects.Columns.Add("subject_code", "Subject Code");
                        DataGridSubjects.Columns.Add("subject_name", "Subject Name");
                        DataGridSubjects.Columns.Add("units", "Units");
                        DataGridSubjects.Columns.Add("course_code", "Course Code");
                        DataGridSubjects.Columns.Add("semester1", "Semester");
                        DataGridSubjects.Columns.Add("year_level1", "Year Level");

                        DataGridSubjects.Columns["No."].ReadOnly = true;
                        DataGridSubjects.Columns["No."].Width = 50;

                        DataGridSubjects.Columns["course_subject_id"].Visible = false;

                        int rowNumber = 1;
                        foreach (DataRow row in dt.Rows)
                        {
                            DataGridSubjects.Rows.Add(
                                row["course_subject_id"],  
                                rowNumber.ToString(),      
                                row["subject_code"],
                                row["subject_name"],
                                row["units"],
                                row["course_code"],
                                row["semester1"],
                                row["year_level1"]
                            );

                            rowNumber++;

                            int units;
                            if (int.TryParse(row["units"].ToString(), out units))
                            {
                                totalUnits += units;
                            }
                        }

                        UpdatePaymentCalculation(totalUnits);
                        StyleDataGridSubjects();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading subjects: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return totalUnits;
        }

        private void DataGridSubjects_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private decimal CalculateTuitionFee(int totalUnits)
        {
            const decimal PER_UNIT_COST = 150.00m;
            return totalUnits * PER_UNIT_COST;
        }

        private decimal CalculateMiscellaneousFee()
        {
            return 800.00m;
        }

        private void UpdatePaymentCalculation(int totalUnits)
        {
            decimal tuitionFee = CalculateTuitionFee(totalUnits);
            decimal miscFee = CalculateMiscellaneousFee();
            decimal totalAmountDue = tuitionFee + miscFee;

            // Update your UI elements with these values
            // For example:
            LblTuitionFee1.Text = tuitionFee.ToString("0.00");
            LblMiscFee1.Text = miscFee.ToString("0.00");
            LblTotalAmount.Text = totalAmountDue.ToString("0.00");
        }

        private void LoadStudentPayments()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                    SELECT 
                        p.payment_id,
                        p.payment_date,
                        p.total_units,
                        p.total_amount_due,
                        p.amount_paid,
                        p.is_unifast,
                        p.payment_method,
                        p.receipt_no,
                        se.academic_year,
                        se.semester,
                        p.payment_status,
                        (SELECT amount FROM payment_breakdowns WHERE payment_id = p.payment_id AND fee_type = 'Tuition') AS tuition,
                        (SELECT amount FROM payment_breakdowns WHERE payment_id = p.payment_id AND fee_type = 'Miscellaneous') AS miscellaneous
                    FROM payments p
                    INNER JOIN student_enrollments se ON p.enrollment_id = se.enrollment_id
                    WHERE se.student_id = @StudentId
                    ORDER BY p.payment_date DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", SessionManager.StudentId);

                        DataGridPayment.Rows.Clear();
                        int rowNumber = 1;
                        
                        DataGridPayment.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                DataGridPayment.Rows.Add(
                                    reader["payment_id"]?.ToString() ?? "N/A",
                                    rowNumber.ToString(),
                                    reader["payment_date"] != DBNull.Value ? Convert.ToDateTime(reader["payment_date"]).ToString("yyyy-MM-dd") : "N/A",
                                    reader["total_units"]?.ToString() ?? "0",
                                    reader["total_amount_due"] != DBNull.Value ? Convert.ToDecimal(reader["total_amount_due"]).ToString("0.00") : "0.00",
                                    reader["amount_paid"] != DBNull.Value ? Convert.ToDecimal(reader["amount_paid"]).ToString("0.00") : "0.00",
                                    reader["is_unifast"] != DBNull.Value ? (Convert.ToBoolean(reader["is_unifast"]) ? "Yes" : "No") : "No",
                                    reader["payment_method"]?.ToString() ?? "N/A",
                                    reader["receipt_no"]?.ToString() ?? "N/A",
                                    reader["academic_year"]?.ToString() ?? "N/A",
                                    reader["semester"]?.ToString() ?? "N/A",
                                    reader["tuition"] != DBNull.Value ? Convert.ToDecimal(reader["tuition"]).ToString("0.00") : "0.00",
                                    reader["miscellaneous"] != DBNull.Value ? Convert.ToDecimal(reader["miscellaneous"]).ToString("0.00") : "0.00",
                                    reader["payment_status"]?.ToString() ?? "N/A"
                                );
                                rowNumber++; 
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading payment data: {ex.Message}", "Database Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateRowNumbersEnrollment()
        {
            if (DataGridEnrollment.Rows.Count == 0) return;

            int noColumnIndex = DataGridEnrollment.Columns[1].Index;

            for (int i = 0; i < DataGridEnrollment.Rows.Count; i++)
            {
                if (DataGridEnrollment.Rows[i].IsNewRow) continue;
                DataGridEnrollment.Rows[i].Cells[noColumnIndex].Value = (i + 1).ToString();
            }
        }

        private void UpdateRowNumbersPayment()
        {
            if (DataGridPayment.Rows.Count == 0) return;

            int noColumnIndex = DataGridPayment.Columns[1].Index;

            for (int i = 0; i < DataGridPayment.Rows.Count; i++)
            {
                if (DataGridPayment.Rows[i].IsNewRow) continue;
                DataGridPayment.Rows[i].Cells[noColumnIndex].Value = (i + 1).ToString();
            }
        }

        private void UpdateRowNumbersSubject()
        {
            if (DataGridSubjects.Rows.Count == 0) return;

            int noColumnIndex = DataGridSubjects.Columns[1].Index;

            for (int i = 0; i < DataGridSubjects.Rows.Count; i++)
            {
                if (DataGridSubjects.Rows[i].IsNewRow) continue;
                DataGridSubjects.Rows[i].Cells[noColumnIndex].Value = (i + 1).ToString();
            }
        }

        private void DataGridNewEnrollment_Sorted(object sender, EventArgs e)
        {
            UpdateRowNumbersEnrollment();
        }

        private void DataGridPayment_Sorted(object sender, EventArgs e)
        {
            UpdateRowNumbersPayment();
        }

        private void DataGridSubjects_Sorted(object sender, EventArgs e)
        {
            UpdateRowNumbersSubject();
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}