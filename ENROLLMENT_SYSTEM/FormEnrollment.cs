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

            DataGridEnrollment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridPayment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
                col.Frozen = false; 
                col.Resizable = DataGridViewTriState.True; 
            }

            /////////////////////////////////////////
            DataGridPayment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridPayment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
            DataGridSubjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridSubjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridSubjects.RowTemplate.Height = 40;
            DataGridSubjects.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            DataGridSubjects.RowTemplate.Height = 40;
            DataGridSubjects.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            DataGridSubjects.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridSubjects.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

    
            foreach (DataGridViewColumn col in DataGridSubjects.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
        }

        private void FormEnrollment_Activated(object sender, EventArgs e)
        {
            LoadEnrollmentData();
        }


        private void FormEnrollment_Load(object sender, EventArgs e)
        {
            LoadEnrollmentData();
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
            using (FormNewAcademiccs formNewAcademiccs = new FormNewAcademiccs())
            {
                formNewAcademiccs.StartPosition = FormStartPosition.CenterParent;


                DialogResult result = formNewAcademiccs.ShowDialog();


                if (result == DialogResult.OK)
                {
                    LoadEnrollmentData();
                }
            }
        }

        private void LoadEnrollmentData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=localhost;database=PDM_Enrollment_DB;user=root;password=;"))
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

                            while (reader.Read())
                            {
                                DataGridEnrollment.Rows.Add(
                                    reader["enrollment_id"].ToString(),
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
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading enrollment data: " + ex.Message);
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

                string studentNo = selectedRow.Cells[1].Value?.ToString() ?? "";
                string lastName = selectedRow.Cells[2].Value?.ToString() ?? "";
                string firstName = selectedRow.Cells[3].Value?.ToString() ?? "";
                string middleName = selectedRow.Cells[4].Value?.ToString() ?? "";
                string courseCode = selectedRow.Cells[5].Value?.ToString() ?? "";
                string academicYear = selectedRow.Cells[6].Value?.ToString() ?? "";
                string semester = selectedRow.Cells[7].Value?.ToString() ?? "";
                
                string yearLevel = selectedRow.Cells[8].Value?.ToString() ?? "";

                UpdateStudentInfoPanel(studentNo, $"{firstName} {middleName} {lastName}", courseCode, academicYear, semester, yearLevel);

                int courseId = GetCourseId(courseCode);
                if (courseId > 0)
                {
                    LoadSubjectsForEnrollment(courseId, yearLevel, semester);

                }
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
            string firstName = selectedRow.Cells[3].Value?.ToString() ?? "";
            string lastName = selectedRow.Cells[2].Value?.ToString() ?? "";
            string studentName = $"{lastName}, {firstName}";

            
            if (DataGridEnrollment.Columns[e.ColumnIndex].Name == "ColOpen")
            {
               
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
                using (MySqlConnection conn = new MySqlConnection("server=localhost;database=PDM_Enrollment_DB;user=root;password=;"))
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

        private void UpdateStudentInfoPanel(string studentNo, string fullName, string courseCode, string academicYear, string semester, string yearLevel)
        {
            LblStudentNo.Text = studentNo;
            LblName.Text = fullName;
            LblCourse.Text = courseCode;
            LblAcademicYear.Text = academicYear;
            LblSemester.Text = semester;
            LblYearLevel.Text = yearLevel;
        }

        private bool DeleteEnrollment(string enrollmentId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=localhost;database=PDM_Enrollment_DB;user=root;password=;"))
                {
                    conn.Open();

                    // SQL query to delete the enrollment based on the enrollment_id
                    string query = "DELETE FROM student_enrollments WHERE enrollment_id = @EnrollmentId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Use the provided enrollmentId parameter to execute the query
                        cmd.Parameters.AddWithValue("@EnrollmentId", enrollmentId);

                        int rowsAffected = cmd.ExecuteNonQuery(); // Execute the query
                        return rowsAffected > 0; // Return true if rows were deleted
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
            DataGridEnrollment.RowTemplate.Height = 35;
            DataGridEnrollment.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrollment.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrollment.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrollment.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrollment.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrollment.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
            DataGridSubjects.Columns[1].Width = 200;
            DataGridSubjects.Columns[2].Width = 800;
            DataGridSubjects.Columns[3].Width = 100;
            DataGridSubjects.Columns[4].Width = 100;
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
            if (DataGridEnrollment.Columns[e.ColumnIndex].Name == "ColPay")
            {
                new FormPayment().Show();
               
             }
         }

        private void button1_Click(object sender, EventArgs e)
        {
            new FormPayment().Show();
        }

        private void LoadSubjectsForEnrollment(int courseId, string yearLevel, string semester)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=localhost;database=PDM_Enrollment_DB;user=root;password=;"))
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

                        // Clear existing columns if they exist
                        DataGridSubjects.Columns.Clear();

                        // Add columns to DataGridSubjects
                        DataGridSubjects.Columns.Add("course_subject_id", "ID");
                        DataGridSubjects.Columns.Add("subject_code", "Subject Code");
                        DataGridSubjects.Columns.Add("subject_name", "Subject Name");
                        DataGridSubjects.Columns.Add("units", "Units");
                        DataGridSubjects.Columns.Add("course_code", "Course Code");
                        DataGridSubjects.Columns.Add("semester1", "Semester");
                        DataGridSubjects.Columns.Add("year_level1", "Year Level");

                        DataGridSubjects.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        DataGridSubjects.Columns[0].Width = 50;
                        DataGridSubjects.Columns[1].Width = 200;
                        DataGridSubjects.Columns[2].Width = 800;
                        DataGridSubjects.Columns[3].Width = 100;
                        DataGridSubjects.Columns[4].Width = 100;
                        DataGridSubjects.RowTemplate.Height = 35;
                        DataGridSubjects.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        DataGridSubjects.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        DataGridSubjects.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        DataGridSubjects.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        DataGridSubjects.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        // Populate the DataGrid
                        foreach (DataRow row in dt.Rows)
                        {
                            DataGridSubjects.Rows.Add(
                                row["course_subject_id"],
                                row["subject_code"],
                                row["subject_name"],
                                row["units"],
                                row["course_code"],
                                row["semester1"],
                                row["year_level1"]
                            );
                        }

                        // Apply styling to the newly added data
                        StyleDataGridSubjects();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading subjects: " + ex.Message);
            }
        }

        private void DataGridSubjects_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}