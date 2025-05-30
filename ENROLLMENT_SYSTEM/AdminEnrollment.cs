using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;

namespace Enrollment_System
{
    public partial class AdminEnrollment : Form
    {
        private readonly string connectionString = DatabaseConfig.ConnectionString;

        private readonly string _sendGridApiKey = ConfigurationManager.AppSettings["SendGridApiKey"];
        private string currentProgramFilter = "All";
        private Button[] programButtons;
        private int _maxStudentsPerSection = 5;

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            LoadPaymentData();
            LoadStudentData();
            RefreshDataGridView(DataGridNewEnrollment);
        }

        public AdminEnrollment()
        {
            InitializeComponent();
            InitializeProgramButtons();
            InitializeDataGridView();
            LoadStudentData();
            InitializeFilterControls();

            DataGridNewEnrollment.DataBindingComplete += (s, e) => UpdateRowNumbersNewEnrollment();
            DataGridPaidEnrollment.DataBindingComplete += (s, e) => UpdateRowNumbesPaidEnrollment();

            DataGridNewEnrollment.Sorted += DataGridNewEnrollment_Sorted;
            DataGridPayment.Sorted += DataGridPayment_Sorted;
            DataGridPaidEnrollment.Sorted += DataGridPaidEnrollment_Sorted;

            ProgramButton_Click(BtnAll, EventArgs.Empty);
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
        }

        public static class EnrollmentStatus
        {
            public const string Pending = "Pending";
            public const string PaymentPending = "Payment Pending";
            public const string Enrolled = "Enrolled";
        }

