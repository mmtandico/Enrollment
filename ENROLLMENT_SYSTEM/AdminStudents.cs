using System;
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
using System.Diagnostics;

namespace Enrollment_System
{
    public partial class AdminStudents : Form
    {
        private readonly string connectionString = DatabaseConfig.ConnectionString;
        private string currentProgramFilter = "All";
        private Button[] programButtons;
        private XStringFormat yPos;
        public bool IsViewMode { get; set; } = false;


        public AdminStudents()
        {
            InitializeComponent();
            InitializeProgramButtons();
            InitializeDataGridView();
            LoadStudentData();
            InitializeFilterControls();


            DataGridEnrolled.Sorted += DataGridEnrolled_Sorted;

            ProgramButton_Click(BtnAll, EventArgs.Empty);
        }

        private void InitializeDataGridView()
        {
            foreach (DataGridViewColumn col in DataGridEnrolled.Columns)
            {
                col.Frozen = false;
            }

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

        private void AdminStudents_Load(object sender, EventArgs e)
        {
            DataGridEnrolled.CellClick += DataGridEnrolled_CellContentClick;
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
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    Debug.WriteLine("Database connection successful");

                    // Check if fee_settings has data
                    var cmd = new MySqlCommand("SELECT COUNT(*) FROM fee_settings", conn);
                    var count = cmd.ExecuteScalar();
                    Debug.WriteLine($"Fee settings records: {count}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database connection failed: {ex}");
            }

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
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DataGridEnrolled.Rows[e.RowIndex];
                string studentNo = row.Cells["student_no"].Value.ToString();
                FetchStudentDetails(studentNo);
            }

