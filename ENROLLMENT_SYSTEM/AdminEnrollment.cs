using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace Enrollment_System
{
    public partial class AdminEnrollment : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";

        private string currentProgramFilter = "All";
        private Button[] programButtons;

        public AdminEnrollment()
        {
            InitializeComponent();
            InitializeProgramButtons();
            InitializeDataGridView();
            LoadStudentData();
            InitializeFilterControls();

            ProgramButton_Click(BtnAll, EventArgs.Empty);

        }
        private void AdminEnrollment_Load(object sender, EventArgs e)
        {
            DataGridNewEnrollment.CellClick += DataGridNewEnrollment_CellClick;

            StyleTwoTabControl();
            InitializeDataGridView();

            InitializeFilterControls();
            DataGridNewEnrollment.AutoGenerateColumns = false;
            student_id.DataPropertyName = "enrollment_id";
            student_no.DataPropertyName = "student_no";
            last_name.DataPropertyName = "last_name";
            first_name.DataPropertyName = "first_name";
            middle_name.DataPropertyName = "middle_name";
            courseCode.DataPropertyName = "Program";
            academic_year.DataPropertyName = "academic_year";
            semester.DataPropertyName = "semester";
            year_level.DataPropertyName = "year_level";
            status.DataPropertyName = "status";

            LoadStudentData();


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

            CustomizeDataGridNewEnrollment();
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
                    se.status
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

        private void DataGridNewEnrollment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // ensure a valid row was clicked
            {
                DataGridViewRow row = DataGridNewEnrollment.Rows[e.RowIndex];

                string studentNo = row.Cells["student_no"].Value.ToString();

                FetchStudentDetails(studentNo);
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
                WHERE s.student_no = @studentNo
            ";

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


                                // Load profile picture
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

    }
}
