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

namespace Enrollment_System
{
    public partial class AdminStudents : Form
    {
        public AdminStudents()
        {
            InitializeComponent();

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
            ///////////////////////////////////////////////
            

        }

        private void AdminStudents_Load(object sender, EventArgs e)
        {
            StyleTwoTabControl();

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
    }
}
