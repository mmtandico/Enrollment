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

namespace Enrollment_System
{
    public partial class AdminEnrollment : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";

        private readonly string _sendGridApiKey = ConfigurationManager.AppSettings["SendGridApiKey"];

        private string currentProgramFilter = "All";
        private Button[] programButtons;

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            LoadPaymentData(); 
            LoadStudentData(); 
        }
        
        public AdminEnrollment()
        {
            InitializeComponent();
            InitializeProgramButtons();
            InitializeDataGridView();
            LoadStudentData();
            InitializeFilterControls();

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


            LoadStudentData();
            LoadPaymentData();

            DataGridNewEnrollment.AllowUserToResizeColumns = false;
            DataGridNewEnrollment.AllowUserToResizeRows = false;
            DataGridNewEnrollment.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn column in DataGridNewEnrollment.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            int totalCols1 = DataGridNewEnrollment.Columns.Count;
            DataGridNewEnrollment.Columns[totalCols1 - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridNewEnrollment.Columns[totalCols1 - 1].Width = 40;
            DataGridNewEnrollment.Columns[totalCols1 - 2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridNewEnrollment.Columns[totalCols1 - 2].Width = 40;
            DataGridNewEnrollment.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridNewEnrollment.Columns[0].Width = 50;
            DataGridNewEnrollment.RowTemplate.Height = 35;
            DataGridNewEnrollment.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridNewEnrollment.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridNewEnrollment.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridNewEnrollment.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridNewEnrollment.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridNewEnrollment.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            foreach (DataGridViewColumn col in DataGridNewEnrollment.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
            ////////////////////////////
            DataGridPaidEnrollment.AllowUserToResizeColumns = false;
            DataGridPaidEnrollment.AllowUserToResizeRows = false;
            DataGridPaidEnrollment.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn column in DataGridPaidEnrollment.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            int totalCols2 = DataGridPaidEnrollment.Columns.Count;
            DataGridPaidEnrollment.Columns[totalCols2 - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPaidEnrollment.Columns[totalCols2 - 1].Width = 40;
            DataGridPaidEnrollment.Columns[totalCols2 - 2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPaidEnrollment.Columns[totalCols2 - 2].Width = 40;
            DataGridPaidEnrollment.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPaidEnrollment.Columns[0].Width = 50;
            DataGridPaidEnrollment.RowTemplate.Height = 35;
            DataGridPaidEnrollment.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridPaidEnrollment.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridPaidEnrollment.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridPaidEnrollment.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridPaidEnrollment.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridPaidEnrollment.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            foreach (DataGridViewColumn col in DataGridPaidEnrollment.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
            ///////////////////////////////
            DataGridPayment.AllowUserToResizeColumns = false;
            DataGridPayment.AllowUserToResizeRows = false;
            DataGridPayment.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn column in DataGridPayment.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            int totalCols3 = DataGridPayment.Columns.Count;
            DataGridPayment.Columns[totalCols3 - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPayment.Columns[totalCols3 - 1].Width = 40;
            DataGridPayment.Columns[totalCols3 - 2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPayment.Columns[totalCols3 - 2].Width = 40;
            DataGridPayment.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPayment.Columns[0].Width = 50;
            DataGridPayment.RowTemplate.Height = 35;
            DataGridPayment.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridPayment.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridPayment.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridPayment.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridPayment.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridPayment.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            foreach (DataGridViewColumn col in DataGridPayment.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }

            CustomizeDataGridNewEnrollment();
            CustomizeDataGridPaidEnrollment();
            CustomizeDataGridPayment();
            StyleTwoTabControl();

        }

        private void InitializeDataGridView()
        {
            DataGridNewEnrollment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridNewEnrollment.Columns["ColOpen1"].Width = 50;
            DataGridNewEnrollment.Columns["ColClose1"].Width = 50;
            DataGridNewEnrollment.Columns["ColOpen1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridNewEnrollment.Columns["ColClose1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridNewEnrollment.RowTemplate.Height = 40;
            DataGridNewEnrollment.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;


            DataGridNewEnrollment.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridNewEnrollment.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewImageColumn colOpen1 = (DataGridViewImageColumn)DataGridNewEnrollment.Columns["ColOpen1"];
            colOpen1.ImageLayout = DataGridViewImageCellLayout.Zoom;

            DataGridViewImageColumn colClose1 = (DataGridViewImageColumn)DataGridNewEnrollment.Columns["ColClose1"];
            colClose1.ImageLayout = DataGridViewImageCellLayout.Zoom;

            foreach (DataGridViewColumn col in DataGridNewEnrollment.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
            ///////////////////////////////
            DataGridPaidEnrollment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridPaidEnrollment.Columns["ColOpen2"].Width = 50;
            DataGridPaidEnrollment.Columns["ColClose2"].Width = 50;
            DataGridPaidEnrollment.Columns["ColOpen2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPaidEnrollment.Columns["ColClose2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPaidEnrollment.RowTemplate.Height = 40;
            DataGridPaidEnrollment.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;


            DataGridPaidEnrollment.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridPaidEnrollment.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewImageColumn colOpen2 = (DataGridViewImageColumn)DataGridPaidEnrollment.Columns["ColOpen2"];
            colOpen2.ImageLayout = DataGridViewImageCellLayout.Zoom;

            DataGridViewImageColumn colClose2 = (DataGridViewImageColumn)DataGridPaidEnrollment.Columns["ColClose2"];
            colClose2.ImageLayout = DataGridViewImageCellLayout.Zoom;

            foreach (DataGridViewColumn col in DataGridPaidEnrollment.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
            /////////////////////////////////
            DataGridPayment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridPayment.Columns["ColOpen3"].Width = 50;
            DataGridPayment.Columns["ColClose3"].Width = 50;
            DataGridPayment.Columns["ColOpen3"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPayment.Columns["ColClose3"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPayment.RowTemplate.Height = 40;
            DataGridPayment.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;


            DataGridPayment.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridPayment.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewImageColumn colOpen3 = (DataGridViewImageColumn)DataGridPayment.Columns["ColOpen3"];
            colOpen3.ImageLayout = DataGridViewImageCellLayout.Zoom;

            DataGridViewImageColumn colClose3 = (DataGridViewImageColumn)DataGridPayment.Columns["ColClose3"];
            colClose3.ImageLayout = DataGridViewImageCellLayout.Zoom;

            foreach (DataGridViewColumn col in DataGridPayment.Columns)
            {
                col.Frozen = false;
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

            CmbYrLvl.SelectedIndexChanged += ApplyFilters;
            CmbSem.SelectedIndexChanged += ApplyFilters;
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
                            DataGridNewEnrollment.DataSource = dt;

                            // Ensure column stays hidden if it exists
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


        private void ApplyFilters(object sender, EventArgs e)
        {
            string yearLevelFilter = CmbYrLvl.SelectedItem.ToString();
            string semesterFilter = CmbSem.SelectedItem.ToString();

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

            if (DataGridNewEnrollment.DataSource is DataTable)
            {
                DataTable dt = (DataTable)DataGridNewEnrollment.DataSource;
                dt.DefaultView.RowFilter = filterExpression;
            }
        }

        private void ProgramButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null) return;

            currentProgramFilter = clickedButton == BtnAll ? "All" : clickedButton.Text.Replace("Btn", "");

            ApplyFilters(null, null);
        }

        private void StyleDataGridEnrolled()
        {
            DataGridNewEnrollment.BorderStyle = BorderStyle.None;

            DataGridNewEnrollment.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridNewEnrollment.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridNewEnrollment.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridNewEnrollment.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridNewEnrollment.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridNewEnrollment.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridNewEnrollment.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridNewEnrollment.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridNewEnrollment.EnableHeadersVisualStyles = false;

            DataGridNewEnrollment.GridColor = Color.BurlyWood;

            DataGridNewEnrollment.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridNewEnrollment.RowTemplate.Height = 35;

            DataGridNewEnrollment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn column in DataGridNewEnrollment.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }

        }

        private void StyleDataGridPaidEnrollment()
        {
            DataGridPaidEnrollment.BorderStyle = BorderStyle.None;

            DataGridPaidEnrollment.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridPaidEnrollment.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridPaidEnrollment.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridPaidEnrollment.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridPaidEnrollment.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridPaidEnrollment.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridPaidEnrollment.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridPaidEnrollment.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridPaidEnrollment.EnableHeadersVisualStyles = false;

            DataGridPaidEnrollment.GridColor = Color.BurlyWood;

            DataGridPaidEnrollment.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridPaidEnrollment.RowTemplate.Height = 35;

            DataGridPaidEnrollment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn column in DataGridPaidEnrollment.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }

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



        private void CustomizeDataGridNewEnrollment()
        {
            DataGridNewEnrollment.BorderStyle = BorderStyle.None;

            DataGridNewEnrollment.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridNewEnrollment.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridNewEnrollment.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridNewEnrollment.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridNewEnrollment.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridNewEnrollment.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridNewEnrollment.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridNewEnrollment.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridNewEnrollment.EnableHeadersVisualStyles = false;

            DataGridNewEnrollment.GridColor = Color.BurlyWood;

            DataGridNewEnrollment.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridNewEnrollment.RowTemplate.Height = 35;

            DataGridNewEnrollment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            foreach (DataGridViewColumn column in DataGridNewEnrollment.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }
        }

        private void CustomizeDataGridPaidEnrollment()
        {
            DataGridPaidEnrollment.BorderStyle = BorderStyle.None;

            DataGridPaidEnrollment.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridPaidEnrollment.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridPaidEnrollment.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridPaidEnrollment.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridPaidEnrollment.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridPaidEnrollment.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridPaidEnrollment.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridPaidEnrollment.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridPaidEnrollment.EnableHeadersVisualStyles = false;

            DataGridPaidEnrollment.GridColor = Color.BurlyWood;

            DataGridPaidEnrollment.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridPaidEnrollment.RowTemplate.Height = 35;

            DataGridPaidEnrollment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            foreach (DataGridViewColumn column in DataGridPaidEnrollment.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }
        }

        private void CustomizeDataGridPayment()
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

        private void ViewPdfFromBinary(byte[] pdfData, int enrollmentId)
        {
            try
            {
                string tempPath = Path.Combine(Path.GetTempPath(), $"grade_{enrollmentId}.pdf");
                File.WriteAllBytes(tempPath, pdfData);
                System.Diagnostics.Process.Start(tempPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening PDF: {ex.Message}");
            }
        }

        private void ViewPdfFromPath(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    System.Diagnostics.Process.Start(filePath);
                }
                else
                {
                    MessageBox.Show("PDF file not found at the specified path.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening PDF: {ex.Message}");
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
                                // Try binary data first
                                if (reader["grade_pdf"] != DBNull.Value)
                                {
                                    byte[] fileData = (byte[])reader["grade_pdf"];
                                    string tempPath = Path.Combine(Path.GetTempPath(), $"grade_{enrollmentId}.pdf");
                                    File.WriteAllBytes(tempPath, fileData);
                                    System.Diagnostics.Process.Start(tempPath);
                                }
                                // Fall back to file path
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

                if (tabControl1.SelectedTab == tabPayment)
                {
                    currentGrid = DataGridPayment;

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
                    newStatus = "Pending";
                    confirmationMessage = "Are you sure you want to confirm this payment and return the student to pending status?";
                    successMessage = "Payment confirmed! Student returned to pending status.";
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

                // Get these values here so you can pass them to the email method
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
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        // For payment tab, we need to get the enrollment_id first
                        int recordId;
                        string updateQuery;

                        if (tabControl1.SelectedTab == tabPayment)
                        {
                            int paymentId = Convert.ToInt32(selectedRow.Cells[enrollmentIdColumn].Value);

                            // First get the enrollment_id associated with this payment
                            string getEnrollmentIdQuery = "SELECT enrollment_id FROM payments WHERE payment_id = @paymentId";
                            using (MySqlCommand getCmd = new MySqlCommand(getEnrollmentIdQuery, conn))
                            {
                                getCmd.Parameters.AddWithValue("@paymentId", paymentId);
                                recordId = Convert.ToInt32(getCmd.ExecuteScalar());
                            }

                            updateQuery = "UPDATE student_enrollments SET status = @newStatus WHERE enrollment_id = @recordId";
                        }
                        else
                        {
                            recordId = Convert.ToInt32(selectedRow.Cells[enrollmentIdColumn].Value);
                            updateQuery = "UPDATE student_enrollments SET status = @newStatus WHERE enrollment_id = @recordId";
                        }

                        using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@newStatus", newStatus);
                            cmd.Parameters.AddWithValue("@recordId", recordId);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show($"{successMessage}\nStudent: {studentName}",
                                              "Success",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Information);

                                if (sendEmail)
                                {
                                    string getEmailQuery = @"
                                        SELECT u.email, CONCAT(s.first_name, ' ', s.last_name) AS full_name
                                        FROM student_enrollments se
                                        JOIN students s ON se.student_id = s.student_id
                                        JOIN users u ON s.user_id = u.user_id
                                        WHERE se.enrollment_id = @enrollmentId";

                                    using (MySqlCommand emailCmd = new MySqlCommand(getEmailQuery, conn))
                                    {
                                        emailCmd.Parameters.AddWithValue("@enrollmentId", recordId);
                                        using (MySqlDataReader reader = emailCmd.ExecuteReader())
                                        {
                                            if (reader.Read())
                                            {
                                                string email = reader.GetString("email");
                                                string fullName = reader.GetString("full_name");

                                                await SendEnrollmentConfirmationEmail(email, fullName, courseCode, yearLevel, semester, academicYear);
                                            }
                                        }
                                    }
                                }

                                LoadStudentData();
                                LoadPaymentData();
                                ClearDetails();
                            }
                            else
                            {
                                MessageBox.Show("No changes were made to the record.",
                                              "Warning",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Warning);
                            }
                        }
                    }
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
                                    MessageBox.Show("Student moved to payment processing!", "Success",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    LoadStudentData();
                                    LoadPaymentData();
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPayment) 
            {
                LoadPaymentData();
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
            // Check if the clicked cell is in the column containing the blue button
            if (e.ColumnIndex == DataGridPayment.Columns["ColOpen3"].Index && e.RowIndex >= 0)
            {
                // Open the FormPayment
                int enrollmentId = Convert.ToInt32(DataGridPayment.Rows[e.RowIndex].Cells["payment_id_payment"].Value);
                AdminCashier Cashier = new AdminCashier(enrollmentId);
                Cashier.ShowDialog();
            }// Check if the clicked cell is in the delete button column
            else if (e.ColumnIndex == DataGridPayment.Columns["ColClose3"].Index && e.RowIndex >= 0)
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

        private void DataGridNewEnrollment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow row = DataGridNewEnrollment.Rows[e.RowIndex];
            string columnName = DataGridNewEnrollment.Columns[e.ColumnIndex].Name;

            if (columnName == "colOpen1") // PDF View button
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
            else if (columnName == "colClose1") // Delete button
            {
                DeleteNewEnrollment(row);
            }
            else // Any other cell click
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

           
            if (DataGridNewEnrollment.DataSource is DataTable)
            {
                DataTable dt = (DataTable)DataGridNewEnrollment.DataSource;
                dt.DefaultView.RowFilter = string.Format("last_name LIKE '%{0}%' OR first_name LIKE '%{0}%' OR student_no LIKE '%{0}%'", searchTerm);
            }
            if (DataGridPaidEnrollment.DataSource is DataTable)
            {
                DataTable dt = (DataTable)DataGridPaidEnrollment.DataSource;
                dt.DefaultView.RowFilter = string.Format("last_name LIKE '%{0}%' OR first_name LIKE '%{0}%' OR student_no LIKE '%{0}%'", searchTerm);
            }
            if (DataGridPayment.DataSource is DataTable)
            {
                DataTable dt = (DataTable)DataGridPayment.DataSource;
                dt.DefaultView.RowFilter = string.Format("last_name LIKE '%{0}%' OR first_name LIKE '%{0}%' OR student_no LIKE '%{0}%'", searchTerm);
            }
        }

        private void DataGridNewEnrollment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
