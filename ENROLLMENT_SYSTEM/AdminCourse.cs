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
    public partial class AdminCourse : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
        private string currentProgramFilter = "All";
        private Button[] programButtons;

        private enum CurrentMode { Subjects, Prerequisites }
        private CurrentMode currentMode = CurrentMode.Subjects;
        private int currentPrerequisiteId = -1;

        public AdminCourse()
        {
            InitializeComponent();
            InitializeDataGridView();
            StyleTwoTabControl();
            InitializeFilterControls();
            InitializeProgramButtons();
            
            LoadSubjectsCourse();

            ProgramButton_Click(BtnAll, EventArgs.Empty);
        }
        
        private void AdminCourse_Load(object sender, EventArgs e)
        {

            StyleTwoTabControl();
            InitializeDataGridView();
            InitializeFilterControls();
            InitializeProgramButtons();
            

            LoadSubjectsCourse();
            //LoadPrerequisites();
            LoadCourseComboBox();

            DataGridSubjects.CellContentClick += DataGridSubjects_CellContentClick;
           // DataGridPrerequisite.CellContentClick += DataGridPrerequisite_CellContentClick;

            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;

            BtnAdd.Click += BtnAdd_Click;
            BtnUpdate.Click += BtnUpdate_Click;
            BtnDrop.Click += BtnDrop_Click;

            DataGridSubjects.AutoGenerateColumns = false;
            subject_id.DataPropertyName = "subject_id";
            subject_code.DataPropertyName = "subject_code";
            subject_name.DataPropertyName = "subject_name";
            units.DataPropertyName = "units";
            courseCode.DataPropertyName = "courseCode";
            semester.DataPropertyName = "semester";
            year_level.DataPropertyName = "year_level";

            DataGridSubjects.AllowUserToResizeColumns = false;
            DataGridSubjects.AllowUserToResizeRows = false;
            DataGridSubjects.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn column in DataGridSubjects.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            int totalCols1 = DataGridSubjects.Columns.Count;
            DataGridSubjects.Columns[totalCols1 - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridSubjects.Columns[totalCols1 - 1].Width = 40;
            DataGridSubjects.Columns[totalCols1 - 2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridSubjects.Columns[totalCols1 - 2].Width = 40;
            DataGridSubjects.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridSubjects.Columns[0].Width = 50;
            DataGridSubjects.Columns[1].Width = 150;
            DataGridSubjects.Columns[2].Width = 500;
            DataGridSubjects.Columns[3].Width = 50;
            DataGridSubjects.Columns[4].Width = 100;
            DataGridSubjects.Columns[5].Width = 100;
            DataGridSubjects.Columns[6].Width = 100;
            DataGridSubjects.RowTemplate.Height = 35;
            DataGridSubjects.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            foreach (DataGridViewColumn col in DataGridSubjects.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
            ////////////////////////
            DataGridPrerequisite.AllowUserToResizeColumns = false;
            DataGridPrerequisite.AllowUserToResizeRows = false;
            DataGridPrerequisite.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn column in DataGridPrerequisite.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            int totalCols2 = DataGridPrerequisite.Columns.Count;
            DataGridPrerequisite.Columns[totalCols2 - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPrerequisite.Columns[totalCols2 - 1].Width = 40;
            DataGridPrerequisite.Columns[totalCols2 - 2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPrerequisite.Columns[totalCols2 - 2].Width = 40;
            DataGridPrerequisite.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPrerequisite.Columns[0].Width = 50;
            DataGridPrerequisite.Columns[1].Width = 150;
            DataGridPrerequisite.Columns[2].Width = 500;
            DataGridPrerequisite.Columns[3].Width = 50;
            DataGridPrerequisite.Columns[4].Width = 100;
            DataGridPrerequisite.Columns[5].Width = 100;
            DataGridPrerequisite.Columns[6].Width = 100;
            DataGridPrerequisite.RowTemplate.Height = 35;
            DataGridPrerequisite.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            foreach (DataGridViewColumn col in DataGridPrerequisite.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }

            CustomizeDataGridPrerequisites();
            CustomizeDataGridSubjects();
            StyleTwoTabControl();

            
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentMode = tabControl1.SelectedIndex == 0 ? CurrentMode.Subjects : CurrentMode.Prerequisites;
            ClearFields();
        }

        private void InitializeDataGridView()
        {
            DataGridSubjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridSubjects.Columns["ColOpen1"].Width = 50;
            DataGridSubjects.Columns["ColClose1"].Width = 50;
            DataGridSubjects.Columns["ColOpen1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridSubjects.Columns["ColClose1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridSubjects.RowTemplate.Height = 40;
            DataGridSubjects.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;


            DataGridSubjects.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridSubjects.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewImageColumn colOpen1 = (DataGridViewImageColumn)DataGridSubjects.Columns["ColOpen1"];
            colOpen1.ImageLayout = DataGridViewImageCellLayout.Zoom;

            DataGridViewImageColumn colClose1 = (DataGridViewImageColumn)DataGridSubjects.Columns["ColClose1"];
            colClose1.ImageLayout = DataGridViewImageCellLayout.Zoom;

            foreach (DataGridViewColumn col in DataGridSubjects.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
            //////////////////////////
            DataGridPrerequisite.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridPrerequisite.Columns["ColOpen"].Width = 50;
            DataGridPrerequisite.Columns["ColClose"].Width = 50;
            DataGridPrerequisite.Columns["ColOpen"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPrerequisite.Columns["ColClose"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridPrerequisite.RowTemplate.Height = 40;
            DataGridPrerequisite.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;


            DataGridPrerequisite.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridPrerequisite.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewImageColumn colOpen = (DataGridViewImageColumn)DataGridPrerequisite.Columns["ColOpen"];
            colOpen.ImageLayout = DataGridViewImageCellLayout.Zoom;

            DataGridViewImageColumn colClose = (DataGridViewImageColumn)DataGridPrerequisite.Columns["ColClose"];
            colClose.ImageLayout = DataGridViewImageCellLayout.Zoom;

            foreach (DataGridViewColumn col in DataGridPrerequisite.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
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

        private void StyleDataGridPrerequisites()
        {
            DataGridPrerequisite.BorderStyle = BorderStyle.None;

            DataGridPrerequisite.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridPrerequisite.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridPrerequisite.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridPrerequisite.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridPrerequisite.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridPrerequisite.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridPrerequisite.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridPrerequisite.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridPrerequisite.EnableHeadersVisualStyles = false;

            DataGridPrerequisite.GridColor = Color.BurlyWood;

            DataGridPrerequisite.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridPrerequisite.RowTemplate.Height = 35;

            DataGridPrerequisite.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn column in DataGridPrerequisite.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }

        }

        private void CustomizeDataGridSubjects()
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

        private void CustomizeDataGridPrerequisites()
        {
            DataGridPrerequisite.BorderStyle = BorderStyle.None;

            DataGridPrerequisite.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridPrerequisite.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridPrerequisite.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridPrerequisite.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridPrerequisite.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridPrerequisite.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridPrerequisite.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridPrerequisite.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridPrerequisite.EnableHeadersVisualStyles = false;

            DataGridPrerequisite.GridColor = Color.BurlyWood;

            DataGridPrerequisite.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridPrerequisite.RowTemplate.Height = 35;

            DataGridPrerequisite.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            foreach (DataGridViewColumn column in DataGridPrerequisite.Columns)
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

        private void LoadSubjectsCourse()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                    SELECT 
                        s.subject_id,
                        s.subject_code,
                        s.subject_name,
                        s.units,
                        IFNULL(c.course_code, 'N/A') AS courseCode,
                        IFNULL(cs.semester, 'N/A') AS semester,
                        IFNULL(cs.year_level, 'N/A') AS year_level
                    FROM subjects s
                    LEFT JOIN course_subjects cs ON s.subject_id = cs.subject_id
                    LEFT JOIN courses c ON cs.course_id = c.course_id
                    ORDER BY s.subject_code, c.course_code";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        
                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            DataGridSubjects.AutoGenerateColumns = false;
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            DataGridSubjects.DataSource = dt;
                        }

                        DataGridSubjects.Columns["subject_id"].DefaultCellStyle.Alignment =
                            DataGridViewContentAlignment.MiddleCenter;
                        DataGridSubjects.Columns["subject_code"].DefaultCellStyle.Alignment =
                            DataGridViewContentAlignment.MiddleCenter;
                        DataGridSubjects.Columns["units"].DefaultCellStyle.Alignment =
                            DataGridViewContentAlignment.MiddleCenter;
                        DataGridSubjects.Columns["courseCode"].DefaultCellStyle.Alignment =
                            DataGridViewContentAlignment.MiddleCenter;
                        DataGridSubjects.Columns["semester"].DefaultCellStyle.Alignment =
                            DataGridViewContentAlignment.MiddleCenter;
                        DataGridSubjects.Columns["year_level"].DefaultCellStyle.Alignment =
                            DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading subjects: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //sorting
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

        private void ApplyFilters(object sender, EventArgs e)
        {
            string yearLevelFilter = CmbYrLvl.SelectedItem.ToString();
            string semesterFilter = CmbSem.SelectedItem.ToString();

            string filterExpression = "";

            if (currentProgramFilter != "All")
                filterExpression += $"[courseCode] = '{currentProgramFilter}'";

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

            if (DataGridSubjects.DataSource is DataTable)
            {
                DataTable dt = (DataTable)DataGridSubjects.DataSource;
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

        private void DataGridSubjects_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = DataGridSubjects.Rows[e.RowIndex];
            currentMode = CurrentMode.Subjects;

            if (e.ColumnIndex != DataGridSubjects.Columns["ColOpen1"].Index &&
                e.ColumnIndex != DataGridSubjects.Columns["ColClose1"].Index)
            {
                PopulateFields(row);
            }

            if (e.ColumnIndex == DataGridSubjects.Columns["ColOpen1"].Index)
            {
                PopulateFields(row);
            }

            if (e.ColumnIndex == DataGridSubjects.Columns["ColClose1"].Index)
            {
                if (MessageBox.Show("Are you sure you want to delete this subject?", "Confirm Delete",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int subjectId = Convert.ToInt32(row.Cells["subject_id"].Value);
                    DeleteSubject(subjectId);
                }
            }
        }

        //add, update

        private void ClearFields()
        {
            TxtSubID.Clear();
            TxtSubCode.Clear();
            TxtSubName.Clear();
            CmbCourse.SelectedIndex = -1;
            CmbYearLevel.SelectedIndex = -1;
            CmbSemester.SelectedIndex = -1;
        }

        private void LoadCourseComboBox()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT course_id, course_code FROM courses";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            CmbCourse.Items.Clear();
                            while (reader.Read())
                            {
                                CmbCourse.Items.Add(new KeyValuePair<int, string>(
                                    reader.GetInt32("course_id"),
                                    reader.GetString("course_code")));
                            }
                            CmbCourse.DisplayMember = "Value";
                            CmbCourse.ValueMember = "Key";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading courses: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (currentMode == CurrentMode.Subjects)
            {
                AddSubject();
            }
            else
            {
                //AddPrerequisite();
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (currentMode == CurrentMode.Subjects)
            {
                UpdateSubject();
            }
            else
            {
               // UpdatePrerequisite();
            }
        }

        private void BtnDrop_Click(object sender, EventArgs e)
        {
            if (currentMode == CurrentMode.Subjects)
            {
                DeleteSelectedSubject();
            }
            else
            {
               // DeleteSelectedPrerequisite();
            }
        }

        private void AddSubject()
        {
            if (ValidateSubjectFields())
            {
                try
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        // First, insert the subject
                        string subjectQuery = @"INSERT INTO subjects (subject_code, subject_name, units) 
                                       VALUES (@code, @name, @units);
                                       SELECT LAST_INSERT_ID();";

                        using (var cmd = new MySqlCommand(subjectQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@code", TxtSubCode.Text.Trim());
                            cmd.Parameters.AddWithValue("@name", TxtSubName.Text.Trim());
                            cmd.Parameters.AddWithValue("@units", 3); // Default units or add a field for this

                            int subjectId = Convert.ToInt32(cmd.ExecuteScalar());

                            // Then link it to the course
                            if (CmbCourse.SelectedItem != null)
                            {
                                int courseId = ((KeyValuePair<int, string>)CmbCourse.SelectedItem).Key;

                                string linkQuery = @"INSERT INTO course_subjects 
                                           (course_id, subject_id, year_level, semester)
                                           VALUES (@courseId, @subjectId, @year, @semester)";

                                using (var linkCmd = new MySqlCommand(linkQuery, conn))
                                {
                                    linkCmd.Parameters.AddWithValue("@courseId", courseId);
                                    linkCmd.Parameters.AddWithValue("@subjectId", subjectId);
                                    linkCmd.Parameters.AddWithValue("@year", CmbYearLevel.Text);
                                    linkCmd.Parameters.AddWithValue("@semester", CmbSemester.Text);
                                    linkCmd.ExecuteNonQuery();
                                }
                            }

                            MessageBox.Show("Subject added successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                            LoadSubjectsCourse();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding subject: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateSubject()
        {
            if (string.IsNullOrEmpty(TxtSubID.Text))
            {
                MessageBox.Show("Please select a subject to update.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ValidateSubjectFields() && !string.IsNullOrEmpty(TxtSubID.Text))
            {
                try
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        // Update subject
                        string subjectQuery = @"UPDATE subjects 
                                      SET subject_code = @code, 
                                          subject_name = @name
                                      WHERE subject_id = @id";

                        using (var cmd = new MySqlCommand(subjectQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@code", TxtSubCode.Text.Trim());
                            cmd.Parameters.AddWithValue("@name", TxtSubName.Text.Trim());
                            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(TxtSubID.Text));
                            cmd.ExecuteNonQuery();
                        }

                        // Update course link if a course is selected
                        if (CmbCourse.SelectedItem != null)
                        {
                            int courseId = ((KeyValuePair<int, string>)CmbCourse.SelectedItem).Key;

                            // First check if link exists
                            string checkLinkQuery = "SELECT COUNT(*) FROM course_subjects WHERE subject_id = @subjectId";
                            bool linkExists = false;

                            using (var checkCmd = new MySqlCommand(checkLinkQuery, conn))
                            {
                                checkCmd.Parameters.AddWithValue("@subjectId", Convert.ToInt32(TxtSubID.Text));
                                linkExists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;
                            }

                            if (linkExists)
                            {
                                // Update existing link
                                string updateLinkQuery = @"UPDATE course_subjects 
                                                SET course_id = @courseId,
                                                    year_level = @year,
                                                    semester = @semester
                                                WHERE subject_id = @subjectId";

                                using (var updateCmd = new MySqlCommand(updateLinkQuery, conn))
                                {
                                    updateCmd.Parameters.AddWithValue("@courseId", courseId);
                                    updateCmd.Parameters.AddWithValue("@year", CmbYearLevel.Text);
                                    updateCmd.Parameters.AddWithValue("@semester", CmbSemester.Text);
                                    updateCmd.Parameters.AddWithValue("@subjectId", Convert.ToInt32(TxtSubID.Text));
                                    updateCmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // Create new link
                                string insertLinkQuery = @"INSERT INTO course_subjects 
                                                (course_id, subject_id, year_level, semester)
                                                VALUES (@courseId, @subjectId, @year, @semester)";

                                using (var insertCmd = new MySqlCommand(insertLinkQuery, conn))
                                {
                                    insertCmd.Parameters.AddWithValue("@courseId", courseId);
                                    insertCmd.Parameters.AddWithValue("@subjectId", Convert.ToInt32(TxtSubID.Text));
                                    insertCmd.Parameters.AddWithValue("@year", CmbYearLevel.Text);
                                    insertCmd.Parameters.AddWithValue("@semester", CmbSemester.Text);
                                    insertCmd.ExecuteNonQuery();
                                }
                            }
                        }

                        MessageBox.Show("Subject updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields();
                        LoadSubjectsCourse();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating subject: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteSelectedSubject()
        {
            if (!string.IsNullOrEmpty(TxtSubID.Text))
            {
                if (MessageBox.Show("Are you sure you want to delete this subject?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DeleteSubject(Convert.ToInt32(TxtSubID.Text));
                }
            }
        }

        private void DeleteSubject(int subjectId)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // First delete from course_subjects (foreign key constraint)
                    string deleteLinkQuery = "DELETE FROM course_subjects WHERE subject_id = @id";
                    using (var linkCmd = new MySqlCommand(deleteLinkQuery, conn))
                    {
                        linkCmd.Parameters.AddWithValue("@id", subjectId);
                        linkCmd.ExecuteNonQuery();
                    }

                    // Then delete from subjects
                    string deleteQuery = "DELETE FROM subjects WHERE subject_id = @id";
                    using (var cmd = new MySqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", subjectId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Subject deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadSubjectsCourse();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting subject: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateSubjectFields()
        {
            if (string.IsNullOrWhiteSpace(TxtSubCode.Text))
            {
                MessageBox.Show("Please enter subject code.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtSubCode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtSubName.Text))
            {
                MessageBox.Show("Please enter subject name.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtSubName.Focus();
                return false;
            }

            if (CmbCourse.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a course.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CmbCourse.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(CmbYearLevel.Text))
            {
                MessageBox.Show("Please select year level.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CmbYearLevel.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(CmbSemester.Text))
            {
                MessageBox.Show("Please select semester.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CmbSemester.Focus();
                return false;
            }

            return true;
        }

        private void PopulateFields(DataGridViewRow row)
        {
            try
            {
                TxtSubID.Text = row.Cells["subject_id"].Value?.ToString() ?? "";
                TxtSubCode.Text = row.Cells["subject_code"].Value?.ToString() ?? "";
                TxtSubName.Text = row.Cells["subject_name"].Value?.ToString() ?? "";

                string courseCode = DataGridSubjects.Columns.Contains("course_code")
            ? row.Cells["course_code"].Value?.ToString() ?? "N/A"
            : "N/A";

                if (courseCode != "N/A")
                {
                    foreach (KeyValuePair<int, string> item in CmbCourse.Items)
                    {
                        if (item.Value == courseCode)
                        {
                            CmbCourse.SelectedItem = item;
                            break;
                        }
                    }
                }

                CmbYearLevel.Text = row.Cells["year_level"].Value?.ToString() ?? "";
                CmbSemester.Text = row.Cells["semester"].Value?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading details: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
