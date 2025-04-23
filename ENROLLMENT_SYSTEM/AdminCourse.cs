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
        private int courseId;
        public AdminCourse()
        {
            InitializeComponent();
            LoadSubjectsCourse(courseId);
            InitializeDataGridView();
            StyleTwoTabControl();

        }

        private void AdminCourse_Load(object sender, EventArgs e)
        {

            StyleTwoTabControl();
            InitializeDataGridView();

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

        private void LoadSubjectsCourse(int courseId)
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
                            c.course_code,
                            cs.semester,
                            cs.year_level
                        FROM course_subjects cs
                        JOIN subjects s ON cs.subject_id = s.subject_id
                        JOIN courses c ON cs.course_id = c.course_id
                        WHERE cs.course_id = @courseId
                        ORDER BY cs.year_level, cs.semester, s.subject_code;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@courseId", courseId);

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            DataGridSubjects.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading course subjects: " + ex.Message);
            }
        }

    }
}
