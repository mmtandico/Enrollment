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
            monthCalendar1.Width = 300;
            monthCalendar1.Height = 300;

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
    }
}
