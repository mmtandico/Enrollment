using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Enrollment_System
{
    public partial class FormEnrollment : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
        private FormCourse mainForm;

        public FormEnrollment()
        {
            InitializeComponent();
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            SetupForm();
        }

        private void SetupForm()
        {
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            UIHelper.ApplyAdminVisibility(BtnDataBase);
            this.Activated += FormEnrollment_Activated;

            SetupDataGridEvents();
            InitializeWelcomeLabel();
            ConfigureDataGrids();
        }

        private void SetupDataGridEvents()
        {
            DataGridEnrollment.CellClick += DataGridEnrollment_CellClick;
            DataGridEnrollment.CellMouseEnter += DataGridEnrollment_CellMouseEnter;
            DataGridEnrollment.CellMouseLeave += DataGridEnrollment_CellMouseLeave;

            DataGridEnrollment.Sorted += DataGridNewEnrollment_Sorted;
            DataGridPayment.Sorted += DataGridPayment_Sorted;
            DataGridSubjects.Sorted += DataGridSubjects_Sorted;

            DataGridPayment.DataError += DataGridPayment_DataError;
        }

        private void InitializeWelcomeLabel()
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

        private void ConfigureDataGrids()
        {
            ConfigureDataGridEnrollment();
            ConfigureDataGridPayment();
            ConfigureDataGridSubjects();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabEnrollment)
            {
                LoadEnrollmentData();
            }
            else if (tabControl1.SelectedTab == tabPayment)
            {
                LoadStudentPayments();  
            }
        }

        private void ConfigureDataGridEnrollment()
        {
            if (DataGridEnrollment.Columns.Count < 11)
                return; // Avoid index-out-of-range errors

            // Reset freeze and default formatting
            foreach (DataGridViewColumn col in DataGridEnrollment.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            DataGridEnrollment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridEnrollment.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            DataGridEnrollment.RowTemplate.Height = 35;

            DataGridEnrollment.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridEnrollment.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridEnrollment.AllowUserToResizeColumns = false;
            DataGridEnrollment.AllowUserToResizeRows = false;
            DataGridEnrollment.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Configure the row number column (column 1) to be non-sortable
            if (DataGridEnrollment.Columns.Count > 1)
            {
                DataGridEnrollment.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                DataGridEnrollment.Columns[1].ReadOnly = true;
                DataGridEnrollment.Columns[1].Width = 50;
                DataGridEnrollment.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Image columns: layout zoom
            var colOpen = DataGridEnrollment.Columns["ColOpen"] as DataGridViewImageColumn;
            if (colOpen != null)
            {
                colOpen.ImageLayout = DataGridViewImageCellLayout.Zoom;
                colOpen.Width = 50;
                colOpen.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                colOpen.SortMode = DataGridViewColumnSortMode.NotSortable; // Also make image column non-sortable
            }

            var colClose = DataGridEnrollment.Columns["ColClose"] as DataGridViewImageColumn;
            if (colClose != null)
            {
                colClose.ImageLayout = DataGridViewImageCellLayout.Zoom;
                colClose.Width = 50;
                colClose.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                colClose.SortMode = DataGridViewColumnSortMode.NotSortable; // Also make image column non-sortable
            }

            // Adjust specific columns
            int last = DataGridEnrollment.Columns.Count - 1;
            int secondLast = DataGridEnrollment.Columns.Count - 2;

            DataGridEnrollment.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridEnrollment.Columns[0].Width = 50;

            DataGridEnrollment.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridEnrollment.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrollment.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrollment.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrollment.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrollment.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridEnrollment.Columns[secondLast].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridEnrollment.Columns[secondLast].Width = 40;

            DataGridEnrollment.Columns[last].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridEnrollment.Columns[last].Width = 40;

            // Handle sorting to maintain proper numbering
            DataGridEnrollment.Sorted += (sender, e) =>
            {
                for (int i = 0; i < DataGridEnrollment.Rows.Count; i++)
                {
                    if (!DataGridEnrollment.Rows[i].IsNewRow)
                    {
                        DataGridEnrollment.Rows[i].Cells[1].Value = (i + 1).ToString();
                    }
                }
            };
        }


        private void ConfigureDataGridPayment()
        {
            if (DataGridPayment.Columns.Count < 3)
                return; // Avoid index out-of-range errors

            // Reset freeze, alignment, and sizing
            foreach (DataGridViewColumn col in DataGridPayment.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            // General settings
            DataGridPayment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridPayment.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            DataGridPayment.RowTemplate.Height = 35;
            DataGridPayment.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridPayment.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridPayment.AllowUserToResizeColumns = false;
            DataGridPayment.AllowUserToResizeRows = false;
            DataGridPayment.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Configure row number column (column 1) to be non-sortable and fixed
            if (DataGridPayment.Columns.Count > 1)
            {
                DataGridPayment.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                DataGridPayment.Columns[1].ReadOnly = true;
                DataGridPayment.Columns[1].Width = 50;
                DataGridPayment.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Set widths for action columns if they exist
            var colPay = DataGridPayment.Columns["ColPay"] as DataGridViewImageColumn;
            if (colPay != null)
            {
                colPay.Width = 50;
                colPay.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                colPay.ImageLayout = DataGridViewImageCellLayout.Zoom;
                colPay.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            var colDelete = DataGridPayment.Columns["ColDelete"] as DataGridViewImageColumn;
            if (colDelete != null)
            {
                colDelete.Width = 50;
                colDelete.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                colDelete.ImageLayout = DataGridViewImageCellLayout.Zoom;
                colDelete.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Configure first column (ID)
            if (DataGridPayment.Columns.Count > 0)
            {
                DataGridPayment.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                DataGridPayment.Columns[0].Width = 50;
            }

            // Configure last two columns (if they exist)
            int totalCols = DataGridPayment.Columns.Count;
            if (totalCols >= 2)
            {
                DataGridPayment.Columns[totalCols - 2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                DataGridPayment.Columns[totalCols - 2].Width = 40;

                DataGridPayment.Columns[totalCols - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                DataGridPayment.Columns[totalCols - 1].Width = 40;
            }

            for(int i = 0; i < DataGridPayment.Columns.Count; i++)
            {
                DataGridPayment.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Handle sorting to maintain proper numbering
            DataGridPayment.Sorted += (sender, e) =>
            {
                for (int i = 0; i < DataGridPayment.Rows.Count; i++)
                {
                    if (!DataGridPayment.Rows[i].IsNewRow)
                    {
                        DataGridPayment.Rows[i].Cells[1].Value = (i + 1).ToString();
                    }
                }
            };
        }

        private void ConfigureDataGridSubjects()
        {
            if (DataGridSubjects.Columns.Count < 8)
                return; // Ensure we have enough columns

            // Reset all column properties to defaults first
            foreach (DataGridViewColumn col in DataGridSubjects.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Configure general DataGridView properties
            DataGridSubjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridSubjects.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            DataGridSubjects.RowTemplate.Height = 35;
            DataGridSubjects.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridSubjects.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridSubjects.AllowUserToResizeColumns = false;
            DataGridSubjects.AllowUserToResizeRows = false;
            DataGridSubjects.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Configure specific columns
            DataGridSubjects.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;  // ID column
            DataGridSubjects.Columns[0].Width = 50;
            DataGridSubjects.Columns[0].Visible = false;  // Hide ID column

            DataGridSubjects.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;  // No. column
            DataGridSubjects.Columns[1].Width = 50;
            DataGridSubjects.Columns[1].ReadOnly = true;
            DataGridSubjects.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;  
            
        }

        private void FormEnrollment_Load(object sender, EventArgs e)
        {
            LoadEnrollmentData();
            LoadStudentPayments();
            ApplyDataGridStyles();
        }

        private void ApplyDataGridStyles()
        {
            CustomizeDataGrid();
            StyleTwoTabControl();
            StyleDataGridPayment();
            StyleDataGridSubjects();
        }

        private void LoadEnrollmentData()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT se.enrollment_id, s.student_id, s.student_no, s.last_name, 
                           s.first_name, s.middle_name, c.course_code, se.academic_year, 
                           se.semester, se.year_level, se.status
                           FROM student_enrollments se
                           INNER JOIN students s ON se.student_id = s.student_id
                           INNER JOIN courses c ON se.course_id = c.course_id
                           WHERE s.user_id = @UserID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", SessionManager.UserId);

                        DataGridEnrollment.Rows.Clear();
                        int rowNumber = 1;

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
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
                UpdateEnrollmentRowStyles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading enrollment data: " + ex.Message);
            }
        }

        private void UpdateEnrollmentRowStyles()
        {
            foreach (DataGridViewRow row in DataGridEnrollment.Rows)
            {
                string status = row.Cells[10].Value?.ToString()?.ToLower() ?? "";

                if (status == "enrolled" || status == "completed")
                {
                    row.Cells["ColOpen"].ReadOnly = true;
                    row.Cells["ColOpen"].Style.ForeColor = Color.Gray;
                    row.Cells["ColOpen"].Style.BackColor = Color.LightGray;

                    row.Cells["ColClose"].ReadOnly = true;
                    row.Cells["ColClose"].Style.ForeColor = Color.Gray;
                    row.Cells["ColClose"].Style.BackColor = Color.LightGray;
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

        private void LoadStudentPayments()
        {
            try
            {
                // First try to get StudentId from SessionManager
                int studentId = SessionManager.StudentId;

                // If not set, try to get it from database
                if (studentId <= 0)
                {
                    studentId = GetCurrentStudentId();
                    if (studentId <= 0)
                    {
                        DataGridPayment.Rows.Clear();
                        return;
                    }
                    SessionManager.StudentId = studentId;
                }

                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT p.payment_id, p.payment_date, p.total_units, 
                          p.total_amount_due, p.amount_paid, p.is_unifast, 
                          p.payment_method, p.receipt_no, se.academic_year, 
                          se.semester, p.payment_status,
                          (SELECT amount FROM payment_breakdowns WHERE payment_id = p.payment_id AND fee_type = 'Tuition') AS tuition,
                          (SELECT amount FROM payment_breakdowns WHERE payment_id = p.payment_id AND fee_type = 'Miscellaneous') AS miscellaneous
                          FROM payments p
                          INNER JOIN student_enrollments se ON p.enrollment_id = se.enrollment_id
                          WHERE se.student_id = @StudentId
                          ORDER BY p.payment_date DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", studentId);

                        DataGridPayment.Rows.Clear();
                        int rowNumber = 1;

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

        private int GetCurrentStudentId()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT student_id FROM students WHERE user_id = @UserID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", SessionManager.UserId);
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : -1;
                    }
                }
            }
            catch
            {
                return -1;
            }
        }

        private int LoadSubjectsForEnrollment(int courseId, string yearLevel, string semester)
        {
            int totalUnits = 0;
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT cs.course_subject_id, s.subject_code, s.subject_name, 
                           s.units, c.course_code, cs.semester AS semester1, 
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

                        // Add columns
                        DataGridSubjects.Columns.Add("course_subject_id", "ID");

                        // Add No. column with custom sorting behavior
                        DataGridViewColumn noColumn = new DataGridViewTextBoxColumn();
                        noColumn.Name = "No.";
                        noColumn.HeaderText = "No.";
                        noColumn.ReadOnly = true;
                        noColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                        DataGridSubjects.Columns.Add(noColumn);

                        DataGridSubjects.Columns.Add("subject_code", "Subject Code");
                        DataGridSubjects.Columns.Add("subject_name", "Subject Name");
                        DataGridSubjects.Columns.Add("units", "Units");
                        DataGridSubjects.Columns.Add("course_code", "Course Code");
                        DataGridSubjects.Columns.Add("semester1", "Semester");
                        DataGridSubjects.Columns.Add("year_level1", "Year Level");

                        // Configure column properties
                        DataGridSubjects.Columns["course_subject_id"].Visible = false;

                        // Set middle-center alignment for specific columns
                        string[] centerAlignedColumns = { "No.", "subject_code", "units", "course_code", "semester1", "year_level1" };
                        foreach (string colName in centerAlignedColumns)
                        {
                            DataGridSubjects.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            DataGridSubjects.Columns[colName].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }

                        // Set column widths
                        DataGridSubjects.Columns["No."].Width = 30;
                        DataGridSubjects.Columns["units"].Width = 50;
                        DataGridSubjects.Columns["subject_code"].Width = 50;
                       
                        DataGridSubjects.Columns["course_code"].Width = 60;
                        DataGridSubjects.Columns["semester1"].Width = 100;
                        DataGridSubjects.Columns["year_level1"].Width = 150;

                        // Populate rows with sequential numbers
                        int rowNumber = 1;
                        foreach (DataRow row in dt.Rows)
                        {
                            int rowIndex = DataGridSubjects.Rows.Add(
                                row["course_subject_id"],
                                rowNumber.ToString(), 
                                row["subject_code"],
                                row["subject_name"],
                                row["units"],
                                row["course_code"],
                                row["semester1"],
                                row["year_level1"]
                            );

                            // Store the actual row number in the Tag property
                            DataGridSubjects.Rows[rowIndex].Tag = rowNumber;

                            rowNumber++;
                            int units;
                            if (int.TryParse(row["units"].ToString(), out units))
                            {
                                totalUnits += units;
                            }
                        }

                        // Handle sorting to maintain proper numbering
                        DataGridSubjects.Sorted += (sender, e) =>
                        {
                            for (int i = 0; i < DataGridSubjects.Rows.Count; i++)
                            {
                                if (!DataGridSubjects.Rows[i].IsNewRow)
                                {
                                    DataGridSubjects.Rows[i].Cells["No."].Value = (i + 1).ToString();
                                }
                            }
                        };

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

        private void UpdatePaymentCalculation(int totalUnits)
        {
            decimal tuitionFee = CalculateTuitionFee(totalUnits);
            decimal miscFee = CalculateMiscellaneousFee();
            decimal totalAmountDue = tuitionFee + miscFee;

            LblTuitionFee1.Text = tuitionFee.ToString("0.00");
            LblMiscFee1.Text = miscFee.ToString("0.00");
            LblTotalAmount.Text = totalAmountDue.ToString("0.00");
        }

        private decimal CalculateTuitionFee(int totalUnits) => totalUnits * 150.00m;
        private decimal CalculateMiscellaneousFee() => 800.00m;

        private void CustomizeDataGrid()
        {
            ApplyDataGridStyle(DataGridEnrollment);
            ApplyDataGridStyle(DataGridPayment);
        }

        private void ApplyDataGridStyle(DataGridView dataGrid)
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

        private void StyleTwoTabControl()
        {
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.ItemSize = new Size(160, 36);

            Color darkBrown = Color.FromArgb(94, 55, 30);
            Color mediumBrown = Color.FromArgb(139, 69, 19);
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

                using (var brush = new LinearGradientBrush(
                    tabRect,
                    isSelected ? Color.FromArgb(255, 230, 170) : mediumBrown,
                    isSelected ? goldYellow : mediumBrown,
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(brush, tabRect);
                }

                using (var pen = new Pen(isSelected ? goldYellow : darkBrown, isSelected ? 2f : 1f))
                {
                    g.DrawRectangle(pen, tabRect);
                }

                TextRenderer.DrawText(
                    g,
                    currentTab.Text,
                    new Font(tabControl1.Font.FontFamily, 9f, isSelected ? FontStyle.Bold : FontStyle.Regular),
                    tabRect,
                    isSelected ? darkBrown : Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                if (isSelected)
                {
                    using (var pen = new Pen(goldYellow, 2))
                    {
                        int underlineY = tabRect.Bottom - 3;
                        g.DrawLine(pen, tabRect.Left + 10, underlineY, tabRect.Right - 10, underlineY);
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

            tabControl1.BackColor = Color.FromArgb(210, 180, 140);
            this.BackColor = Color.FromArgb(250, 240, 220);
        }

        private void StyleDataGridPayment()
        {
            ApplyDataGridStyle(DataGridPayment);
        }

        private void StyleDataGridSubjects()
        {
            ApplyDataGridStyle(DataGridSubjects);
        }

        // Event Handlers
        private void FormEnrollment_Activated(object sender, EventArgs e)
        {
            LoadEnrollmentData();
            LoadStudentPayments();
        }

        private void DataGridEnrollment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string columnName = DataGridEnrollment.Columns[e.ColumnIndex].Name;
                if (columnName == "ColOpen" || columnName == "ColClose") return;

                var selectedRow = DataGridEnrollment.Rows[e.RowIndex];
                string enrollmentId = selectedRow.Cells[0].Value?.ToString() ?? "";
                string studentNo = selectedRow.Cells[2].Value?.ToString() ?? "";
                string lastName = selectedRow.Cells[3].Value?.ToString() ?? "";
                string firstName = selectedRow.Cells[4].Value?.ToString() ?? "";
                string middleName = selectedRow.Cells[5].Value?.ToString() ?? "";
                string courseCode = selectedRow.Cells[6].Value?.ToString() ?? "";
                string academicYear = selectedRow.Cells[7].Value?.ToString() ?? "";
                string semester = selectedRow.Cells[8].Value?.ToString() ?? "";
                string yearLevel = selectedRow.Cells[9].Value?.ToString() ?? "";

                int courseId = GetCourseId(courseCode);
                int totalUnits = courseId > 0 ? LoadSubjectsForEnrollment(courseId, yearLevel, semester) : 0;

                // Get the current section for this enrollment
                string currentSection = GetCurrentSection(enrollmentId);

                UpdateStudentInfoPanel(
                    studentNo,
                    $"{firstName} {middleName} {lastName}",
                    courseCode,
                    academicYear,
                    semester,
                    yearLevel,
                    totalUnits,
                    currentSection
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

            DataGridViewRow selectedRow;
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

                if (MessageBox.Show($"Are you sure you want to drop {studentName}'s enrollment?",
                    "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        if (DeleteEnrollment(enrollmentId))
                        {
                            DataGridEnrollment.Rows.RemoveAt(e.RowIndex);
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

            if (e.ColumnIndex == DataGridPayment.Columns["ColPay"].Index && e.RowIndex >= 0)
            {
                int enrollmentId = Convert.ToInt32(DataGridPayment.Rows[e.RowIndex].Cells["payment_id"].Value);
                FormPayment formPayment = new FormPayment(enrollmentId);
                formPayment.ShowDialog();
            }
            else if (e.ColumnIndex == DataGridPayment.Columns["ColDelete"].Index && e.RowIndex >= 0)
            {
                if (MessageBox.Show("Are you sure you want to delete this row?",
                    "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DataGridPayment.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        private void BtnAddAcademic_Click(object sender, EventArgs e)
        {
            if (!ValidationHelper.IsPersonalInfoComplete(SessionManager.UserId))
            {
                ValidationHelper.ShowValidationError(this);
                SwitchForm(new FormPersonalInfo());
                return;
            }

            string status = GetActiveEnrollmentStatus();

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
                if (formNewAcademiccs.ShowDialog() == DialogResult.OK)
                {
                    // Force refresh all data
                    LoadEnrollmentData();

                    // Explicitly load payments and switch to payment tab
                    tabControl1.SelectedTab = tabPayment;
                    LoadStudentPayments();

                    // Additional check to ensure data loaded
                    if (DataGridPayment.Rows.Count == 0)
                    {
                        // If still empty, try reloading after a small delay
                        Task.Delay(100).ContinueWith(t =>
                        {
                            this.Invoke((MethodInvoker)delegate {
                                LoadStudentPayments();
                            });
                        });
                    }
                }
            }
        }

        // Helper Methods
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

        private string GetCurrentSection(string enrollmentId)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT current_section 
                           FROM academic_history 
                           WHERE enrollment_id = @EnrollmentId
                           ORDER BY effective_date DESC 
                           LIMIT 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EnrollmentId", enrollmentId);
                        object result = cmd.ExecuteScalar();
                        return result?.ToString() ?? "Not Assigned";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting current section: " + ex.Message);
                return "Error";
            }
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
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting enrollment: " + ex.Message);
            }
        }

        private string GetActiveEnrollmentStatus()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT status FROM student_enrollments 
                                 WHERE student_id = @StudentId 
                                 AND status IN ('Pending', 'Payment Pending', 'Enrolled') 
                                 LIMIT 1";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", SessionManager.StudentId);
                        return cmd.ExecuteScalar()?.ToString();
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

        private void UpdateStudentInfoPanel(string studentNo, string fullName, string courseCode,
                                   string academicYear, string semester, string yearLevel,
                                   int totalUnits, string currentSection)
        {
            LblStudentNo.Text = studentNo;
            LblName.Text = fullName;
            LblCourse.Text = courseCode;
            LblAcademicYear.Text = academicYear;
            LblSemester.Text = semester;
            LblYearLevel.Text = yearLevel;
            LblTotalUnit.Text = totalUnits.ToString();
            LblSection.Text = currentSection;
        }

        private void SwitchForm(Form newForm)
        {
            this.Hide();
            newForm.Show();
            this.Close();
        }

        // Navigation Button Events
        private void BtnHome_Click(object sender, EventArgs e) => SwitchForm(new FormHome());
        private void BtnPI_Click(object sender, EventArgs e) => SwitchForm(new FormPersonalInfo());
        private void BtnDataBase_Click(object sender, EventArgs e) => SwitchForm(new FormDatabaseInfo());
        private void BtnCourses_Click(object sender, EventArgs e) => SwitchForm(new FormCourse());
        private void BtnExit_Click(object sender, EventArgs e) => Application.Exit();

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out?",
                "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                new FormLogin().Show();
                this.Close();
            }
        }

        // Empty methods preserved to prevent errors
        private void DataGridPayment_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) { }
        private void DataGridPayment_DataError(object sender, DataGridViewDataErrorEventArgs e) { }
        private void DataGridEnrollment_CellMouseEnter(object sender, DataGridViewCellEventArgs e) { }
        private void DataGridEnrollment_CellMouseLeave(object sender, DataGridViewCellEventArgs e) { }
        private void DataGridNewEnrollment_Sorted(object sender, EventArgs e) { }
        private void DataGridPayment_Sorted(object sender, EventArgs e) { }
        private void DataGridSubjects_Sorted(object sender, EventArgs e) { }
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e) { }
        private void DataGridPayment_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void DataGridSubjects_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void panel10_Paint(object sender, PaintEventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
    }
}