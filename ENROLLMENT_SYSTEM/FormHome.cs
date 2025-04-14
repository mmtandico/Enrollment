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
    public partial class FormHome : Form
    {
        public FormHome()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            UIHelper.ApplyAdminVisibility(BtnDataBase);
            this.Text = "Welcome to Enrollment System";
            if (!string.IsNullOrEmpty(SessionManager.LastName) && !string.IsNullOrEmpty(SessionManager.FirstName))
            {
                LblWelcome.Text = $"{SessionManager.LastName}, {SessionManager.FirstName[0]}.";
            }
            else if (!string.IsNullOrEmpty(SessionManager.LastName))
            {
                LblWelcome.Text = $"{SessionManager.LastName}";
            }
            else if (!string.IsNullOrEmpty(SessionManager.FirstName))
            {
                LblWelcome.Text = $"{SessionManager.FirstName[0]}.";
            }
            else
            {
                LblWelcome.Text = "";
            }
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
        private void SwitchForm(Form newForm)
        {
            this.Hide(); 
            newForm.Show(); 
            this.Close(); 
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnCourses_Click(object sender, EventArgs e)
        {
            SwitchForm(new FormCourse());
        }

        private void BtnEnrollment_Click(object sender, EventArgs e)
        {
            SwitchForm(new FormEnrollment());
        }

        private void BtnPI_Click(object sender, EventArgs e)
        {
            SwitchForm(new FormPersonalInfo());
        }

        private void BtnDataBase_Click(object sender, EventArgs e)
        {
            SwitchForm(new FormDatabaseInfo());
        }

        private void BtnHome_Click(object sender, EventArgs e)
        {    
            
        }
        
    }
}
