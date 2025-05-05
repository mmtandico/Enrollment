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
        private readonly string connectionString = DatabaseConfig.ConnectionString;

        private string currentProgramFilter = "All";
        private Button[] programButtons;
        private bool isUpdatingRowNumbers = false;


        private enum CurrentMode { Subjects, Prerequisites }
        private CurrentMode currentMode = CurrentMode.Subjects;
        //private int currentPrerequisiteId = -1;

        public AdminCourse()
        {
            InitializeComponent();

            DataGridSubjects.DataBindingComplete += DataGridSubjects_DataBindingComplete;
            DataGridPrerequisite.DataBindingComplete += DataGridPrerequisite_DataBindingComplete;
            DataGridSubjects.Sorted += DataGridSubjects_Sorted;
            DataGridPrerequisite.Sorted += DataGridPrerequisite_Sorted;

            //InitializeDataGridView();
            //BtnAdd.Click += BtnAdd_Click;
            //BtnUpdate.Click += BtnUpdate_Click;
            //BtnDrop.Click += BtnDrop_Click;
            StyleTwoTabControl();
            InitializeFilterControls();
            InitializeProgramButtons();
            
            LoadSubjectsCourse();

            ProgramButton_Click(BtnAll, EventArgs.Empty);
        }
        
        private void AdminCourse_Load(object sender, EventArgs e)
        {
         
            StyleTwoTabControl();
            //InitializeDataGridView();
            InitializeFilterControls();
            InitializeProgramButtons();
            

            LoadSubjectsCourse();
            LoadPrerequisiteSubjects();
            LoadCourseComboBox();

            DataGridSubjects.CellContentClick += DataGridSubjects_CellContentClick;
            DataGridPrerequisite.CellContentClick += DataGridPrerequisite_CellContentClick;
            DataGridPrerequisite.CellClick += DataGridPrerequisite_CellClick;

            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;

            

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
            DataGridSubjects.Columns[1].Width = 50;
            DataGridSubjects.Columns[2].Width = 150;
            DataGridSubjects.Columns[3].Width = 500;
            DataGridSubjects.Columns[4].Width = 50;
            DataGridSubjects.Columns[5].Width = 100;
            DataGridSubjects.Columns[6].Width = 100;
            DataGridSubjects.Columns[7].Width = 100;
            DataGridSubjects.RowTemplate.Height = 35;
            DataGridSubjects.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridSubjects.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridSubjects.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


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
            DataGridPrerequisite.Columns[2].Width = 150;
            DataGridPrerequisite.Columns[3].Width = 350;
            DataGridPrerequisite.Columns[4].Width = 50;
            DataGridPrerequisite.Columns[5].Width = 50;
            DataGridPrerequisite.Columns[6].Width = 150;
            DataGridPrerequisite.Columns[7].Width = 300;
            DataGridPrerequisite.Columns[8].Width = 50;
            DataGridPrerequisite.RowTemplate.Height = 35;
            DataGridPrerequisite.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridPrerequisite.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridPrerequisite.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridPrerequisite.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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

            bool isTabPage3 = tabControl1.SelectedIndex == 1;

            CmbYrLvl.Enabled = !isTabPage3;
            CmbSem.Enabled = !isTabPage3;

            BtnBSCS.Enabled = !isTabPage3;
            BtnBSIT.Enabled = !isTabPage3;
            BtnBSTM.Enabled = !isTabPage3;
            BtnBSHM.Enabled = !isTabPage3;
            BtnBSOAD.Enabled = !isTabPage3;
            BtnBECED.Enabled = !isTabPage3;
            BtnBTLED.Enabled = !isTabPage3;
            BtnAll.Enabled = !isTabPage3;
        }

        private void InitializeDataGridView(DataGridView dataGrid)
        {
            if (dataGrid.Columns.Count == 0) return;

            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid.RowTemplate.Height = 40;
            dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Configure image columns if they exist
            if (dataGrid.Columns.Contains("ColOpen") || dataGrid.Columns.Contains("ColOpen1"))
            {
                string colName = dataGrid.Columns.Contains("ColOpen") ? "ColOpen" : "ColOpen1";
                dataGrid.Columns[colName].Width = 50;
                dataGrid.Columns[colName].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                ((DataGridViewImageColumn)dataGrid.Columns[colName]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            }

            if (dataGrid.Columns.Contains("ColClose") || dataGrid.Columns.Contains("ColClose1"))
            {
                string colName = dataGrid.Columns.Contains("ColClose") ? "ColClose" : "ColClose1";
                dataGrid.Columns[colName].Width = 50;
                dataGrid.Columns[colName].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                ((DataGridViewImageColumn)dataGrid.Columns[colName]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            }

            // Set alignment for other columns
            foreach (DataGridViewColumn col in dataGrid.Columns)
            {
                if (col is DataGridViewImageColumn) continue;

                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;

                if (col.Name.EndsWith("_name") || col.Name.EndsWith("_name_pre"))
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (!col.Name.StartsWith("Col"))
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
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
                        DataGridSubjects.AutoGenerateColumns = false;
                        DataGridSubjects.Columns.Clear();

                        DataGridSubjects.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "RowNumber",
                            Name = "no_subjects",
                            HeaderText = "No.",
                            Width = 50,
                            ReadOnly = true,
                            SortMode = DataGridViewColumnSortMode.NotSortable
                        });

                        DataGridSubjects.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "subject_id",
                            HeaderText = "ID",
                            Name = "subject_id",
                            Visible = false
                        });

                       
      
                        DataGridSubjects.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "subject_code",
                            HeaderText = "Subject Code",
                            Name = "subject_code",
                            Width = 120
                        });

                        DataGridSubjects.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "subject_name",
                            HeaderText = "Subject Name",
                            Name = "subject_name",
                            Width = 200,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        });

                        DataGridSubjects.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "units",
                            HeaderText = "Units",
                            Name = "units",
                            Width = 60,
                            DefaultCellStyle = new DataGridViewCellStyle()
                            {
                                Alignment = DataGridViewContentAlignment.MiddleCenter
                            }
                        });

                        DataGridSubjects.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "courseCode",
                            HeaderText = "Course",
                            Name = "courseCode",
                            Width = 100
                        });

                        DataGridSubjects.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "semester",
                            HeaderText = "Semester",
                            Name = "semester",
                            Width = 80,
                            DefaultCellStyle = new DataGridViewCellStyle()
                            {
                                Alignment = DataGridViewContentAlignment.MiddleCenter
                            }
                        });

                        DataGridSubjects.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "year_level",
                            HeaderText = "Year Level",
                            Name = "year_level",
                            Width = 80,
                            DefaultCellStyle = new DataGridViewCellStyle()
                            {
                                Alignment = DataGridViewContentAlignment.MiddleCenter
                            }
                        });

                        // Action buttons
                        DataGridViewImageColumn openCol = new DataGridViewImageColumn()
                        {
                            Name = "ColOpen1",
                            HeaderText = "",
                            Image = Properties.Resources.EditButton,
                            Width = 40,
                            ImageLayout = DataGridViewImageCellLayout.Zoom
                        };
                        DataGridSubjects.Columns.Add(openCol);

                        DataGridViewImageColumn closeCol = new DataGridViewImageColumn()
                        {
                            Name = "ColClose1",
                            HeaderText = "",
                            Image = Properties.Resources.RemoveButton,
                            Width = 40,
                            ImageLayout = DataGridViewImageCellLayout.Zoom
                        };
                        DataGridSubjects.Columns.Add(closeCol);
                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Add row numbers to the DataTable before binding
                            dt.Columns.Add("RowNumber", typeof(int));
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["RowNumber"] = i + 1;
                            }

                            DataGridSubjects.DataSource = dt;
                        }

                        // InitializeDataGridView(DataGridSubjects);
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

        private void UpdateRowNumbers(DataGridView dataGrid)
        {
            if (isUpdatingRowNumbers || dataGrid == null || dataGrid.Rows == null) return;

            isUpdatingRowNumbers = true;

            try
            {
                string columnName = dataGrid == DataGridSubjects ? "no_subjects" : "no_pre";
                int columnIndex = dataGrid.Columns[columnName]?.Index ?? -1;

                if (columnIndex < 0) return;

                for (int i = 0; i < dataGrid.Rows.Count; i++)
                {
                    if (dataGrid.Rows[i].IsNewRow) continue;
                    dataGrid.Rows[i].Cells[columnIndex].Value = (i + 1).ToString();
                }
            }
            finally
            {
                isUpdatingRowNumbers = false;
            }
        }

        private void DataGrid_Sorted(object sender, EventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid != null)
            {
                UpdateRowNumbers(grid);
            }
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
                UpdateRowNumbers(DataGridSubjects);
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
               UpdatePrerequisite();
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
               DeleteSelectedPrerequisite();
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

                // Handle course code selection
                string courseCode = row.Cells["courseCode"].Value?.ToString() ?? "N/A";

                if (courseCode != "N/A")
                {
                    // Find the matching item in the ComboBox
                    foreach (var item in CmbCourse.Items)
                    {
                        var kvp = (KeyValuePair<int, string>)item;
                        if (kvp.Value == courseCode)
                        {
                            CmbCourse.SelectedItem = item;
                            break;
                        }
                    }
                }
                else
                {
                    CmbCourse.SelectedIndex = -1;
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

        private void LoadPrerequisiteSubjects()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT 
                    s1.subject_id AS subject_id_pre,
                    s1.subject_code AS subject_code_pre,
                    s1.subject_name AS subject_name_pre,
                    s1.units AS units_pre,
                    p.subject_id AS prerequisite_id,
                    p.subject_code AS prerequisite_code_pre,
                    p.subject_name AS prerequisite_name_pre,
                    p.units AS prerequisite_units
                FROM 
                    subjects s1
                JOIN 
                    subject_prerequisites sp ON s1.subject_id = sp.subject_id
                JOIN 
                    subjects p ON sp.prerequisite_id = p.subject_id
                ORDER BY 
                    s1.subject_code, p.subject_code";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        DataGridPrerequisite.AutoGenerateColumns = false;
                        DataGridPrerequisite.Columns.Clear();

                        DataGridPrerequisite.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "RowNumber",
                            Name = "no_pre",
                            HeaderText = "No.",
                            Width = 50,
                            ReadOnly = true,
                            SortMode = DataGridViewColumnSortMode.NotSortable
                        });
                        DataGridPrerequisite.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "subject_id_pre",
                            HeaderText = "Subject ID",
                            Name = "subject_id_pre",
                            Visible = false
                        });

                        DataGridPrerequisite.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "subject_code_pre",
                            HeaderText = "Subject Code",
                            Name = "subject_code_pre",
                            Width = 120
                        });

                        DataGridPrerequisite.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "subject_name_pre",
                            HeaderText = "Subject Name",
                            Name = "subject_name_pre",
                            Width = 200,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        });

                        DataGridPrerequisite.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "units_pre",
                            HeaderText = "Units",
                            Name = "units_pre",
                            Width = 60,
                            DefaultCellStyle = new DataGridViewCellStyle()
                            {
                                Alignment = DataGridViewContentAlignment.MiddleCenter
                            }
                        });

                        DataGridPrerequisite.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "prerequisite_id",
                            HeaderText = "Prereq ID",
                            Name = "prerequisite_id",
                            Visible = false
                        });

                        DataGridPrerequisite.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "prerequisite_code_pre",
                            HeaderText = "Prerequisite Code",
                            Name = "prerequisite_code_pre",
                            Width = 120
                        });

                        DataGridPrerequisite.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "prerequisite_name_pre",
                            HeaderText = "Prerequisite Name",
                            Name = "prerequisite_name_pre",
                            Width = 200
                        });

                        DataGridPrerequisite.Columns.Add(new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = "prerequisite_units",
                            HeaderText = "P. Units",
                            Name = "prerequisite_units",
                            Width = 60
                        });

                        // Action buttons
                        DataGridViewImageColumn openCol = new DataGridViewImageColumn()
                        {
                            Name = "ColOpen",
                            HeaderText = "",
                            Image = Properties.Resources.EditButton,
                            Width = 40,
                            ImageLayout = DataGridViewImageCellLayout.Zoom
                        };
                        DataGridPrerequisite.Columns.Add(openCol);

                        DataGridViewImageColumn closeCol = new DataGridViewImageColumn()
                        {
                            Name = "ColClose",
                            HeaderText = "",
                            Image = Properties.Resources.RemoveButton,
                            Width = 40,
                            ImageLayout = DataGridViewImageCellLayout.Zoom
                        };
                        DataGridPrerequisite.Columns.Add(closeCol);

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Add row numbers
                            dt.Columns.Add("RowNumber", typeof(int));
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["RowNumber"] = i + 1;
                            }

                            DataGridPrerequisite.DataSource = dt;
                        }

                        //InitializeDataGridView(DataGridPrerequisite);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading prerequisite subjects: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeletePrerequisite(int subjectId, int prerequisiteId)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM subject_prerequisites WHERE subject_id = @subjectId AND prerequisite_id = @prerequisiteId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@subjectId", subjectId);
                        cmd.Parameters.AddWithValue("@prerequisiteId", prerequisiteId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Prerequisite deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPrerequisiteSubjects();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting prerequisite: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridPrerequisite_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = DataGridPrerequisite.Rows[e.RowIndex];

            // Get the IDs from the hidden columns
            int subjectId = Convert.ToInt32(row.Cells["subject_id_pre"].Value);
            int prerequisiteId = Convert.ToInt32(row.Cells["prerequisite_id"].Value);

            if (e.ColumnIndex == DataGridPrerequisite.Columns["ColOpen"].Index)
            {
                // Update action
                UpdatePrerequisite();
            }
            else if (e.ColumnIndex == DataGridPrerequisite.Columns["ColClose"].Index)
            {
                // Delete action
                if (MessageBox.Show("Are you sure you want to delete this prerequisite?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DeletePrerequisite(subjectId, prerequisiteId);
                }
            }
        }

        private void DeleteSelectedPrerequisite()
        {
            if (DataGridPrerequisite.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a prerequisite relationship to delete", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = DataGridPrerequisite.SelectedRows[0];
            int subjectId = Convert.ToInt32(selectedRow.Cells["subject_id_pre"].Value);
            int prerequisiteId = Convert.ToInt32(selectedRow.Cells["prerequisite_id"].Value);

            if (MessageBox.Show("Are you sure you want to delete this prerequisite relationship?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM subject_prerequisites WHERE subject_id = @subjectId AND prerequisite_id = @prerequisiteId";

                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@subjectId", subjectId);
                            cmd.Parameters.AddWithValue("@prerequisiteId", prerequisiteId);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Prerequisite relationship deleted successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadPrerequisiteSubjects();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting prerequisite: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdatePrerequisite()
        {
            if (DataGridPrerequisite.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a prerequisite relationship to update", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = DataGridPrerequisite.SelectedRows[0];
            int subjectId = Convert.ToInt32(selectedRow.Cells["subject_id_pre"].Value);
            int prerequisiteId = Convert.ToInt32(selectedRow.Cells["prerequisite_id"].Value);

            // Get the new prerequisite ID from your existing controls
            if (string.IsNullOrWhiteSpace(TxtSubID.Text))
            {
                MessageBox.Show("Please enter a valid subject ID for the new prerequisite", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int newPrerequisiteId;
            if (!int.TryParse(TxtSubID.Text, out newPrerequisiteId))
            {
                MessageBox.Show("Please enter a valid numeric subject ID", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPrerequisiteId == subjectId)
            {
                MessageBox.Show("A subject cannot be a prerequisite for itself", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Verify the new prerequisite exists
                    string checkQuery = "SELECT COUNT(*) FROM subjects WHERE subject_id = @prerequisiteId";
                    using (var checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@prerequisiteId", newPrerequisiteId);
                        int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (exists == 0)
                        {
                            MessageBox.Show("The specified prerequisite subject does not exist", "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Check if this relationship already exists
                    string existsQuery = "SELECT COUNT(*) FROM subject_prerequisites WHERE subject_id = @subjectId AND prerequisite_id = @newPrerequisiteId";
                    using (var existsCmd = new MySqlCommand(existsQuery, conn))
                    {
                        existsCmd.Parameters.AddWithValue("@subjectId", subjectId);
                        existsCmd.Parameters.AddWithValue("@newPrerequisiteId", newPrerequisiteId);
                        int relationshipExists = Convert.ToInt32(existsCmd.ExecuteScalar());
                        if (relationshipExists > 0)
                        {
                            MessageBox.Show("This prerequisite relationship already exists", "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // First delete the old relationship
                    string deleteQuery = "DELETE FROM subject_prerequisites WHERE subject_id = @subjectId AND prerequisite_id = @prerequisiteId";
                    using (var cmd = new MySqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@subjectId", subjectId);
                        cmd.Parameters.AddWithValue("@prerequisiteId", prerequisiteId);
                        cmd.ExecuteNonQuery();
                    }

                    // Then add the new relationship
                    string insertQuery = "INSERT INTO subject_prerequisites (subject_id, prerequisite_id) VALUES (@subjectId, @newPrerequisiteId)";
                    using (var cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@subjectId", subjectId);
                        cmd.Parameters.AddWithValue("@newPrerequisiteId", newPrerequisiteId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Prerequisite updated successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPrerequisiteSubjects();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating prerequisite: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridPrerequisite_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; 

            DataGridViewRow row = DataGridPrerequisite.Rows[e.RowIndex];

            int prerequisiteId = Convert.ToInt32(row.Cells["prerequisite_id"].Value);

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT s.subject_id, s.subject_code, s.subject_name, 
                       c.course_code, cs.semester, cs.year_level
                FROM subjects s
                LEFT JOIN course_subjects cs ON s.subject_id = cs.subject_id
                LEFT JOIN courses c ON cs.course_id = c.course_id
                WHERE s.subject_id = @prerequisiteId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@prerequisiteId", prerequisiteId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                TxtSubID.Text = reader["subject_id"].ToString();
                                TxtSubCode.Text = reader["subject_code"].ToString();
                                TxtSubName.Text = reader["subject_name"].ToString();

                                string courseCode = reader["course_code"].ToString();
                                foreach (KeyValuePair<int, string> item in CmbCourse.Items)
                                {
                                    if (item.Value == courseCode)
                                    {
                                        CmbCourse.SelectedItem = item;
                                        break;
                                    }
                                }

                                CmbSemester.Text = reader["semester"].ToString();
                                CmbYearLevel.Text = reader["year_level"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading prerequisite details: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = textBox1.Text.ToLower();

            if (DataGridSubjects.DataSource is DataTable)
            {
                DataTable dt = (DataTable)DataGridSubjects.DataSource;
                dt.DefaultView.RowFilter = string.Format("Subject_Code LIKE '%{0}%' OR Subject_Name LIKE '%{0}%'", searchTerm);
            }

            if (DataGridPrerequisite.DataSource is DataTable)
            {
                DataTable dt = (DataTable)DataGridPrerequisite.DataSource;
                dt.DefaultView.RowFilter = string.Format("subject_code_pre LIKE '%{0}%' OR subject_name_pre LIKE '%{0}%' OR prerequisite_code_pre LIKE '%{0}%' OR prerequisite_name_pre LIKE '%{0}%'", searchTerm);
            }
        }

        private void DataGridSubjects_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            UpdateRowNumbers(DataGridSubjects);
        }

        private void DataGridPrerequisite_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            UpdateRowNumbers(DataGridPrerequisite);
        }

        private void DataGridSubjects_Sorted(object sender, EventArgs e)
        {
            UpdateRowNumbers(DataGridSubjects);
        }

        private void DataGridPrerequisite_Sorted(object sender, EventArgs e)
        {
            UpdateRowNumbers(DataGridPrerequisite);
        }
    }
}