            if (e.ColumnIndex == DataGridEnrolled.Columns["ColClose"].Index && e.RowIndex >= 0)
            {
                try
                {
                    if (DataGridEnrolled.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("No row selected for the report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    DataGridViewRow selectedRow = DataGridEnrolled.SelectedRows[0];
                    GenerateStudentReportPDF(selectedRow);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error generating PDF: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (e.ColumnIndex == DataGridEnrolled.Columns["ColOpen"].Index && e.RowIndex >= 0)
            {
                if (DataGridEnrolled.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a student first.", "No Selection",
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = DataGridEnrolled.SelectedRows[0];
                int enrollmentId = Convert.ToInt32(selectedRow.Cells["student_id"].Value);
                string studentName = $"{selectedRow.Cells["first_name"].Value} {selectedRow.Cells["last_name"].Value}";

                try
                {
                    StudentHistory historyForm = new StudentHistory();
                    historyForm.EnrollmentId = enrollmentId;
                    historyForm.StudentName = studentName;
                    historyForm.LoadAcademicHistory();
                    historyForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading academic history: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void GenerateStudentReportPDF(DataGridViewRow selectedRow)
        {
            try
            {

                // Create PDF document
                PdfDocument pdfDoc = new PdfDocument();
                string savePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "Downloads",
                    $"Enrollment_COR_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

                PdfPage page = pdfDoc.AddPage();
                page.Orientation = PageOrientation.Landscape;
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Font definitions
                XFont titleFont = new XFont("Verdana", 18, XFontStyle.Bold);
                XFont regularFont = new XFont("Verdana", 9, XFontStyle.Regular);
                XFont boldFont = new XFont("Verdana", 10, XFontStyle.Bold);
                XFont headerFont = new XFont("Verdana", 12, XFontStyle.Bold);
                XFont subHeaderFont = new XFont("Verdana", 11, XFontStyle.BoldItalic);

                // Layout measurements
                double marginLeft = 30;
                double marginTop = 30;
                double pageWidth = page.Width;
                double pageHeight = page.Height;
                double contentWidth = pageWidth - (2 * marginLeft);
                double yPos = marginTop;

                // 1. Draw header banner
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        var bannerImage = Properties.Resources.BANNERPDM;
                        bannerImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        var xImage = XImage.FromStream(ms);
                        gfx.DrawImage(xImage, marginLeft, yPos, contentWidth, 80);
                        yPos += 100; // Banner height + spacing
                    }
                }
                catch
                {
                    // If banner fails, just continue without it
                    yPos += 20;
                }

                // 2. Title and date
                string title = "CERTIFICATE OF REGISTRATION";
                double titleWidth = gfx.MeasureString(title, titleFont).Width;
                gfx.DrawString(title, titleFont, XBrushes.Black,
                              new XPoint((pageWidth - titleWidth) / 2, yPos));
                yPos += 30;

                string dateGenerated = "Generated on: " + DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss");
                double dateWidth = gfx.MeasureString(dateGenerated, regularFont).Width;
                gfx.DrawString(dateGenerated, regularFont, XBrushes.Black,
                              new XPoint(pageWidth - marginLeft - dateWidth, yPos));
                yPos += 30;

                // 3. Student information section

                string studentNo = selectedRow.Cells["student_no"].Value?.ToString() ?? "N/A";
                string lastName = selectedRow.Cells["last_name"].Value?.ToString() ?? "N/A";
                string firstName = selectedRow.Cells["first_name"].Value?.ToString() ?? "N/A";
                string middleName = selectedRow.Cells["middle_name"].Value?.ToString() ?? "N/A";
                string program = selectedRow.Cells["courseCode"].Value?.ToString() ?? "N/A";
                string yearLevel = selectedRow.Cells["year_level"].Value?.ToString() ?? "N/A";
                string semester = selectedRow.Cells["semester"].Value?.ToString() ?? "N/A";
                string status = selectedRow.Cells["status"].Value?.ToString() ?? "N/A";

                // Two-column layout for student info
                double infoColWidth = contentWidth / 2;
                double startYPos = yPos;

                // Left column
                gfx.DrawString("Student Name:", boldFont, XBrushes.Black, marginLeft, yPos);
                gfx.DrawString($"{lastName}, {firstName} {middleName}", regularFont, XBrushes.Black, marginLeft + 100, yPos);
                yPos += 20;

                gfx.DrawString("Student No:", boldFont, XBrushes.Black, marginLeft, yPos);
                gfx.DrawString(studentNo, regularFont, XBrushes.Black, marginLeft + 100, yPos);
                yPos += 20;

                gfx.DrawString("Program:", boldFont, XBrushes.Black, marginLeft, yPos);
                gfx.DrawString(program, regularFont, XBrushes.Black, marginLeft + 100, yPos);

                // Right column
                yPos = startYPos;
                double rightColX = marginLeft + infoColWidth;

                gfx.DrawString("Year Level:", boldFont, XBrushes.Black, rightColX, yPos);
                gfx.DrawString(yearLevel, regularFont, XBrushes.Black, rightColX + 100, yPos);
                yPos += 20;

                gfx.DrawString("Semester:", boldFont, XBrushes.Black, rightColX, yPos);
                gfx.DrawString(semester, regularFont, XBrushes.Black, rightColX + 100, yPos);
                yPos += 20;

                gfx.DrawString("Status:", boldFont, XBrushes.Black, rightColX, yPos);
                gfx.DrawString(status, regularFont, XBrushes.Black, rightColX + 100, yPos);

                yPos = startYPos + 60; // Adjust for next section

                // 4. Enrolled Subjects
                gfx.DrawLine(new XPen(XColors.Black, 1), marginLeft, yPos, pageWidth - marginLeft, yPos);
                yPos += 20;

                string subjectsTitle = "ENROLLED SUBJECTS";
                double subjectsTitleWidth = gfx.MeasureString(subjectsTitle, headerFont).Width;
                gfx.DrawString(subjectsTitle, headerFont, XBrushes.Black,
                              new XPoint((pageWidth - subjectsTitleWidth) / 2, yPos));
                yPos += 25;

                // Get subjects from database
                List<Subject> subjects = GetSubjectsForStudent(program, semester, yearLevel);

                // Subjects table
                double[] columnWidths = { 40, 80, 350, 60, 80, 80, 80 }; // Column widths
                string[] headers = { "No.", "Code", "Subject Name", "Units", "Program", "Sem", "Yr Level" };
                double rowHeight = 20;
                double cellPadding = 5;
                double tableStartY = yPos;

                // Draw table header
                XRect headerRect = new XRect(marginLeft, tableStartY, contentWidth, rowHeight);
                gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(230, 230, 230)), headerRect);

                // Draw column headers
                double xPos = marginLeft;
                for (int i = 0; i < headers.Length; i++)
                {
                    double textWidth = gfx.MeasureString(headers[i], boldFont).Width;
                    double xOffset = (i >= 3) ? (columnWidths[i] - textWidth) / 2 : cellPadding; // Center align some columns

                    gfx.DrawString(headers[i], boldFont, XBrushes.Black,
                                  new XPoint(xPos + xOffset, tableStartY + rowHeight - cellPadding));
                    xPos += columnWidths[i];
                }

                // Draw rows
                double currentY = tableStartY + rowHeight;
                for (int i = 0; i < subjects.Count; i++)
                {
                    var subject = subjects[i];

                    // Alternate row background
                    if (i % 2 == 1)
                    {
                        gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(245, 245, 245)),
                                        new XRect(marginLeft, currentY, contentWidth, rowHeight));
                    }

                    // Draw cell content
                    xPos = marginLeft;

                    // Row number
                    gfx.DrawString((i + 1).ToString(), regularFont, XBrushes.Black,
                                  new XPoint(xPos + cellPadding, currentY + rowHeight - cellPadding));
                    xPos += columnWidths[0];

                    // Subject Code
                    gfx.DrawString(subject.SubjectCode, regularFont, XBrushes.Black,
                                  new XPoint(xPos + cellPadding, currentY + rowHeight - cellPadding));
                    xPos += columnWidths[1];

                    // Subject Name (with ellipsis if too long)
                    string subjectName = subject.SubjectName;
                    XSize nameSize = gfx.MeasureString(subjectName, regularFont);
                    double maxWidth = columnWidths[2] - (2 * cellPadding);

                    if (nameSize.Width > maxWidth)
                    {
                        while (nameSize.Width > maxWidth && subjectName.Length > 3)
                        {
                            subjectName = subjectName.Substring(0, subjectName.Length - 4) + "...";
                            nameSize = gfx.MeasureString(subjectName, regularFont);
                        }
                    }
                    gfx.DrawString(subjectName, regularFont, XBrushes.Black,
                                  new XPoint(xPos + cellPadding, currentY + rowHeight - cellPadding));
                    xPos += columnWidths[2];

                    // Units (center aligned)
                    string units = subject.Units.ToString();
                    double unitsWidth = gfx.MeasureString(units, regularFont).Width;
                    gfx.DrawString(units, regularFont, XBrushes.Black,
                                  new XPoint(xPos + (columnWidths[3] - unitsWidth) / 2, currentY + rowHeight - cellPadding));
                    xPos += columnWidths[3];

                    // Program (center aligned)
                    double programWidth = gfx.MeasureString(program, regularFont).Width;
                    gfx.DrawString(program, regularFont, XBrushes.Black,
                                  new XPoint(xPos + (columnWidths[4] - programWidth) / 2, currentY + rowHeight - cellPadding));
                    xPos += columnWidths[4];

                    // Semester (center aligned)
                    double semWidth = gfx.MeasureString(semester, regularFont).Width;
                    gfx.DrawString(semester, regularFont, XBrushes.Black,
                                  new XPoint(xPos + (columnWidths[5] - semWidth) / 2, currentY + rowHeight - cellPadding));
                    xPos += columnWidths[5];

                    // Year Level (center aligned)
                    double yrLevelWidth = gfx.MeasureString(yearLevel, regularFont).Width;
                    gfx.DrawString(yearLevel, regularFont, XBrushes.Black,
                                  new XPoint(xPos + (columnWidths[6] - yrLevelWidth) / 2, currentY + rowHeight - cellPadding));

                    currentY += rowHeight;
                }

                yPos = currentY + 20;

                // 5. Fee Calculation from existing payment data
                try
                {
                    int enrollmentId = Convert.ToInt32(selectedRow.Cells["student_id"].Value);

                    // Add this line:
                    CheckDatabaseState(enrollmentId);

                    var payment = GetExistingPaymentBreakdown(enrollmentId);

                    // Fee box
                    double feeBoxWidth = 250;
                    double feeBoxStartX = pageWidth - marginLeft - feeBoxWidth;
                    double feeBoxStartY = yPos;

                    gfx.DrawString("ASSESSMENT SUMMARY", headerFont, XBrushes.Black,
                                  new XPoint(feeBoxStartX + (feeBoxWidth - gfx.MeasureString("ASSESSMENT SUMMARY", headerFont).Width) / 2, feeBoxStartY));
                    feeBoxStartY += 20;

                    // Box border
                    gfx.DrawRectangle(new XPen(XColors.Black, 1),
                                     new XRect(feeBoxStartX, feeBoxStartY, feeBoxWidth, 110));

                    // Fee details
                    feeBoxStartY += 20;
                    gfx.DrawString("Tuition Fee:", boldFont, XBrushes.Black, feeBoxStartX + 10, feeBoxStartY);
                    gfx.DrawString($"₱{payment.TuitionFee:0.00}", regularFont, XBrushes.Black, feeBoxStartX + 150, feeBoxStartY);
                    feeBoxStartY += 20;

                    gfx.DrawString("Miscellaneous Fee:", boldFont, XBrushes.Black, feeBoxStartX + 10, feeBoxStartY);
                    gfx.DrawString($"₱{payment.MiscellaneousFee:0.00}", regularFont, XBrushes.Black, feeBoxStartX + 150, feeBoxStartY);
                    feeBoxStartY += 20;

                    // Total line
                    gfx.DrawLine(new XPen(XColors.Black, 0.5), feeBoxStartX + 10, feeBoxStartY - 5, feeBoxStartX + feeBoxWidth - 10, feeBoxStartY - 5);

                    gfx.DrawString("TOTAL ASSESSMENT:", boldFont, XBrushes.Black, feeBoxStartX + 10, feeBoxStartY);
                    gfx.DrawString($"₱{payment.TotalAmountDue:0.00}", boldFont, XBrushes.Black, feeBoxStartX + 150, feeBoxStartY);
                }
                catch (Exception feeEx)
                {
                    Debug.WriteLine($"Fee calculation error: {feeEx}");
                    gfx.DrawString("Fee Information Not Available", boldFont, XBrushes.Red, marginLeft, yPos);
                    yPos += 20;
                    gfx.DrawString($"Error: {feeEx.Message}", regularFont, XBrushes.Red, marginLeft, yPos);
                    yPos += 40;
                }

                // 6. Footer and signatures
                yPos = pageHeight - 60;

                // Student signature line
                gfx.DrawLine(new XPen(XColors.Black, 1), marginLeft + 100, yPos, marginLeft + 300, yPos);
                gfx.DrawString("Student Signature", regularFont, XBrushes.Black,
                              new XPoint(marginLeft + 200 - gfx.MeasureString("Student Signature", regularFont).Width / 2, yPos + 15));

                // Registrar signature line
                gfx.DrawLine(new XPen(XColors.Black, 1), pageWidth - marginLeft - 300, yPos, pageWidth - marginLeft - 100, yPos);
                gfx.DrawString("Registrar", regularFont, XBrushes.Black,
                              new XPoint(pageWidth - marginLeft - 200 - gfx.MeasureString("Registrar", regularFont).Width / 2, yPos + 15));

                // Footer text
                yPos += 40;
                string footerText = "This document is computer-generated and does not require a signature stamp to be considered valid.";
                gfx.DrawString(footerText, regularFont, XBrushes.Gray,
                              new XPoint((pageWidth - gfx.MeasureString(footerText, regularFont).Width) / 2, yPos));

                // Save PDF
                pdfDoc.Save(savePath);
                MessageBox.Show($"PDF report generated successfully:\n{savePath}",
                               "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating PDF: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                        se.enrollment_id as student_id,
                        s.student_no,
                        s.last_name,
                        s.first_name,
                        s.middle_name,
                        c.course_code AS Program,
                        se.academic_year,
                        se.semester,
                        se.year_level,
                        se.status,
                        COALESCE(p.total_amount_due, 0) as total_amount_due,
                        COALESCE(SUM(CASE WHEN pb.fee_type = 'Tuition' THEN pb.amount ELSE 0 END), 0) AS tuition_fee,
                        COALESCE(SUM(CASE WHEN pb.fee_type = 'Miscellaneous' THEN pb.amount ELSE 0 END), 0) AS misc_fee
                    FROM student_enrollments se
                    INNER JOIN students s ON se.student_id = s.student_id
                    INNER JOIN courses c ON se.course_id = c.course_id
                    LEFT JOIN payments p ON se.enrollment_id = p.enrollment_id
                    LEFT JOIN payment_breakdowns pb ON p.payment_id = pb.payment_id
                    WHERE se.status IN ('Enrolled', 'Dropped', 'Completed')
                    GROUP BY se.enrollment_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataGridEnrolled.AutoGenerateColumns = false;
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataGridEnrolled.DataSource = dt;
                            UpdateRowNumbers();
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

        private void UpdateRowNumbers()
        {
            if (DataGridEnrolled.Rows.Count == 0) return;

            int noColumnIndex = DataGridEnrolled.Columns[1].Index;

            for (int i = 0; i < DataGridEnrolled.Rows.Count; i++)
            {
                if (DataGridEnrolled.Rows[i].IsNewRow) continue;
                DataGridEnrolled.Rows[i].Cells[noColumnIndex].Value = (i + 1).ToString();
            }
        }

        private void ApplyFilters(object sender, EventArgs e)
        {
            string yearLevelFilter = CmbYrLvl.SelectedItem.ToString();
            string semesterFilter = CmbSem.SelectedItem.ToString();
            string schoolyearFilter = CmbSchoolYear.SelectedItem.ToString();

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

            if (DataGridEnrolled.DataSource is DataTable)
            {
                DataTable dt = (DataTable)DataGridEnrolled.DataSource;
                dt.DefaultView.RowFilter = filterExpression;
                UpdateRowNumbers();
            }
        }

        private void DataGridEnrolled_Sorted(object sender, EventArgs e)
        {
            UpdateRowNumbers();
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
                // Create a new PDF document
                PdfDocument pdfDoc = new PdfDocument();
                string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "Enrollment_Report_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");

                PdfPage page = pdfDoc.AddPage();
                page.Orientation = PageOrientation.Landscape;
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Define fonts
                XFont titleFont = new XFont("Verdana", 14, XFontStyle.Bold);
                XFont headerFont = new XFont("Verdana", 9, XFontStyle.Bold);
                XFont contentFont = new XFont("Verdana", 8);

                // Title
                string title = "Student Enrollment Report";
                double titleWidth = gfx.MeasureString(title, titleFont).Width;
                double pageWidth = page.Width;
                gfx.DrawString(title, titleFont, XBrushes.Black, new XPoint((pageWidth - titleWidth) / 2, 40));

                // Draw a line under the title
                double titleYPosition = 60;
                gfx.DrawLine(XPens.Black, 40, titleYPosition + 10, pageWidth - 40, titleYPosition + 10);

                // Column Names for the table
                string[] headers = { "NO.", "Student No.", "Last Name", "First Name", "Middle Name", "Course", "School Year", "Semester", "Year Level", "Status" };

                // Calculate column widths
                double[] columnWidths = new double[headers.Length];
                double marginLeft = 40;
                double availableWidth = pageWidth - 2 * marginLeft;

                // Measure the width of each column's content
                double totalContentWidth = 0;
                for (int i = 0; i < headers.Length; i++)
                {
                    // Measure the header width
                    double maxContentWidth = gfx.MeasureString(headers[i], headerFont).Width;

                    foreach (DataGridViewRow row in DataGridEnrolled.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string cellValue = "";
                        if (i == 0) // NO. column - sequential numbers
                        {
                            cellValue = (row.Index + 1).ToString();
                        }
                        else if (i == 1) // Student No. column - PDM ID
                        {
                            cellValue = row.Cells["student_no"].Value?.ToString() ?? "";
                        }
                        else // Other columns
                        {
                            // Adjust column index mapping
                            int dataGridColIndex = i; // Default mapping
                            if (i >= 2) dataGridColIndex = i + 1; // Skip the original student_no column
                            cellValue = row.Cells[dataGridColIndex]?.Value?.ToString() ?? "";
                        }

                        double cellWidth = gfx.MeasureString(cellValue, contentFont).Width;
                        maxContentWidth = Math.Max(maxContentWidth, cellWidth);
                    }

                    columnWidths[i] = maxContentWidth + 10; // Added padding
                    totalContentWidth += columnWidths[i];
                }

                // Scale the column widths to fit the page
                double scaleFactor = availableWidth / totalContentWidth;
                for (int i = 0; i < columnWidths.Length; i++)
                {
                    columnWidths[i] *= scaleFactor;
                }

                // Adjusted starting Y position for header
                double headerYPosition = 80;
                double currentX = marginLeft;

                // Draw Header Row
                for (int i = 0; i < headers.Length; i++)
                {
                    gfx.DrawString(headers[i], headerFont, XBrushes.Black, new XPoint(currentX + 5, headerYPosition + 3));
                    currentX += columnWidths[i];
                }

                // Draw vertical grid lines for columns
                currentX = marginLeft;
                for (int i = 0; i <= headers.Length; i++)
                {
                    // Draw vertical lines from header to the end of the table
                    double lineStartY = headerYPosition - 5;
                    double lineEndY = headerYPosition + 25 + (DataGridEnrolled.Rows.Count - 1) * 20;

                    gfx.DrawLine(XPens.Gray, currentX, lineStartY, currentX, lineEndY);

                    if (i < headers.Length)
                        currentX += columnWidths[i];
                }

                // Draw a line under the header row
                double headerLineY = headerYPosition + 15;
                gfx.DrawLine(new XPen(XColors.Black, 1), marginLeft, headerLineY, pageWidth - marginLeft, headerLineY);

                // Adjusted starting Y position for row content
                int yPosition = (int)(headerYPosition + 25);

                // Loop through all rows in the DataGridView
                foreach (DataGridViewRow row in DataGridEnrolled.Rows)
                {
                    if (row.IsNewRow) continue;

                    // Draw horizontal grid line above each row (except the first one)
                    if (yPosition > headerYPosition + 25)
                    {
                        gfx.DrawLine(XPens.LightGray, marginLeft, yPosition - 10, pageWidth - marginLeft, yPosition - 10);
                    }

                    currentX = marginLeft;

                    // NO. column - sequential numbers
                    gfx.DrawString((row.Index + 1).ToString(), contentFont, XBrushes.Black, new XPoint(currentX + 5, yPosition));
                    currentX += columnWidths[0];

                    // Student No. column - PDM ID
                    string studentNo = row.Cells["student_no"].Value?.ToString() ?? "";
                    gfx.DrawString(studentNo, contentFont, XBrushes.Black, new XPoint(currentX + 5, yPosition));
                    currentX += columnWidths[1];

                    // Remaining columns
                    for (int i = 2; i < headers.Length; i++)
                    {
                        // Adjust column index mapping
                        int dataGridColIndex = i + 1; // Skip student_no column
                        string cellValue = row.Cells[dataGridColIndex]?.Value?.ToString() ?? "N/A";
                        gfx.DrawString(cellValue, contentFont, XBrushes.Black, new XPoint(currentX + 5, yPosition));
                        currentX += columnWidths[i];
                    }

                    yPosition += 20;
                }

                // Draw a line at the bottom of the table
                gfx.DrawLine(new XPen(XColors.Black, 1), marginLeft, yPosition - 10, pageWidth - marginLeft, yPosition - 10);

                // Save the PDF
                pdfDoc.Save(savePath);
                MessageBox.Show("Report generated successfully!\nSaved to: " + savePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating report: " + ex.Message);
            }
        }

        private decimal GetCurrentTuitionPerUnit()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT tuition_per_unit 
                   FROM fee_settings 
                   WHERE effective_date <= @currentDate
                   ORDER BY effective_date DESC 
                   LIMIT 1";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@currentDate", DateTime.Today);
                        object result = cmd.ExecuteScalar();

                        return result != null && result != DBNull.Value ?
                               Convert.ToDecimal(result) : 1000.00m; // Default value
                    }
                }
            }
            catch
            {
                return 1000.00m; // Default value if error occurs
            }
        }

        private decimal GetCurrentMiscellaneousFee()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT miscellaneous_fee 
                           FROM fee_settings 
                           WHERE effective_date <= @currentDate
                           ORDER BY effective_date DESC 
                           LIMIT 1";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@currentDate", DateTime.Today);
                        object result = cmd.ExecuteScalar();

                        if (result == null || result == DBNull.Value)
                        {
                            throw new InvalidOperationException("No miscellaneous fee found in database");
                        }

                        return Convert.ToDecimal(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting misc fee: {ex.Message}");
                throw; // Re-throw to let caller handle
            }
        }

        private decimal CalculateTuitionFee(int totalUnits)
        {
            if (totalUnits <= 0)
                throw new ArgumentException("Total units must be positive", nameof(totalUnits));

            return totalUnits * GetCurrentTuitionPerUnit();
        }

        private decimal CalculateMiscellaneousFee()
        {
            return GetCurrentMiscellaneousFee();
        }


        private int GetTotalUnits(string courseCode, string semester, string yearLevel)
        {
            try
            {
                Debug.WriteLine($"Getting units for {courseCode}, {semester}, {yearLevel}");

                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                SELECT SUM(s.units) 
                FROM course_subjects cs
                JOIN subjects s ON cs.subject_id = s.subject_id
                JOIN courses c ON cs.course_id = c.course_id
                WHERE c.course_code = @courseCode
                AND cs.semester = @semester
                AND cs.year_level = @yearLevel";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@courseCode", courseCode);
                        cmd.Parameters.AddWithValue("@semester", semester);
                        cmd.Parameters.AddWithValue("@yearLevel", yearLevel);

                        object result = cmd.ExecuteScalar();

                        if (result == null || result == DBNull.Value)
                        {
                            Debug.WriteLine("No units found - returning 0");
                            return 0;
                        }

                        int units = Convert.ToInt32(result);
                        Debug.WriteLine($"Found {units} units");
                        return units;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetTotalUnits: {ex}");
                return 0;
            }
        }

        private List<Subject> GetSubjectsForStudent(string courseCode, string semester, string yearLevel)
        {
            List<Subject> subjects = new List<Subject>();


            using (MySqlConnection conn = new MySqlConnection(connectionString))
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




        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = textBox1.Text.ToLower();


            if (DataGridEnrolled.DataSource is DataTable)
            {
                DataTable dt = (DataTable)DataGridEnrolled.DataSource;
                dt.DefaultView.RowFilter = string.Format("last_name LIKE '%{0}%' OR first_name LIKE '%{0}%' OR student_no LIKE '%{0}%'", searchTerm);
            }

        }

        private void BtnInfos_Click(object sender, EventArgs e)
        {
            if (DataGridEnrolled.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a student first.", "No Selection",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = DataGridEnrolled.SelectedRows[0];
            string studentNo = selectedRow.Cells["student_no"].Value?.ToString();

            if (string.IsNullOrEmpty(studentNo))
            {
                MessageBox.Show("Invalid student selected.", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Pass the student number to the constructor
                using (FormPersonalInfo formPersonalInfo = new FormPersonalInfo(studentNo))
                {
                    formPersonalInfo.IsViewMode = true;
                    formPersonalInfo.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening student information: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDrop_Click(object sender, EventArgs e)
        {
            if (DataGridEnrolled.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a student first.", "No Selection",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = DataGridEnrolled.SelectedRows[0];
            int enrollmentId = Convert.ToInt32(selectedRow.Cells["student_id"].Value);
            string studentName = $"{selectedRow.Cells["first_name"].Value} {selectedRow.Cells["last_name"].Value}";

            DialogResult result = MessageBox.Show($"Are you sure you want to drop {studentName}?",
                                                 "Confirm Drop",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        string updateQuery = "UPDATE student_enrollments SET status = 'Dropped' WHERE enrollment_id = @enrollmentId";

                        using (MySqlCommand cmd = new MySqlCommand(updateQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@enrollmentId", enrollmentId);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show($"{studentName} has been successfully dropped.",
                                              "Success",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Information);

                                LoadStudentData();
                            }
                            else
                            {
                                MessageBox.Show("No records were updated.",
                                              "Error",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating enrollment status: {ex.Message}",
                                   "Database Error",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCompleted_Click(object sender, EventArgs e)
        {
            if (DataGridEnrolled.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a student first.", "No Selection",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = DataGridEnrolled.SelectedRows[0];
            int enrollmentId = Convert.ToInt32(selectedRow.Cells["student_id"].Value);
            string studentName = $"{selectedRow.Cells["first_name"].Value} {selectedRow.Cells["last_name"].Value}";
            string currentStatus = selectedRow.Cells["status"].Value?.ToString() ?? "";
            string schoolYear = selectedRow.Cells["academic_year"].Value?.ToString() ?? "2023-2024";
            string semester = selectedRow.Cells["semester"].Value?.ToString() ?? "1st";
            string yearLevel = selectedRow.Cells["year_level"].Value?.ToString() ?? "1st Year";

            // Format the confirmation message
            string confirmationMessage = $"Mark {studentName} as completed for {yearLevel}, {semester} Semester {schoolYear}?";

            if (currentStatus == "Completed")
            {
                MessageBox.Show($"{studentName} has already completed {yearLevel}, {semester} Semester {schoolYear}.",
                              "Already Completed",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(confirmationMessage,
                                                "Confirm Completion",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        string updateQuery = @"UPDATE student_enrollments 
                                    SET status = 'Completed' 
                                    WHERE enrollment_id = @enrollmentId";

                        using (MySqlCommand cmd = new MySqlCommand(updateQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@enrollmentId", enrollmentId);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show($"Successfully marked {studentName} as completed for:\n{yearLevel}, {semester} Semester {schoolYear}",
                                              "Completion Recorded",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Information);

                                LoadStudentData();
                            }
                            else
                            {
                                MessageBox.Show("No records were updated.",
                                              "Error",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating enrollment status:\n{ex.Message}",
                                  "Database Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
        }

        private void BtnAcademicHistory_Click(object sender, EventArgs e)
        {
            if (DataGridEnrolled.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a student first.", "No Selection",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = DataGridEnrolled.SelectedRows[0];
            int enrollmentId = Convert.ToInt32(selectedRow.Cells["student_id"].Value);
            string studentName = $"{selectedRow.Cells["first_name"].Value} {selectedRow.Cells["last_name"].Value}";

            try
            {
                // Create and show the StudentHistory form
                StudentHistory historyForm = new StudentHistory();

                // Pass the enrollment ID to the form (you'll need to modify StudentHistory to accept this)
                historyForm.EnrollmentId = enrollmentId;
                historyForm.StudentName = studentName;

                // Load the academic history
                historyForm.LoadAcademicHistory();

                // Show the form
                historyForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading academic history: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private PaymentBreakdown GetExistingPaymentBreakdown(int enrollmentId)
        {
            // 1. First try to get existing payment
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string paymentQuery = @"
                SELECT 
                    p.total_amount_due,
                    SUM(CASE WHEN pb.fee_type = 'Tuition' THEN pb.amount ELSE 0 END) AS tuition,
                    SUM(CASE WHEN pb.fee_type = 'Miscellaneous' THEN pb.amount ELSE 0 END) AS misc
                FROM payments p
                LEFT JOIN payment_breakdowns pb ON p.payment_id = pb.payment_id
                WHERE p.enrollment_id = @enrollmentId
                GROUP BY p.payment_id
                ORDER BY p.payment_date DESC
                LIMIT 1";

                    using (var cmd = new MySqlCommand(paymentQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@enrollmentId", enrollmentId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new PaymentBreakdown
                                {
                                    TotalAmountDue = reader.GetDecimal(0),
                                    TuitionFee = reader.GetDecimal(1),
                                    MiscellaneousFee = reader.GetDecimal(2)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking for existing payment: {ex}");
            }

            // 2. If no payment exists, calculate new one
            return CalculateNewPaymentBreakdown(enrollmentId);
        }

        private PaymentBreakdown CalculateNewPaymentBreakdown(int enrollmentId)
        {
            try
            {
                // Get total units
                int totalUnits = GetTotalUnitsFromDatabase(enrollmentId);
                if (totalUnits <= 0) totalUnits = 15; // Fallback

                // Get fee rates
                var fees = GetCurrentFeeSettings();

                // Calculate
                return new PaymentBreakdown
                {
                    TuitionFee = totalUnits * fees.TuitionPerUnit,
                    MiscellaneousFee = fees.MiscellaneousFee,
                    TotalAmountDue = (totalUnits * fees.TuitionPerUnit) + fees.MiscellaneousFee
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error calculating new breakdown: {ex}");
                return new PaymentBreakdown
                {
                    TuitionFee = 15000.00m,
                    MiscellaneousFee = 5000.00m,
                    TotalAmountDue = 20000.00m
                };
            }
        }



        private int GetTotalUnitsFromDatabase(int enrollmentId)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT SUM(s.units) 
                FROM student_enrollments se
                JOIN course_subjects cs ON se.course_id = cs.course_id 
                    AND se.year_level = cs.year_level
                    AND se.semester = cs.semester
                JOIN subjects s ON cs.subject_id = s.subject_id
                WHERE se.enrollment_id = @enrollmentId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@enrollmentId", enrollmentId);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                        return 0; // Default if no units found
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting units: {ex}");
                return 0;
            }
        }

        public class PaymentBreakdown
        {
            public decimal TotalAmountDue { get; set; }
            public decimal TuitionFee { get; set; }
            public decimal MiscellaneousFee { get; set; }
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
        }

        public class FeeSettings
        {
            public decimal TuitionPerUnit { get; set; }
            public decimal MiscellaneousFee { get; set; }
        }

        private FeeSettings GetCurrentFeeSettings()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT tuition_per_unit, miscellaneous_fee 
                FROM fee_settings 
                ORDER BY effective_date DESC 
                LIMIT 1";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new FeeSettings
                                {
                                    TuitionPerUnit = reader.GetDecimal(0),
                                    MiscellaneousFee = reader.GetDecimal(1)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting fee settings: {ex}");
            }

            // Fallback values if no settings found
            return new FeeSettings
            {
                TuitionPerUnit = 1000.00m,
                MiscellaneousFee = 5000.00m
            };
        }

        private void CheckDatabaseState(int enrollmentId)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    Debug.WriteLine("Database connection successful");

                    // 1. Check if enrollment exists
                    var enrollmentCmd = new MySqlCommand(
                        "SELECT COUNT(*) FROM student_enrollments WHERE enrollment_id = @enrollmentId",
                        conn);
                    enrollmentCmd.Parameters.AddWithValue("@enrollmentId", enrollmentId);
                    var enrollmentCount = Convert.ToInt32(enrollmentCmd.ExecuteScalar());
                    Debug.WriteLine($"Found {enrollmentCount} enrollment records");

                    if (enrollmentCount > 0)
                    {
                        // 2. Get course details
                        var courseCmd = new MySqlCommand(
                            @"SELECT c.course_code, se.year_level, se.semester 
                      FROM student_enrollments se
                      JOIN courses c ON se.course_id = c.course_id
                      WHERE se.enrollment_id = @enrollmentId",
                            conn);
                        courseCmd.Parameters.AddWithValue("@enrollmentId", enrollmentId);

                        using (var reader = courseCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string courseCode = reader.GetString(0);
                                string yearLevel = reader.GetString(1);
                                string semester = reader.GetString(2);
                                Debug.WriteLine($"Course: {courseCode}, Year: {yearLevel}, Sem: {semester}");

                                // 3. Check subjects for this course
                                var subjectCmd = new MySqlCommand(
                                    @"SELECT COUNT(*) FROM course_subjects cs
                              JOIN courses c ON cs.course_id = c.course_id
                              WHERE c.course_code = @courseCode
                              AND cs.semester = @semester
                              AND cs.year_level = @yearLevel",
                                    conn);
                                subjectCmd.Parameters.AddWithValue("@courseCode", courseCode);
                                subjectCmd.Parameters.AddWithValue("@semester", semester);
                                subjectCmd.Parameters.AddWithValue("@yearLevel", yearLevel);
                                var subjectCount = Convert.ToInt32(subjectCmd.ExecuteScalar());
                                Debug.WriteLine($"Found {subjectCount} subjects for this course/semester/year");

                                // 4. Check fee settings
                                var feeCmd = new MySqlCommand(
                                    "SELECT COUNT(*) FROM fee_settings WHERE effective_date <= @currentDate",
                                    conn);
                                feeCmd.Parameters.AddWithValue("@currentDate", DateTime.Today);
                                var feeCount = Convert.ToInt32(feeCmd.ExecuteScalar());
                                Debug.WriteLine($"Found {feeCount} fee settings records");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database check failed: {ex}");
            }
        }
    }
}
