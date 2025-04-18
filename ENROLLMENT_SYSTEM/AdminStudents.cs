﻿using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Enrollment_System
{
    public partial class AdminStudents : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
       
        private string currentProgramFilter = "All";
        private Button[] programButtons;
        private XStringFormat yPos;

        public AdminStudents()
        {
            InitializeComponent();
            InitializeProgramButtons();
            InitializeDataGridView();
            LoadStudentData();
            InitializeFilterControls();

            ProgramButton_Click(BtnAll, EventArgs.Empty);
        }

        private void InitializeDataGridView()
        {
            DataGridEnrolled.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridEnrolled.Columns["ColOpen"].Width = 50;
            DataGridEnrolled.Columns["ColClose"].Width = 50;
            DataGridEnrolled.Columns["ColOpen"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridEnrolled.Columns["ColClose"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridEnrolled.RowTemplate.Height = 40;
            DataGridEnrolled.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;


            DataGridEnrolled.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridEnrolled.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewImageColumn colOpen = (DataGridViewImageColumn)DataGridEnrolled.Columns["ColOpen"];
            colOpen.ImageLayout = DataGridViewImageCellLayout.Zoom;

            DataGridViewImageColumn colClose = (DataGridViewImageColumn)DataGridEnrolled.Columns["ColClose"];
            colClose.ImageLayout = DataGridViewImageCellLayout.Zoom;

            foreach (DataGridViewColumn col in DataGridEnrolled.Columns)
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

        private void AdminStudents_Load(object sender, EventArgs e)
        {
            DataGridEnrolled.CellClick += DataGridNewEnrollment_CellClick;
            StyleTwoTabControl();
            InitializeDataGridView(); 
            
            InitializeFilterControls();
            DataGridEnrolled.AutoGenerateColumns = false;
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

            DataGridEnrolled.AllowUserToResizeColumns = false;
            DataGridEnrolled.AllowUserToResizeRows = false;
            DataGridEnrolled.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn column in DataGridEnrolled.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            int totalCols = DataGridEnrolled.Columns.Count;
            DataGridEnrolled.Columns[totalCols - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridEnrolled.Columns[totalCols - 1].Width = 40;
            DataGridEnrolled.Columns[totalCols - 2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridEnrolled.Columns[totalCols - 2].Width = 40;
            DataGridEnrolled.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridEnrolled.Columns[0].Width = 50;
            DataGridEnrolled.RowTemplate.Height = 35;
            DataGridEnrolled.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrolled.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrolled.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrolled.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrolled.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridEnrolled.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            foreach (DataGridViewColumn col in DataGridEnrolled.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
            ////////////////////////////////////////


            CustomizeDataGridEnrolled();

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

        private void CustomizeDataGridEnrolled()
        {

            DataGridEnrolled.BorderStyle = BorderStyle.None;


            DataGridEnrolled.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);


            DataGridEnrolled.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridEnrolled.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);


            DataGridEnrolled.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridEnrolled.DefaultCellStyle.SelectionForeColor = Color.White;


            DataGridEnrolled.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridEnrolled.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridEnrolled.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridEnrolled.EnableHeadersVisualStyles = false;


            DataGridEnrolled.GridColor = Color.BurlyWood;


            DataGridEnrolled.DefaultCellStyle.Font = new Font("Segoe UI", 10);


            DataGridEnrolled.RowTemplate.Height = 35;


            DataGridEnrolled.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            foreach (DataGridViewColumn column in DataGridEnrolled.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }
        }


        private void DataGridEnrolled_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataGridNewEnrollment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

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
                        WHERE se.status = 'Enrolled'";

                  

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataGridEnrolled.AutoGenerateColumns = false;

                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataGridEnrolled.DataSource = dt;
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

            if (DataGridEnrolled.DataSource is DataTable)
            {
                DataTable dt = (DataTable)DataGridEnrolled.DataSource;
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
            DataGridEnrolled.BorderStyle = BorderStyle.None;

            DataGridEnrolled.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridEnrolled.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridEnrolled.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridEnrolled.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridEnrolled.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridEnrolled.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridEnrolled.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridEnrolled.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridEnrolled.EnableHeadersVisualStyles = false;

            DataGridEnrolled.GridColor = Color.BurlyWood;

            DataGridEnrolled.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridEnrolled.RowTemplate.Height = 35;

            DataGridEnrolled.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn column in DataGridEnrolled.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }

        }

        private void DataGridNewEnrollment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // ensure a valid row was clicked
            {
                DataGridViewRow row = DataGridEnrolled.Rows[e.RowIndex];

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

        private void BtnReport_Click(object sender, EventArgs e)
        {
            try
            {
                
                PdfDocument pdfDoc = new PdfDocument();
                string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "Enrollment_Report_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");

                PdfPage page = pdfDoc.AddPage();
                page.Orientation = PageOrientation.Landscape;
                XGraphics gfx = XGraphics.FromPdfPage(page);

              
                XFont largeFont = new XFont("Verdana", 16, XFontStyle.Bold);
                XFont font = new XFont("Verdana", 8, XFontStyle.Regular);
                XFont boldFont = new XFont("Verdana", 9, XFontStyle.Bold);
                XFont headerFont = new XFont("Verdana", 12, XFontStyle.BoldItalic);

                double marginLeft = 20;
                double marginTop = 20;
                double pageWidth = page.Width;
                double pageHeight = page.Height;

                
                double yPos = marginTop + 100; 

               
                using (MemoryStream ms = new MemoryStream())
                {
                    Properties.Resources.BANNERPDM.Save(ms, System.Drawing.Imaging.ImageFormat.Png); 
                    ms.Position = 0; 
                    var bannerImage = XImage.FromStream(ms); 
                    
                    gfx.DrawImage(bannerImage, marginLeft, marginTop, pageWidth - 2 * marginLeft, 100); 
                    yPos = marginTop + 120; 
                }

                
                string title = "CERTIFICATE OF REGISTRATION";
                double titleWidth = gfx.MeasureString(title, largeFont).Width;
                double titleXPos = (pageWidth - titleWidth) / 2;

                gfx.DrawString(title, largeFont, XBrushes.Black, titleXPos, yPos); 
                yPos += 40; 

               
                gfx.DrawString("Enrollment Report", headerFont, XBrushes.Black, marginLeft, yPos);
                gfx.DrawString("Generated on: " + DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss"), font, XBrushes.Black, marginLeft, yPos + 25);

                yPos += 60;

                if (DataGridEnrolled.SelectedRows.Count > 0)
                {
                   
                    DataGridViewRow selectedRow = DataGridEnrolled.SelectedRows[0];
                    string studentNo = selectedRow.Cells["student_no"].Value.ToString();
                    string lastName = selectedRow.Cells["last_name"].Value.ToString();
                    string firstName = selectedRow.Cells["first_name"].Value.ToString();
                    string middleName = selectedRow.Cells["middle_name"].Value.ToString();
                    string program = selectedRow.Cells["CourseCode"].Value.ToString();
                    string yearLevel = selectedRow.Cells["year_level"].Value.ToString();
                    string semester = selectedRow.Cells["semester"].Value.ToString();
                    string status = selectedRow.Cells["status"].Value.ToString();

                    
                    gfx.DrawString("Selected Student: " + firstName + " " + middleName + " " + lastName, boldFont, XBrushes.Black, marginLeft, yPos);
                    yPos += 20;
                    gfx.DrawString("Student No: " + studentNo, font, XBrushes.Black, marginLeft, yPos);
                    yPos += 20;
                    gfx.DrawString("Course: " + program, font, XBrushes.Black, marginLeft, yPos);
                    yPos += 20;
                    gfx.DrawString("Year Level: " + yearLevel, font, XBrushes.Black, marginLeft, yPos);
                    yPos += 20;
                    gfx.DrawString("Semester: " + semester, font, XBrushes.Black, marginLeft, yPos);
                    yPos += 20;
                    gfx.DrawString("Status: " + status, font, XBrushes.Black, marginLeft, yPos);
                    yPos += 30;

                    gfx.DrawLine(XPens.Black, marginLeft, yPos, pageWidth - marginLeft, yPos); 
                    yPos += 10; 

                  
                    List<Subject> subjects = GetSubjectsForStudent(program, semester, yearLevel);

                   
                    double[] columnWidths = new double[7]; 
                    double totalWidth = pageWidth - 2 * marginLeft;
                    double columnWidth = totalWidth / 7;

                    columnWidths[0] = 50; // ID
                    columnWidths[1] = 80; // Subject Code
                    columnWidths[2] = 300; // Subject Name
                    columnWidths[3] = 50; // Units
                    columnWidths[4] = 80; // Course Code
                    columnWidths[5] = 80; // Semester
                    columnWidths[6] = 80; // Year Level

                   
                    double xPos = marginLeft;
                    string[] headers = { "ID", "Subject Code", "Subject Name", "Units", "Course Code", "Semester", "Year Level" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        gfx.DrawString(headers[i], boldFont, XBrushes.Black, xPos, yPos);
                        xPos += columnWidths[i];
                    }
                    yPos += 20; 

                    
                    foreach (var subject in subjects)
                    {
                        xPos = marginLeft;
                        gfx.DrawString(subject.SubjectID.ToString(), font, XBrushes.Black, xPos, yPos);
                        xPos += columnWidths[0];

                        gfx.DrawString(subject.SubjectCode, font, XBrushes.Black, xPos, yPos);
                        xPos += columnWidths[1];

                        gfx.DrawString(subject.SubjectName, font, XBrushes.Black, xPos, yPos);
                        xPos += columnWidths[2];

                        gfx.DrawString(subject.Units.ToString(), font, XBrushes.Black, xPos, yPos);
                        xPos += columnWidths[3];

                        gfx.DrawString(subject.CourseCode, font, XBrushes.Black, xPos, yPos);
                        xPos += columnWidths[4];

                        gfx.DrawString(subject.Semester, font, XBrushes.Black, xPos, yPos);
                        xPos += columnWidths[5];

                        gfx.DrawString(subject.YearLevel, font, XBrushes.Black, xPos, yPos);
                        yPos += 15; 
                    }

                    gfx.DrawLine(XPens.Black, marginLeft, yPos, pageWidth - marginLeft, yPos); 
                    yPos += 10; 
                }

                
                pdfDoc.Save(savePath);
                MessageBox.Show("PDF report generated successfully and saved to: " + savePath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<Subject> GetSubjectsForStudent(string courseCode, string semester, string yearLevel)
        {
            List<Subject> subjects = new List<Subject>();

            
            using (MySqlConnection conn = new MySqlConnection("server=localhost;database=PDM_Enrollment_DB;user=root;password=;"))
            {
                conn.Open();

                
                string query = @"
            SELECT s.subject_id, s.subject_code, s.subject_name, s.units, 
                   c.course_code, cs.semester, cs.year_level
            FROM course_subjects cs
            JOIN subjects s ON cs.subject_id = s.subject_id
            JOIN courses c ON cs.course_id = c.course_id
            WHERE c.course_code = @courseCode
              AND cs.semester = @semester
              AND cs.year_level = @yearLevel";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@courseCode", courseCode);
                    cmd.Parameters.AddWithValue("@semester", semester);
                    cmd.Parameters.AddWithValue("@yearLevel", yearLevel);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjects.Add(new Subject
                            {
                                SubjectID = reader.GetInt32(0),
                                SubjectCode = reader.GetString(1),
                                SubjectName = reader.GetString(2),
                                Units = reader.GetInt32(3),
                                CourseCode = reader.GetString(4), 
                                Semester = reader.GetString(5),
                                YearLevel = reader.GetString(6)
                            });
                        }
                    }
                }
            }

            return subjects;
        }

       
        public class Subject
        {
            public int SubjectID { get; set; }
            public string SubjectCode { get; set; }
            public string SubjectName { get; set; }
            public int Units { get; set; }
            public string CourseCode { get; set; }
            public string Semester { get; set; }
            public string YearLevel { get; set; }
            public string bannerImage { get; set; }
        }
    }
}
