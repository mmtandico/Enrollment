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
    public partial class AdminUser : Form
    {
        public AdminUser()
        {
            InitializeComponent();
            InitializeDataGridView();
        }

        private void AdminUser_Load(object sender, EventArgs e)
        {
            DataGridUsers.AllowUserToResizeColumns = false;
            DataGridUsers.AllowUserToResizeRows = false;
            DataGridUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn column in DataGridUsers.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            int totalCols1 = DataGridUsers.Columns.Count;
            DataGridUsers.Columns[totalCols1 - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridUsers.Columns[totalCols1 - 1].Width = 40;
            DataGridUsers.Columns[totalCols1 - 2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridUsers.Columns[totalCols1 - 2].Width = 40;
            DataGridUsers.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridUsers.Columns[0].Width = 50;
            DataGridUsers.Columns[1].Width = 150;
            DataGridUsers.Columns[2].Width = 500;
            DataGridUsers.Columns[3].Width = 50;
            DataGridUsers.Columns[4].Width = 100;
            DataGridUsers.Columns[5].Width = 100;
            DataGridUsers.Columns[6].Width = 100;
            DataGridUsers.RowTemplate.Height = 35;
            DataGridUsers.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            foreach (DataGridViewColumn col in DataGridUsers.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
            ////////////////////////////////////////////////
            DataGridAdmins.AllowUserToResizeColumns = false;
            DataGridAdmins.AllowUserToResizeRows = false;
            DataGridAdmins.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn column in DataGridAdmins.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            int totalCols = DataGridAdmins.Columns.Count;
            DataGridAdmins.Columns[totalCols - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridAdmins.Columns[totalCols - 1].Width = 40;
            DataGridAdmins.Columns[totalCols - 2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridAdmins.Columns[totalCols - 2].Width = 40;
            DataGridAdmins.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridAdmins.Columns[0].Width = 50;
            DataGridAdmins.Columns[1].Width = 150;
            DataGridAdmins.Columns[2].Width = 500;
            DataGridAdmins.Columns[3].Width = 50;
            DataGridAdmins.Columns[4].Width = 100;
            DataGridAdmins.Columns[5].Width = 100;
            
            DataGridAdmins.RowTemplate.Height = 35;
            DataGridAdmins.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            foreach (DataGridViewColumn col in DataGridAdmins.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }

            CustomizeDataGridUsers();
            CustomizeDataGridAdmins();
            StyleTwoTabControl();
        }

        private void InitializeDataGridView()
        {
            DataGridUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridUsers.Columns["ColOpen2"].Width = 50;
            DataGridUsers.Columns["ColClose2"].Width = 50;
            DataGridUsers.Columns["ColOpen2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridUsers.Columns["ColClose2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridUsers.RowTemplate.Height = 40;
            DataGridUsers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;


            DataGridUsers.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridUsers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewImageColumn colOpen1 = (DataGridViewImageColumn)DataGridUsers.Columns["ColOpen2"];
            colOpen1.ImageLayout = DataGridViewImageCellLayout.Zoom;

            DataGridViewImageColumn colClose1 = (DataGridViewImageColumn)DataGridUsers.Columns["ColClose2"];
            colClose1.ImageLayout = DataGridViewImageCellLayout.Zoom;

            foreach (DataGridViewColumn col in DataGridUsers.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
            //////////////////////////
            DataGridAdmins.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridAdmins.Columns["ColOpen"].Width = 50;
            DataGridAdmins.Columns["ColClose"].Width = 50;
            DataGridAdmins.Columns["ColOpen"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridAdmins.Columns["ColClose"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridAdmins.RowTemplate.Height = 40;
            DataGridAdmins.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;


            DataGridAdmins.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridAdmins.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewImageColumn colOpen = (DataGridViewImageColumn)DataGridAdmins.Columns["ColOpen"];
            colOpen.ImageLayout = DataGridViewImageCellLayout.Zoom;

            DataGridViewImageColumn colClose = (DataGridViewImageColumn)DataGridAdmins.Columns["ColClose"];
            colClose.ImageLayout = DataGridViewImageCellLayout.Zoom;

            foreach (DataGridViewColumn col in DataGridAdmins.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
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

        private void CustomizeDataGridUsers()
        {
            DataGridUsers.BorderStyle = BorderStyle.None;

            DataGridUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridUsers.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridUsers.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridUsers.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridUsers.EnableHeadersVisualStyles = false;

            DataGridUsers.GridColor = Color.BurlyWood;

            DataGridUsers.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridUsers.RowTemplate.Height = 35;

            DataGridUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            foreach (DataGridViewColumn column in DataGridUsers.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }
        }

        private void CustomizeDataGridAdmins()
        {
            DataGridAdmins.BorderStyle = BorderStyle.None;

            DataGridAdmins.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridAdmins.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridAdmins.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridAdmins.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridAdmins.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridAdmins.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridAdmins.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridAdmins.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridAdmins.EnableHeadersVisualStyles = false;

            DataGridAdmins.GridColor = Color.BurlyWood;

            DataGridAdmins.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridAdmins.RowTemplate.Height = 35;

            DataGridAdmins.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            foreach (DataGridViewColumn column in DataGridAdmins.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }
        }
    }
}
