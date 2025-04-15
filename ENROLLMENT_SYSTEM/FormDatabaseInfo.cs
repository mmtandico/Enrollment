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

    public partial class FormDatabaseInfo : Form
    {

       

        public FormDatabaseInfo()
        {
            InitializeComponent();
            ApplyButtonEffects();

        }

        
       

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnPI_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormPersonalInfo().Show(); 
        }

        private void BtnEnrollment_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormEnrollment().Show();
        }

        private void BtnCourses_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormCourse().Show();
        }

        private void BtnHome_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormHome().Show();
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

        private void FormDatabaseInfo_Load(object sender, EventArgs e)
        {
            ApplyButtonEffects();
            LoadForm(new AdminDashB());
        }


        // Function to load forms inside MAINPANEL
        private void LoadForm(Form form)
        {
            // Remove any existing form in MAINPANEL
            foreach (Control control in MAINPANEL.Controls)
            {
                control.Dispose();
            }

            // Set the new form as a child of MAINPANEL
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;  // Make the form fill the panel
            MAINPANEL.Controls.Add(form); 
            form.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ApplyButtonEffects()
        {
           
        }

       
        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            
        }

        private void MAINPANEL_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DataGridAdmin_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BtnStudent_Click(object sender, EventArgs e)
        {
            LoadForm(new AdminStudents());
        }

        private void BtnDashB_Click(object sender, EventArgs e)
        {
            LoadForm(new AdminDashB());
        }

        private void BtnEnroll_Click(object sender, EventArgs e)
        {
            LoadForm(new AdminEnrollment());
        }
    }
}
