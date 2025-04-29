using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enrollment_System
{
    public partial class StudentHistory : Form
    {
        public StudentHistory()
        {
            InitializeComponent();

            foreach (DataGridViewColumn col in DataGridHistory.Columns)
            {
                col.Frozen = false;
            }
            DataGridHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridHistory.Columns["ColOpen1"].Width = 50;
            DataGridHistory.Columns["ColOpen1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridHistory.RowTemplate.Height = 40;
            DataGridHistory.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;


            DataGridHistory.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewImageColumn colOpen2 = (DataGridViewImageColumn)DataGridHistory.Columns["ColOpen1"];
            colOpen2.ImageLayout = DataGridViewImageCellLayout.Zoom;

            foreach (DataGridViewColumn col in DataGridHistory.Columns)
            {
                //col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StyleDataGridHistory()
        {
            DataGridHistory.BorderStyle = BorderStyle.None;

            DataGridHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridHistory.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridHistory.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridHistory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridHistory.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridHistory.EnableHeadersVisualStyles = false;

            DataGridHistory.GridColor = Color.BurlyWood;

            DataGridHistory.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridHistory.RowTemplate.Height = 35;

            DataGridHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn column in DataGridHistory.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }

        }

        private void StudentHistory_Load(object sender, EventArgs e)
        {
            DataGridHistory.AllowUserToResizeColumns = false;
            DataGridHistory.AllowUserToResizeRows = false;
            DataGridHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn column in DataGridHistory.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            int totalCols2 = DataGridHistory.Columns.Count;
            DataGridHistory.Columns[totalCols2 - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridHistory.Columns[totalCols2 - 1].Width = 40;
            
            DataGridHistory.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridHistory.Columns[1].Width = 50;
            DataGridHistory.RowTemplate.Height = 35;


            foreach (DataGridViewColumn col in DataGridHistory.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }

            CustomizeDataGridHistory();
            StyleDataGridHistory();
        }

        private void CustomizeDataGridHistory()
        {
            DataGridHistory.BorderStyle = BorderStyle.None;

            DataGridHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridHistory.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridHistory.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridHistory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridHistory.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridHistory.EnableHeadersVisualStyles = false;

            DataGridHistory.GridColor = Color.BurlyWood;

            DataGridHistory.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridHistory.RowTemplate.Height = 35;

            DataGridHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            foreach (DataGridViewColumn column in DataGridHistory.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }
        }

        private void DataGridHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
