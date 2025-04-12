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
    public partial class AdminDashB : Form
    {
        public AdminDashB()
        {
            InitializeComponent();

            /////////////////////////////
            DataGridAdmin.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridAdmin.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridAdmin.RowTemplate.Height = 40;
            DataGridAdmin.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            DataGridAdmin.RowTemplate.Height = 40;
            DataGridAdmin.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            DataGridAdmin.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridAdmin.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            foreach (DataGridViewColumn col in DataGridAdmin.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
            ////////////////////////////////

            StyleDataGridAdmin();
        }

        private void DataGridAdmin_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AdminDashB_Load(object sender, EventArgs e)
        {
            monthCalendar1.Width = 300;
            monthCalendar1.Height = 300;


            DataGridAdmin.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridAdmin.AllowUserToResizeColumns = false;
            DataGridAdmin.AllowUserToResizeRows = false;
            DataGridAdmin.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn column in DataGridAdmin.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            DataGridAdmin.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridAdmin.Columns[0].Width = 50;
        }



        private void StyleDataGridAdmin()
        {

            DataGridAdmin.BorderStyle = BorderStyle.None;


            DataGridAdmin.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);


            DataGridAdmin.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridAdmin.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);


            DataGridAdmin.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridAdmin.DefaultCellStyle.SelectionForeColor = Color.White;


            DataGridAdmin.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33); // Rich brown
            DataGridAdmin.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridAdmin.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridAdmin.EnableHeadersVisualStyles = false;


            DataGridAdmin.GridColor = Color.BurlyWood;


            DataGridAdmin.DefaultCellStyle.Font = new Font("Segoe UI", 10);


            DataGridAdmin.RowTemplate.Height = 35;


            DataGridAdmin.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            foreach (DataGridViewColumn column in DataGridAdmin.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }


        }
    }
}
