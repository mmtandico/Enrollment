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
    public partial class FormCourse : Form
    {
        public FormCourse()
        {
            InitializeComponent();
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

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void BtnHome_Click_1(object sender, EventArgs e)
        {
            this.Close();
            new FormHome().Show();
        }

        private void BtnEnrollment_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormEnrollment().Show();
        }

        private void BtnPI_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormPersonalInfo().Show();

        }

        private void BtnDataBase_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormDatabaseInfo().Show();
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void BtnLMIT_Click(object sender, EventArgs e)
        {
            CourseBSIT f = new CourseBSIT();
            f.TopLevel = false;
            panel8.Controls.Add(f);
            f.BringToFront();
            f.Show();
        }

        private void BtnLMCS_Click(object sender, EventArgs e)
        {
            CourseBSCS f = new CourseBSCS();
            f.TopLevel = false;
            panel8.Controls.Add(f);
            f.BringToFront();
            f.Show();
        }

        private void BtnLMTM_Click(object sender, EventArgs e)
        {
            CourseBSTM f = new CourseBSTM();
            f.TopLevel = false;
            panel8.Controls.Add(f);
            f.BringToFront();
            f.Show();
        }

        private void BtnLMOAD_Click(object sender, EventArgs e)
        {
            CourseBSOAD f = new CourseBSOAD();
            f.TopLevel = false;
            panel8.Controls.Add(f);
            f.BringToFront();
            f.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void BtnLMHM_Click(object sender, EventArgs e)
        {
            CourseBSHM f = new CourseBSHM();
            f.TopLevel = false;
            panel8.Controls.Add(f);
            f.BringToFront();
            f.Show();
        }

        private void BtnLMLED_Click(object sender, EventArgs e)
        {
            CourseBTLED f = new CourseBTLED();
            f.TopLevel = false;
            panel8.Controls.Add(f);
            f.BringToFront();
            f.Show();
        }

        private void BtnCED_Click(object sender, EventArgs e)
        {
            CourseBECED f = new CourseBECED();
            f.TopLevel = false;
            panel8.Controls.Add(f);
            f.BringToFront();
            f.Show();
        }
    }
}
