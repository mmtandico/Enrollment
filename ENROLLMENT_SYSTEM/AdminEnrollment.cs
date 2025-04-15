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
    public partial class AdminEnrollment : Form
    {
        public AdminEnrollment()
        {
            InitializeComponent();
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

        private void AdminEnrollment_Load(object sender, EventArgs e)
        {
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
    }
}