        private void AdminEnrollment_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn col in DataGridNewEnrollment.Columns)
            {
                Console.WriteLine("Column Name: " + col.Name);
            }

            DataGridPayment.CellClick += DataGridPayment_CellClick;
            DataGridNewEnrollment.CellClick += DataGridNewEnrollment_CellClick;

            StyleTwoTabControl();
            InitializeDataGridView();
            InitializeFilterControls();
            SetupDataGridColumns();
            ConfigureDataGridLayouts();

            CustomizeDataGridNewEnrollment();
            CustomizeDataGridPaidEnrollment();
            CustomizeDataGridPayment();
            StyleTwoTabControl();

            if (SessionManager.HasRole("cashier"))
            {
                tabControl1.TabPages.Remove(tabStudentEnrollment);
            }
        }

        private void SetupDataGridColumns()
        {
            DataGridNewEnrollment.AutoGenerateColumns = false;
            enrollment_id.DataPropertyName = "enrollment_id";
            student_no.DataPropertyName = "student_no";
            last_name.DataPropertyName = "last_name";
            first_name.DataPropertyName = "first_name";
            middle_name.DataPropertyName = "middle_name";
            courseCode.DataPropertyName = "Program";
            academic_year.DataPropertyName = "academic_year";
            semester.DataPropertyName = "semester";
            year_level.DataPropertyName = "year_level";
            status.DataPropertyName = "status";
            grade_pdf_path.DataPropertyName = "grade_pdf_path";

            DataGridPayment.AutoGenerateColumns = false;
            payment_id_payment.DataPropertyName = "payment_id";
            student_no_payment.DataPropertyName = "student_no";
            last_name_payment.DataPropertyName = "last_name";
            first_name_payment.DataPropertyName = "first_name";
            middle_name_payment.DataPropertyName = "middle_name";
            courseCode_payment.DataPropertyName = "Program";
            academic_year_payment.DataPropertyName = "academic_year";
            semester_payment.DataPropertyName = "semester";
            year_level_payment.DataPropertyName = "year_level";
            status_payment.DataPropertyName = "status";

            DataGridPaidEnrollment.AutoGenerateColumns = false;
            payment_id_pe.DataPropertyName = "payment_id";
            student_no_pe.DataPropertyName = "student_no";
            last_name_pe.DataPropertyName = "last_name";
            first_name_pe.DataPropertyName = "first_name";
            middle_name_pe.DataPropertyName = "middle_name";
            courseCode_pe.DataPropertyName = "Program";
            academic_year_pe.DataPropertyName = "academic_year";
            semester_pe.DataPropertyName = "semester";
            year_level_pe.DataPropertyName = "year_level";
            status_pe.DataPropertyName = "payment_status";
        }

        private void ConfigureDataGridLayouts()
        {
            ConfigureDataGridLayout(DataGridNewEnrollment);
            ConfigureDataGridLayout(DataGridPaidEnrollment);
            ConfigureDataGridLayout(DataGridPayment);
        }

        private void ConfigureDataGridLayout(DataGridView dataGrid)
        {
            dataGrid.AllowUserToResizeColumns = false;
            dataGrid.AllowUserToResizeRows = false;
            dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            foreach (DataGridViewColumn column in dataGrid.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            int totalCols = dataGrid.Columns.Count;
            dataGrid.Columns[totalCols - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGrid.Columns[totalCols - 1].Width = 40;
            dataGrid.Columns[totalCols - 2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGrid.Columns[totalCols - 2].Width = 40;
            dataGrid.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGrid.Columns[0].Width = 50;
            dataGrid.RowTemplate.Height = 35;

            string[] centerAlignedColumns = { "student_no", "Program", "academic_year", "semester", "year_level", "status" };
            foreach (string colName in centerAlignedColumns)
            {
                if (dataGrid.Columns.Contains(colName))
                {
                    dataGrid.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }

            foreach (DataGridViewColumn col in dataGrid.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
        }

        private void InitializeDataGridView()
        {
            InitializeGridSettings(DataGridNewEnrollment, "ColOpen1", "ColClose1");
            InitializeGridSettings(DataGridPaidEnrollment, "ColOpen2", "ColClose2");
            InitializeGridSettings(DataGridPayment, "ColOpen3", "ColClose3");
        }

        private void InitializeGridSettings(DataGridView dataGrid, string openColName, string closeColName)
        {
            foreach (DataGridViewColumn col in dataGrid.Columns)
            {
                col.Frozen = false;
            }

            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid.Columns[openColName].Width = 50;
            dataGrid.Columns[closeColName].Width = 50;
            dataGrid.Columns[openColName].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGrid.Columns[closeColName].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGrid.RowTemplate.Height = 40;
            dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            dataGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewImageColumn openCol = dataGrid.Columns[openColName] as DataGridViewImageColumn;
            if (openCol != null)
            {
                openCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            }

            DataGridViewImageColumn closeCol = dataGrid.Columns[closeColName] as DataGridViewImageColumn;
            if (closeCol != null)
            {
                closeCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            }

            foreach (DataGridViewColumn col in dataGrid.Columns)
            {
                col.Resizable = DataGridViewTriState.True;
            }
        }

        private void InitializeProgramButtons()
        {
            programButtons = new Button[]
            {
                BtnBSCS, BtnBSIT, BtnBSTM, BtnBSHM, BtnBSOAD, BtnBECED, BtnBTLED, BtnAll
            };

            foreach (var btn in programButtons)
            {
                btn.Click += ProgramButton_Click;
            }
        }

        private void InitializeFilterControls()
        {
            CmbYrLvl.SelectedIndex = 0;
            CmbSem.SelectedIndex = 0;
            CmbSchoolYear.SelectedIndex = 0;

            CmbYrLvl.SelectedIndexChanged += ApplyFilters;
            CmbSem.SelectedIndexChanged += ApplyFilters;
            CmbSchoolYear.SelectedIndexChanged += ApplyFilters;
        }

        private void LoadStudentData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                    SELECT 
                        se.enrollment_id,
                        s.student_no,
                        s.last_name,
                        s.first_name,
                        s.middle_name,
                        c.course_code AS Program,
                        se.academic_year,
                        se.semester,
                        se.year_level,
                        se.status,
                        se.grade_pdf_path,
                        se.grade_pdf
                    FROM student_enrollments se
                    INNER JOIN students s ON se.student_id = s.student_id
                    INNER JOIN courses c ON se.course_id = c.course_id
                    WHERE se.status = 'Pending'";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataGridNewEnrollment.AutoGenerateColumns = false;
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataGridViewBindingCompleteEventHandler bindingCompleteHandler = null;
                            bindingCompleteHandler = (sender, e) =>
                            {
                                UpdateRowNumbersNewEnrollment();
                                DataGridNewEnrollment.DataBindingComplete -= bindingCompleteHandler;
                            };

                            DataGridNewEnrollment.DataBindingComplete += bindingCompleteHandler;
                            DataGridNewEnrollment.DataSource = dt;

                            if (DataGridNewEnrollment.Columns.Contains("grade_pdf_path"))
                            {
                                DataGridNewEnrollment.Columns["grade_pdf_path"].Visible = false;
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

        private void UpdateRowNumbersNewEnrollment()
        {
            UpdateRowNumbers(DataGridNewEnrollment, 1);
        }

        private void UpdateRowNumbersPyment()
        {
            UpdateRowNumbers(DataGridPayment, 1);
        }

        private void UpdateRowNumbesPaidEnrollment()
        {
            UpdateRowNumbers(DataGridPaidEnrollment, 0);
        }

        private void UpdateRowNumbers(DataGridView dataGrid, int columnIndex)
        {
            if (dataGrid.Rows.Count == 0) return;

            for (int i = 0; i < dataGrid.Rows.Count; i++)
            {
                if (dataGrid.Rows[i].IsNewRow) continue;
                dataGrid.Rows[i].Cells[columnIndex].Value = (i + 1).ToString();
            }
        }

        private void DataGridNewEnrollment_Sorted(object sender, EventArgs e)
        {
            UpdateRowNumbersNewEnrollment();
        }

        private void DataGridPayment_Sorted(object sender, EventArgs e)
        {
            UpdateRowNumbersPyment();
        }

        private void DataGridPaidEnrollment_Sorted(object sender, EventArgs e)
        {
            UpdateRowNumbesPaidEnrollment();
        }

        private void ApplyFilters(object sender, EventArgs e)
        {
            string yearLevelFilter = CmbYrLvl.SelectedItem.ToString();
            string semesterFilter = CmbSem.SelectedItem.ToString();
            string schoolyearFilter = CmbSchoolYear.SelectedItem.ToString();

            string filterExpression = BuildFilterExpression(yearLevelFilter, semesterFilter, schoolyearFilter);

            ApplyFilterToDataGrid(DataGridNewEnrollment, filterExpression);
            ApplyFilterToDataGrid(DataGridPayment, filterExpression);
            ApplyFilterToDataGrid(DataGridPaidEnrollment, filterExpression);
        }

        private string BuildFilterExpression(string yearLevelFilter, string semesterFilter, string schoolyearFilter)
        {
            string filterExpression = "";

            if (currentProgramFilter != "All")
                filterExpression += $"[program] = '{currentProgramFilter}'";

            if (yearLevelFilter != "All")
            {
                if (!string.IsNullOrEmpty(filterExpression))
                    filterExpression += " AND ";
                filterExpression += $"[year_level] = '{yearLevelFilter}'";
            }

            if (semesterFilter != "All")
            {
                if (!string.IsNullOrEmpty(filterExpression))
                    filterExpression += " AND ";
                filterExpression += $"[semester] = '{semesterFilter}'";
            }

            if (schoolyearFilter != "All")
            {
                if (!string.IsNullOrEmpty(filterExpression))
                    filterExpression += " AND ";
                filterExpression += $"[academic_year] = '{schoolyearFilter}'";
            }

            return filterExpression;
        }

        private void ApplyFilterToDataGrid(DataGridView dataGrid, string filterExpression)
        {
            if (dataGrid.DataSource is DataTable)
            {
                DataTable dt = (DataTable)dataGrid.DataSource;
                dt.DefaultView.RowFilter = filterExpression;
                if (dataGrid == DataGridNewEnrollment) UpdateRowNumbersNewEnrollment();
                else if (dataGrid == DataGridPayment) UpdateRowNumbersPyment();
                else if (dataGrid == DataGridPaidEnrollment) UpdateRowNumbesPaidEnrollment();
            }
        }

        private void ProgramButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null) return;

            currentProgramFilter = clickedButton == BtnAll ? "All" : clickedButton.Text.Replace("Btn", "");

            if (clickedButton == BtnAll)
            {
                CmbYrLvl.SelectedIndex = 0;
                CmbSem.SelectedIndex = 0;
                CmbSchoolYear.SelectedIndex = 0;
            }

            ApplyFilters(null, null);
        }

        private void StyleDataGrid(DataGridView dataGrid)
        {
            dataGrid.BorderStyle = BorderStyle.None;
            dataGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);
            dataGrid.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            dataGrid.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);
            dataGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            dataGrid.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            dataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dataGrid.EnableHeadersVisualStyles = false;
            dataGrid.GridColor = Color.BurlyWood;
            dataGrid.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dataGrid.RowTemplate.Height = 35;
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn column in dataGrid.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }
        }

        private void CustomizeDataGridNewEnrollment()
        {
            StyleDataGrid(DataGridNewEnrollment);
        }

        private void CustomizeDataGridPaidEnrollment()
        {
            StyleDataGrid(DataGridPaidEnrollment);
        }

        private void CustomizeDataGridPayment()
        {
            StyleDataGrid(DataGridPayment);
        }

        private void StyleTwoTabControl()
        {
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.ItemSize = new Size(160, 36);

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
                        Color.FromArgb(218, 165, 32),
                        LinearGradientMode.Vertical))
                    {
                        g.FillRectangle(brush, tabRect);
                    }
                }
                else
                {
                    using (var brush = new SolidBrush(Color.FromArgb(139, 69, 19)))
                    {
                        g.FillRectangle(brush, tabRect);
                    }
                }

                using (var pen = new Pen(isSelected ? Color.FromArgb(218, 165, 32) : Color.FromArgb(94, 55, 30),
                    isSelected ? 2f : 1f))
                {
                    g.DrawRectangle(pen, tabRect);
                }

                TextRenderer.DrawText(
                    g,
                    currentTab.Text,
                    new Font(tabControl1.Font.FontFamily, 9f,
                            isSelected ? FontStyle.Bold : FontStyle.Regular),
                    tabRect,
                    isSelected ? Color.FromArgb(94, 55, 30) : Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                if (isSelected)
                {
                    using (var pen = new Pen(Color.FromArgb(218, 165, 32), 2))
                    {
                        int underlineY = tabRect.Bottom - 3;
                        g.DrawLine(pen, tabRect.Left + 10, underlineY,
                                    tabRect.Right - 10, underlineY);
                    }
                }
            };
        }

        private void DeletePayment(DataGridViewRow row)
        {
            try
            {
                int paymentId = Convert.ToInt32(row.Cells["payment_id_payment"].Value);
                string studentName = $"{row.Cells["last_name_payment"].Value}, {row.Cells["first_name_payment"].Value}";

                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete this payment record?\n\nStudent: {studentName}",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        // First delete payment breakdowns
                        string deleteBreakdownsQuery = "DELETE FROM payment_breakdowns WHERE payment_id = @paymentId";
                        using (var cmd = new MySqlCommand(deleteBreakdownsQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@paymentId", paymentId);
                            cmd.ExecuteNonQuery();
                        }

                        // Then delete the payment
                        string deletePaymentQuery = "DELETE FROM payments WHERE payment_id = @paymentId";
                        using (var cmd = new MySqlCommand(deletePaymentQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@paymentId", paymentId);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Update the enrollment status back to "Payment Pending"
                                string updateEnrollmentQuery = @"
                            UPDATE student_enrollments se
                            JOIN payments p ON se.enrollment_id = p.enrollment_id
                            SET se.status = 'Payment Pending'
                            WHERE p.payment_id = @paymentId";

                                using (var updateCmd = new MySqlCommand(updateEnrollmentQuery, conn))
                                {
                                    updateCmd.Parameters.AddWithValue("@paymentId", paymentId);
                                    updateCmd.ExecuteNonQuery();
                                }

                                // Remove from grid and refresh
                                DataGridPayment.Rows.Remove(row);
                                MessageBox.Show("Payment record deleted successfully!",
                                              "Success",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Information);

                                // Refresh all relevant data
                                LoadPaymentData();
                                LoadStudentData();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting payment: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void DeleteNewEnrollment(DataGridViewRow row)
        {
            int enrollmentId = Convert.ToInt32(row.Cells["enrollment_id"].Value);
            string studentName = $"{row.Cells["last_name"].Value}, {row.Cells["first_name"].Value}";

            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete this enrollment?\n\nStudent: {studentName}",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM student_enrollments WHERE enrollment_id = @enrollmentId";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@enrollmentId", enrollmentId);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                LoadStudentData();
                                RefreshDataGridView(DataGridNewEnrollment);
                                MessageBox.Show("Enrollment deleted successfully!", "Success",
                                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadStudentData();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting enrollment: {ex.Message}",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ViewPdfFromDatabase(int enrollmentId)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT grade_pdf, grade_pdf_path 
                            FROM student_enrollments 
                            WHERE enrollment_id = @enrollmentId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@enrollmentId", enrollmentId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["grade_pdf"] != DBNull.Value)
                                {
                                    byte[] fileData = (byte[])reader["grade_pdf"];
                                    string tempPath = Path.Combine(Path.GetTempPath(), $"grade_{enrollmentId}.pdf");
                                    File.WriteAllBytes(tempPath, fileData);
                                    System.Diagnostics.Process.Start(tempPath);
                                }
                                else if (reader["grade_pdf_path"] != DBNull.Value)
                                {
                                    string filePath = reader["grade_pdf_path"].ToString();
                                    if (File.Exists(filePath))
                                    {
                                        System.Diagnostics.Process.Start(filePath);
                                    }
                                    else
                                    {
                                        MessageBox.Show("PDF file not found at specified path.");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No PDF available for this enrollment.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening PDF: {ex.Message}");
            }
        }

        private void FetchStudentDetails(string studentNo)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            s.student_no,
                            s.last_name,
                            s.first_name,
                            s.middle_name,
                            c.course_name,
                            se.year_level,
                            se.semester,
                            s.profile_picture
                        FROM student_enrollments se
                        INNER JOIN students s ON se.student_id = s.student_id
                        INNER JOIN courses c ON se.course_id = c.course_id
                        WHERE s.student_no = @studentNo";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@studentNo", studentNo);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                TxtLastName.Text = reader["last_name"].ToString();
                                TxtFirstName.Text = reader["first_name"].ToString();
                                TxtMiddleName.Text = reader["middle_name"].ToString();
                                TxtStudentNo.Text = reader["student_no"].ToString();
                                TxtCourseName.Text = reader["course_name"].ToString();
                                TxtYrLevel.Text = reader["year_level"].ToString();
                                TxtSemester.Text = reader["semester"].ToString();

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
                MessageBox.Show("Error fetching student details: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnConfirm_Click(object sender, EventArgs e)
{
    try
    {
        DataGridView currentGrid;
        string enrollmentIdColumn, studentNoColumn, lastNameColumn, firstNameColumn,
               middleNameColumn, courseCodeColumn, academicYearColumn,
               semesterColumn, yearLevelColumn, statusColumn;
        string currentStatus;
        string successMessage;
        string confirmationMessage;
        string newStatus;
        bool sendEmail = false;
        bool isPaymentConfirmation = false;
        bool isUniFastPayment = false;
        int paymentId = 0;

        if (tabControl1.SelectedTab == tabPayment)
        {
            currentGrid = DataGridPayment;
            isPaymentConfirmation = true;

            // Get the payment ID from selected row
            paymentId = Convert.ToInt32(currentGrid.SelectedRows[0].Cells["payment_id_payment"].Value);

            // First validate the payment by opening AdminCashier
            bool isValid = false;
            using (var cashierForm = new AdminCashier(paymentId))
            {
                if (cashierForm.ShowDialog() == DialogResult.OK)
                {
                    isValid = cashierForm.IsPaymentValid;
                    isUniFastPayment = cashierForm.IsUniFastPayment;
                }
                else
                {
                    return; // User cancelled the operation
                }
            }

            if (!isValid)
            {
                MessageBox.Show("Cannot confirm payment. Please complete all payment details first.",
                              "Incomplete Payment",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                
                // Reopen the form for correction
                using (var cashierForm = new AdminCashier(paymentId))
                {
                    cashierForm.ShowDialog();
                }
                return;
            }

            // Rest of the payment confirmation setup
            enrollmentIdColumn = "payment_id_payment";
            studentNoColumn = "student_no_payment";
            lastNameColumn = "last_name_payment";
            firstNameColumn = "first_name_payment";
            middleNameColumn = "middle_name_payment";
            courseCodeColumn = "courseCode_payment";
            academicYearColumn = "academic_year_payment";
            semesterColumn = "semester_payment";
            yearLevelColumn = "year_level_payment";
            statusColumn = "status_payment";

            currentStatus = "Payment Pending";
            newStatus = isUniFastPayment ? "Pending" : "Completed"; // Set status based on UniFAST
            confirmationMessage = $"Are you sure you want to confirm this {(isUniFastPayment ? "UniFAST" : "")} payment?";
            successMessage = $"Payment confirmed as {newStatus.ToLower()}!";
        }
        else if (tabControl1.SelectedTab == tabStudentEnrollment)
        {
            currentGrid = DataGridNewEnrollment;

            enrollmentIdColumn = "enrollment_id";
            studentNoColumn = "student_no";
            lastNameColumn = "last_name";
            firstNameColumn = "first_name";
            middleNameColumn = "middle_name";
            courseCodeColumn = "courseCode";
            academicYearColumn = "academic_year";
            semesterColumn = "semester";
            yearLevelColumn = "year_level";
            statusColumn = "status";

            currentStatus = "Pending";
            newStatus = "Enrolled";
            confirmationMessage = "Are you sure you want to confirm this enrollment?";
            successMessage = "Enrollment confirmed successfully!";
            sendEmail = true;
        }
        else
        {
            MessageBox.Show("Please select either the Enrollment or Payment tab first.",
                          "Invalid Tab",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Warning);
            return;
        }

        if (currentGrid.SelectedRows.Count == 0)
        {
            MessageBox.Show($"Please select a student from the {tabControl1.SelectedTab.Text} tab.",
                          "No Selection",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Warning);
            return;
        }

        DataGridViewRow selectedRow = currentGrid.SelectedRows[0];
        string studentName = $"{selectedRow.Cells[lastNameColumn].Value} {selectedRow.Cells[firstNameColumn].Value}";

        string courseCode = selectedRow.Cells[courseCodeColumn].Value.ToString();
        string yearLevel = selectedRow.Cells[yearLevelColumn].Value.ToString();
        string semester = selectedRow.Cells[semesterColumn].Value.ToString();
        string academicYear = selectedRow.Cells[academicYearColumn].Value.ToString();

        DialogResult dialogResult = MessageBox.Show(
            $"{confirmationMessage}\n\nStudent: {studentName}",
            "Confirm Action",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (dialogResult == DialogResult.Yes)
        {
            await ProcessConfirmation(isPaymentConfirmation, currentGrid, selectedRow,
                enrollmentIdColumn, newStatus, sendEmail, studentName,
                courseCode, yearLevel, semester, academicYear, successMessage);
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"An error occurred: {ex.Message}\n\nPlease try again or contact support.",
                      "Error",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error);
    }
}

        private async Task ProcessConfirmation(bool isPaymentConfirmation, DataGridView currentGrid,
         DataGridViewRow selectedRow, string enrollmentIdColumn, string newStatus,
         bool sendEmail, string studentName, string courseCode, string yearLevel,
         string semester, string academicYear, string successMessage)
        {
            string email = "";
            string fullName = "";
            int changedByUserId = (int)SessionManager.UserId;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        int recordId;
                        string updateQuery;

                        if (isPaymentConfirmation)
                        {
                            // Payment confirmation logic
                            int paymentId = Convert.ToInt32(selectedRow.Cells[enrollmentIdColumn].Value);

                            string getEnrollmentIdQuery = "SELECT enrollment_id FROM payments WHERE payment_id = @paymentId";
                            using (MySqlCommand getCmd = new MySqlCommand(getEnrollmentIdQuery, conn, transaction))
                            {
                                getCmd.Parameters.AddWithValue("@paymentId", paymentId);
                                recordId = Convert.ToInt32(getCmd.ExecuteScalar());
                            }

                            updateQuery = "UPDATE student_enrollments SET status = @newStatus WHERE enrollment_id = @recordId";
                            using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@newStatus", newStatus);
                                cmd.Parameters.AddWithValue("@recordId", recordId);
                                cmd.ExecuteNonQuery();
                            }

                            string updatePaymentQuery = "UPDATE payments SET payment_status = 'Completed' WHERE payment_id = @paymentId";
                            using (MySqlCommand paymentCmd = new MySqlCommand(updatePaymentQuery, conn, transaction))
                            {
                                paymentCmd.Parameters.AddWithValue("@paymentId", paymentId);
                                paymentCmd.ExecuteNonQuery();
                            }

                            sendEmail = true;
                        }
                        else
                        {
                            // Enrollment confirmation logic
                            recordId = Convert.ToInt32(selectedRow.Cells[enrollmentIdColumn].Value);

                            // Update enrollment status
                            updateQuery = "UPDATE student_enrollments SET status = @newStatus WHERE enrollment_id = @recordId";
                            using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@newStatus", newStatus);
                                cmd.Parameters.AddWithValue("@recordId", recordId);
                                cmd.ExecuteNonQuery();
                            }

                            // Get current academic history if exists
                            int? historyId = null;
                            string currentSection = null;
                            string previousSection = null;
                            string getHistoryQuery = @"
                                SELECT history_id, current_section, previous_section 
                                FROM academic_history 
                                WHERE enrollment_id = @enrollmentId
                                ORDER BY effective_date DESC 
                                LIMIT 1";

                            using (MySqlCommand historyCmd = new MySqlCommand(getHistoryQuery, conn, transaction))
                            {
                                historyCmd.Parameters.AddWithValue("@enrollmentId", recordId);
                                using (var reader = historyCmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        historyId = reader.IsDBNull(0) ? (int?)null : reader.GetInt32(0);
                                        currentSection = reader.IsDBNull(1) ? null : reader.GetString(1);
                                        previousSection = reader.IsDBNull(2) ? null : reader.GetString(2);
                                    }
                                }
                            }


                            string newSection = DetermineSection(courseCode, yearLevel, semester, recordId);

                            if (currentSection != newSection || !historyId.HasValue)
                            {
                                string updatedPreviousSection = currentSection ?? previousSection;

                                string upsertHistoryQuery = @"
                            INSERT INTO academic_history 
                            (history_id, enrollment_id, previous_section, current_section, changed_by, effective_date)
                            VALUES (
                                @historyId,
                                @enrollmentId, 
                                @previousSection, 
                                @currentSection, 
                                @changedBy, 
                                NOW()
                            )
                            ON DUPLICATE KEY UPDATE
                                previous_section = COALESCE(VALUES(previous_section), previous_section),
                                current_section = VALUES(current_section),
                                changed_by = VALUES(changed_by),
                                effective_date = VALUES(effective_date)";

                                using (MySqlCommand upsertCmd = new MySqlCommand(upsertHistoryQuery, conn, transaction))
                                {
                                    upsertCmd.Parameters.AddWithValue("@historyId", historyId.HasValue ? (object)historyId.Value : DBNull.Value);
                                    upsertCmd.Parameters.AddWithValue("@enrollmentId", recordId);
                                    upsertCmd.Parameters.AddWithValue("@previousSection",
                                        string.IsNullOrEmpty(updatedPreviousSection) ? DBNull.Value : (object)updatedPreviousSection);
                                    upsertCmd.Parameters.AddWithValue("@currentSection", newSection);
                                    upsertCmd.Parameters.AddWithValue("@changedBy", changedByUserId);
                                    upsertCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    

                        // Send confirmation email if needed
                        if (sendEmail)
                        {
                            string getEmailQuery = @"
                        SELECT u.email, CONCAT(s.first_name, ' ', s.last_name) AS full_name
                        FROM student_enrollments se
                        JOIN students s ON se.student_id = s.student_id
                        JOIN users u ON s.user_id = u.user_id
                        WHERE se.enrollment_id = @enrollmentId";

                            using (MySqlCommand emailCmd = new MySqlCommand(getEmailQuery, conn, transaction))
                            {
                                emailCmd.Parameters.AddWithValue("@enrollmentId", recordId);
                                using (var reader = emailCmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        email = reader.GetString("email");
                                        fullName = reader.GetString("full_name");
                                    }
                                }
                            }
                        }

                        transaction.Commit();

                        if (sendEmail && !string.IsNullOrEmpty(email))
                        {
                            await SendEnrollmentConfirmationEmail(email, fullName, courseCode, yearLevel, semester, academicYear);
                        }

                        // Update UI
                        LoadStudentData();
                        RefreshDataGridView(DataGridNewEnrollment);
                        MessageBox.Show($"{successMessage}\nStudent: {studentName}",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadPaidEnrollments();
                        LoadPaymentData();
                        ClearDetails();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"An error occurred: {ex.Message}\n\nPlease try again.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Debug.WriteLine($"ProcessConfirmation Error: {ex}");
                    }
                }
            }
        }

        private string GetPreviousSection(int enrollmentId)
        {
            return null; 
        }

        private string DetermineSection(string courseCode, string yearLevel, string semester, int enrollmentId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Use the class-level _maxStudentsPerSection variable
                    string availableSectionQuery = @"
                    SELECT ah.current_section, COUNT(*) as student_count
                    FROM academic_history ah
                    JOIN student_enrollments se ON ah.enrollment_id = se.enrollment_id
                    WHERE se.course_id = (SELECT course_id FROM courses WHERE course_code = @courseCode)
                    AND se.year_level = @yearLevel
                    AND se.semester = @semester
                    AND se.status = 'Enrolled'
                    GROUP BY ah.current_section
                    HAVING student_count < @maxStudents
                    ORDER BY ah.current_section
                    LIMIT 1";

                    string availableSection = null;
                    using (MySqlCommand sectionCmd = new MySqlCommand(availableSectionQuery, conn))
                    {
                        sectionCmd.Parameters.AddWithValue("@courseCode", courseCode);
                        sectionCmd.Parameters.AddWithValue("@yearLevel", yearLevel);
                        sectionCmd.Parameters.AddWithValue("@semester", semester);
                        sectionCmd.Parameters.AddWithValue("@maxStudents", _maxStudentsPerSection);

                        using (var reader = sectionCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                availableSection = reader.GetString("current_section");
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(availableSection))
                    {
                        string maxSectionQuery = @"
                    SELECT current_section
                    FROM academic_history ah
                    JOIN student_enrollments se ON ah.enrollment_id = se.enrollment_id
                    WHERE se.course_id = (SELECT course_id FROM courses WHERE course_code = @courseCode)
                    AND se.year_level = @yearLevel
                    AND se.semester = @semester
                    ORDER BY current_section DESC
                    LIMIT 1";

                        string maxSection = null;
                        using (MySqlCommand maxCmd = new MySqlCommand(maxSectionQuery, conn))
                        {
                            maxCmd.Parameters.AddWithValue("@courseCode", courseCode);
                            maxCmd.Parameters.AddWithValue("@yearLevel", yearLevel);
                            maxCmd.Parameters.AddWithValue("@semester", semester);

                            var result = maxCmd.ExecuteScalar();
                            maxSection = result != null ? result.ToString() : null;
                        }

                        char sectionLetter = 'A';
                        if (!string.IsNullOrEmpty(maxSection))
                        {
                            char lastLetter = maxSection[maxSection.Length - 1];
                            sectionLetter = (char)(lastLetter + 1);
                        }

                        string cleanYearLevel = yearLevel
                            .Replace("Year", "")
                            .Replace(" ", "")
                            .Replace("1st", "1")
                            .Replace("2nd", "2")
                            .Replace("3rd", "3")
                            .Replace("4th", "4");

                        string cleanSemester = semester[0].ToString();
                        availableSection = $"{courseCode}-{cleanYearLevel}{cleanSemester}-{sectionLetter}";
                    }

                    return availableSection;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error determining section: {ex.Message}");
                string cleanYearLevel = yearLevel
                    .Replace("Year", "")
                    .Replace(" ", "")
                    .Replace("1st", "1")
                    .Replace("2nd", "2")
                    .Replace("3rd", "3")
                    .Replace("4th", "4");
                string cleanSemester = semester[0].ToString();
                return $"{courseCode}-{cleanYearLevel}{cleanSemester}-A";
            }
        }

        private void UpdateAcademicHistory(int enrollmentId, string currentSection, string previousSection)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                    INSERT INTO academic_history 
                    (enrollment_id, current_section, previous_section, changed_by, effective_date)
                    VALUES (@enrollmentId, @currentSection, @previousSection, @changedBy, NOW())";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@enrollmentId", enrollmentId);
                        cmd.Parameters.AddWithValue("@currentSection", currentSection);
                        cmd.Parameters.AddWithValue("@previousSection", string.IsNullOrEmpty(previousSection) ? DBNull.Value : (object)previousSection);
                        cmd.Parameters.AddWithValue("@changedBy", (int)SessionManager.UserId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating academic history: {ex.Message}");
            }
        }

        private void ClearDetails()
        {
            TxtLastName.Clear();
            TxtFirstName.Clear();
            TxtMiddleName.Clear();
            TxtStudentNo.Clear();
            TxtCourseName.Clear();
            TxtYrLevel.Clear();
            TxtSemester.Clear();
            PicBoxID.Image = Properties.Resources.PROFILE;
        }

        private void BtnPayment_Click(object sender, EventArgs e)
        {
            if (DataGridNewEnrollment.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = DataGridNewEnrollment.SelectedRows[0];
                int enrollmentId = Convert.ToInt32(selectedRow.Cells["enrollment_id"].Value);
                string studentName = $"{selectedRow.Cells["last_name"].Value}, {selectedRow.Cells["first_name"].Value}";

                DialogResult result = MessageBox.Show(
                    $"Move {studentName} to payment processing?",
                    "Confirm Payment",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();
                            string updateQuery = "UPDATE student_enrollments SET status = 'Payment Pending' WHERE enrollment_id = @enrollmentId";

                            using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@enrollmentId", enrollmentId);
                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    LoadStudentData();
                                    LoadPaymentData();
                                    LoadPaidEnrollments();
                                    RefreshDataGridView(DataGridNewEnrollment);
                                    MessageBox.Show("Student moved to payment processing!", "Success",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating database: " + ex.Message,
                                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a student to move to payment.",
                               "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadPaymentData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            p.payment_id,
                            s.student_no,
                            s.last_name,
                            s.first_name,
                            s.middle_name,
                            c.course_code AS Program,
                            se.academic_year,
                            se.semester,
                            se.year_level,
                            se.status
                        FROM payments p
                        INNER JOIN student_enrollments se ON p.enrollment_id = se.enrollment_id
                        INNER JOIN students s ON se.student_id = s.student_id
                        INNER JOIN courses c ON se.course_id = c.course_id
                        WHERE se.status = 'Payment Pending'";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataGridPayment.AutoGenerateColumns = false;
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            DataGridPayment.DataSource = dt;
                            UpdateRowNumbersPyment();
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

        private void LoadPaidEnrollments()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            p.payment_id,
                            s.student_no,
                            s.last_name,
                            s.first_name,
                            s.middle_name,
                            c.course_code AS Program,
                            se.academic_year,
                            se.semester,
                            se.year_level,
                            se.status,
                            p.payment_date,
                            p.amount_paid,
                            p.payment_method,
                            p.payment_status
                        FROM payments p
                        INNER JOIN student_enrollments se ON p.enrollment_id = se.enrollment_id
                        INNER JOIN students s ON se.student_id = s.student_id
                        INNER JOIN courses c ON se.course_id = c.course_id
                        WHERE p.payment_status = 'Completed'
                        ORDER BY p.payment_date DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        DataGridPaidEnrollment.DataSource = dt;
                        UpdateRowNumbesPaidEnrollment();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading paid enrollments: " + ex.Message,
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPayment)
            {
                LoadPaymentData();
            }
            else if (tabControl1.SelectedTab == tabPaidEnrollment)
            {
                LoadPaidEnrollments();
            }
            else if (tabControl1.SelectedTab == tabStudentEnrollment)
            {
                LoadPaidEnrollments();
            }
        }

        private void DataGridPayment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DataGridPayment.Rows[e.RowIndex];
                string studentNo = row.Cells["student_no_payment"].Value.ToString();
                FetchStudentDetails(studentNo);
            }
        }

        private async Task SendEnrollmentConfirmationEmail(string email, string studentName, string courseCode, string yearLevel, string semester, string academicYear)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress("enrollment.test101@gmail.com", "Enrollment System");
            var to = new EmailAddress(email, studentName);
            var subject = "Enrollment Confirmation";

            var plainTextContent = $"Dear {studentName},\n\n" +
                                   $"Congratulations! You are now officially enrolled in:\n" +
                                   $"Course: {courseCode}\nYear Level: {yearLevel}\nSemester: {semester}\nAcademic Year: {academicYear}\n\n" +
                                   $"Thank you!";

            var htmlContent = $@"
            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 20px; border-radius: 10px; background-color: #f9f9f9;'>
                <div style='text-align: center;'>
                    <h2 style='color: #0056b3;'>Enrollment Confirmation</h2>
                    <img src='https://i.imgur.com/cdAIpDj.png' alt='Enrollment Banner' style='max-width: 100%; height: auto; margin-bottom: 20px;' />
                </div>

                <p style='font-size: 16px;'>Dear <strong>{studentName}</strong>,</p>
                <p style='font-size: 16px;'>We are pleased to inform you that your enrollment has been <strong>successfully confirmed</strong>.</p>

                <div style='background-color: #ffffff; padding: 15px; border: 1px solid #ddd; border-radius: 5px; margin: 20px 0;'>
                    <p style='font-size: 16px;'><strong>Course:</strong> {courseCode}</p>
                    <p style='font-size: 16px;'><strong>Year Level:</strong> {yearLevel}</p>
                    <p style='font-size: 16px;'><strong>Semester:</strong> {semester}</p>
                    <p style='font-size: 16px;'><strong>Academic Year:</strong> {academicYear}</p>
                </div>

                <p style='font-size: 16px;'>If you have any questions or need further assistance, feel free to contact us at 
                    <a href='mailto:enrollment.test101@gmail.com' style='color: #0056b3;'>enrollment.test101@gmail.com</a>.
                </p>

                <p style='font-size: 14px; color: #888;'>© Enrollment System 2025</p>
            </div>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }

        private void DataGridPayment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Ignore header clicks

            if (e.ColumnIndex == DataGridPayment.Columns["ColOpen3"].Index)
            {
                // Open payment details
                int paymentId = Convert.ToInt32(DataGridPayment.Rows[e.RowIndex].Cells["payment_id_payment"].Value);
                AdminCashier cashier = new AdminCashier(paymentId);
                cashier.ShowDialog();

                // Refresh payment data after closing the cashier form
                LoadPaymentData();
            }
            else if (e.ColumnIndex == DataGridPayment.Columns["ColClose3"].Index)
            {
                // Delete payment
                DataGridViewRow row = DataGridPayment.Rows[e.RowIndex];
                DeletePayment(row);
            }
        }

        private void DataGridNewEnrollment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow row = DataGridNewEnrollment.Rows[e.RowIndex];
            string columnName = DataGridNewEnrollment.Columns[e.ColumnIndex].Name;

            if (columnName == "colOpen1")
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    int enrollmentId = Convert.ToInt32(row.Cells["enrollment_id"].Value);
                    ViewPdfFromDatabase(enrollmentId);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
            else if (columnName == "colClose1")
            {
                DeleteNewEnrollment(row);
            }
            else
            {
                string studentNo = row.Cells["student_no"].Value.ToString();
                FetchStudentDetails(studentNo);
            }
        }

        private void BtnDrop_Click(object sender, EventArgs e)
        {
            if (DataGridNewEnrollment.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an enrollment to drop.",
                              "No Selection",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = DataGridNewEnrollment.SelectedRows[0];
            DeleteNewEnrollment(selectedRow);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = textBox1.Text.ToLower();

            ApplySearchFilter(DataGridNewEnrollment, searchTerm);
            ApplySearchFilter(DataGridPaidEnrollment, searchTerm);
            ApplySearchFilter(DataGridPayment, searchTerm);
        }

        private void ApplySearchFilter(DataGridView dataGrid, string searchTerm)
        {
            DataTable dt = dataGrid.DataSource as DataTable;
            if (dt != null)
            {
                dt.DefaultView.RowFilter = string.Format("last_name LIKE '%{0}%' OR first_name LIKE '%{0}%' OR student_no LIKE '%{0}%'", searchTerm);
            }
        }

        private void DataGridNewEnrollment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Empty method preserved as requested
        }

        private void DataGridPaidEnrollment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView grid = (DataGridView)sender;

                if (grid.Columns[e.ColumnIndex].Name == "ColOpen2")
                {
                    int paymentId = Convert.ToInt32(grid.Rows[e.RowIndex].Cells["payment_id_pe"].Value);

                    AdminCashier cashier = new AdminCashier(paymentId);
                    cashier.ShowDialog();

                    LoadPaidEnrollments();
                }
            }
        }

        private void DataGridPaidEnrollment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (DataGridPaidEnrollment.Columns[e.ColumnIndex].Name != "ColOpen2" &&
                    DataGridPaidEnrollment.Columns[e.ColumnIndex].Name != "ColClose2")
                {
                    DataGridViewRow row = DataGridPaidEnrollment.Rows[e.RowIndex];
                    string studentNo = row.Cells["student_no_pe"].Value.ToString();
                    FetchStudentDetails(studentNo);
                }
            }
        }

        private void RefreshDataGridView(DataGridView dgv)
        {
            dgv.EndEdit();
            dgv.Refresh();
            dgv.Update();
            if (dgv.Parent != null)
            {
                dgv.Parent.Refresh();
            }
        }

        private bool IsPaymentComplete(int paymentId)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT 
                          is_unifast, 
                          receipt_no, 
                          amount_paid, 
                          payment_method 
                        FROM payments 
                        WHERE payment_id = @paymentId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@paymentId", paymentId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bool isUniFast = reader.GetBoolean("is_unifast");

                                // If UniFast, payment is considered complete
                                if (isUniFast) return true;

                                // Otherwise check all payment fields
                                return !reader.IsDBNull(reader.GetOrdinal("receipt_no")) &&
                                       !reader.IsDBNull(reader.GetOrdinal("amount_paid")) &&
                                       !reader.IsDBNull(reader.GetOrdinal("payment_method"));
                            }
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void BtnConfigureFees_Click(object sender, EventArgs e)
        {
            try
            {
                // Load current fees first
                decimal currentTuition = 0;
                decimal currentMisc = 0;
                bool hasExistingSettings = false;

                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT tuition_per_unit, miscellaneous_fee FROM fee_settings ORDER BY effective_date DESC LIMIT 1";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                currentTuition = reader.GetDecimal("tuition_per_unit");
                                currentMisc = reader.GetDecimal("miscellaneous_fee");
                                hasExistingSettings = true;
                            }
                        }
                    }
                }

                // Gold-brown theme colors
                Color darkBrown = Color.FromArgb(101, 67, 33);    // #654321
                Color gold = Color.FromArgb(218, 165, 32);       // #DAA520
                Color lightGold = Color.FromArgb(255, 215, 0);    // #FFD700
                Color cream = Color.FromArgb(255, 253, 208);     // #FFFDD0

                // Show fee configuration dialog
                using (var form = new Form())
                {
                    form.Text = "Update Fee Settings";
                    form.Size = new Size(400, 280);
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.FormBorderStyle = FormBorderStyle.FixedDialog;
                    form.MaximizeBox = false;
                    form.BackColor = cream;
                    form.ForeColor = darkBrown;
                    form.Font = new Font("Segoe UI", 9, FontStyle.Regular);

                    // Title label
                    var lblTitle = new Label
                    {
                        Text = "Fee Configuration",
                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                        ForeColor = darkBrown,
                        Left = 20,
                        Top = 15,
                        Width = 360,
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    var lblTuition = new Label
                    {
                        Text = "Tuition per Unit (₱):",
                        Left = 30,
                        Top = 60,
                        Width = 150,
                        ForeColor = darkBrown,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold)
                    };

                    var txtTuition = new NumericUpDown
                    {
                        Left = 190,
                        Top = 55,
                        Width = 170,
                        DecimalPlaces = 2,
                        Minimum = 0,
                        Maximum = 100000,
                        BackColor = Color.White,
                        ForeColor = darkBrown,
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    txtTuition.Value = Math.Min(currentTuition, txtTuition.Maximum);

                    var lblMisc = new Label
                    {
                        Text = "Miscellaneous Fee (₱):",
                        Left = 30,
                        Top = 100,
                        Width = 150,
                        ForeColor = darkBrown,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold)
                    };

                    var txtMisc = new NumericUpDown
                    {
                        Left = 190,
                        Top = 95,
                        Width = 170,
                        DecimalPlaces = 2,
                        Minimum = 0,
                        Maximum = 100000,
                        BackColor = Color.White,
                        ForeColor = darkBrown,
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    txtMisc.Value = Math.Min(currentMisc, txtMisc.Maximum);

                    // Add a note label
                    var lblNote = new Label
                    {
                        Text = "Note: Both values must be greater than 0.",
                        Left = 30,
                        Top = 140,
                        Width = 340,
                        ForeColor = Color.FromArgb(139, 69, 19), // SaddleBrown
                        Font = new Font("Segoe UI", 8, FontStyle.Italic)
                    };

                    var btnSave = new Button
                    {
                        Text = "Save",
                        Left = 190,
                        Top = 180,
                        Width = 80,
                        BackColor = gold,
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold)
                    };
                    btnSave.FlatAppearance.BorderColor = darkBrown;

                    var btnCancel = new Button
                    {
                        Text = "Cancel",
                        Left = 280,
                        Top = 180,
                        Width = 80,
                        BackColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = darkBrown,
                        Font = new Font("Segoe UI", 9)
                    };
                    btnCancel.FlatAppearance.BorderColor = darkBrown;

                    btnSave.Click += (s, ev) =>
                    {
                        if (txtTuition.Value <= 0 || txtMisc.Value <= 0)
                        {
                            MessageBox.Show("Both tuition and miscellaneous fees must be greater than ₱0.",
                                            "Invalid Input",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                            return;
                        }

                        if (txtTuition.Value == currentTuition && txtMisc.Value == currentMisc && hasExistingSettings)
                        {
                            MessageBox.Show("No changes were made to the fee settings.",
                                            "No Changes",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                            return;
                        }

                        SaveFeeSettings(txtTuition.Value, txtMisc.Value);
                        form.DialogResult = DialogResult.OK;
                        form.Close();
                    };

                    btnCancel.Click += (s, ev) => form.Close();

                    // Add gold border effect
                    Panel borderPanel = new Panel
                    {
                        BackColor = gold,
                        Location = new Point(5, 5),
                        Size = new Size(form.Width - 12, form.Height - 12),
                        Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
                    };

                    Panel contentPanel = new Panel
                    {
                        BackColor = cream,
                        Location = new Point(2, 2),
                        Size = new Size(borderPanel.Width - 4, borderPanel.Height - 4),
                        Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
                    };

                    borderPanel.Controls.Add(contentPanel);
                    contentPanel.Controls.AddRange(new Control[] { lblTitle, lblTuition, txtTuition, lblMisc, txtMisc, lblNote, btnSave, btnCancel });
                    form.Controls.Add(borderPanel);

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show("Fee settings updated successfully!",
                                        "Success",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating fees: {ex.Message}",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void SaveFeeSettings(decimal tuitionPerUnit, decimal miscFee)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
            INSERT INTO fee_settings (tuition_per_unit, miscellaneous_fee, updated_by, effective_date)
            VALUES (@tuition, @misc, @userId, NOW())";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tuition", tuitionPerUnit);
                    cmd.Parameters.AddWithValue("@misc", miscFee);
                    cmd.Parameters.AddWithValue("@userId", SessionManager.UserId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void BtnPerSection_Click(object sender, EventArgs e)
        {
            try
            {
                // Gold-brown theme colors
                Color darkBrown = Color.FromArgb(101, 67, 33);
                Color gold = Color.FromArgb(218, 165, 32);
                Color cream = Color.FromArgb(255, 253, 208);

                using (var form = new Form())
                {
                    form.Text = "Set Maximum Students Per Section";
                    form.Size = new Size(350, 200);
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.FormBorderStyle = FormBorderStyle.FixedDialog;
                    form.MaximizeBox = false;
                    form.BackColor = cream;
                    form.ForeColor = darkBrown;

                    // Title label
                    var lblTitle = new Label
                    {
                        Text = "Section Capacity Configuration",
                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                        ForeColor = darkBrown,
                        Left = 20,
                        Top = 15,
                        Width = 310,
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    var lblMaxStudents = new Label
                    {
                        Text = "Max Students Per Section:",
                        Left = 20,
                        Top = 60,
                        Width = 150,
                        ForeColor = darkBrown
                    };

                    var numMaxStudents = new NumericUpDown
                    {
                        Left = 180,
                        Top = 55,
                        Width = 100,
                        Minimum = 2,
                        Maximum = 50,
                        Value = _maxStudentsPerSection
                    };

                    var btnSave = new Button
                    {
                        Text = "Save",
                        Left = 150,
                        Top = 110,
                        Width = 80,
                        BackColor = gold,
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat
                    };
                    btnSave.FlatAppearance.BorderColor = darkBrown;

                    var btnCancel = new Button
                    {
                        Text = "Cancel",
                        Left = 240,
                        Top = 110,
                        Width = 80,
                        BackColor = Color.White,
                        ForeColor = darkBrown,
                        FlatStyle = FlatStyle.Flat
                    };
                    btnCancel.FlatAppearance.BorderColor = darkBrown;

                    btnSave.Click += (s, ev) =>
                    {
                        _maxStudentsPerSection = (int)numMaxStudents.Value;
                        MessageBox.Show($"Maximum students per section set to {_maxStudentsPerSection}",
                                        "Setting Saved",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        form.Close();
                    };

                    btnCancel.Click += (s, ev) => form.Close();

                    form.Controls.AddRange(new Control[] { lblTitle, lblMaxStudents, numMaxStudents, btnSave, btnCancel });
                    form.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error configuring section capacity: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}