using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Enrollment_System
{
    public partial class FormCourse : Form
    {

        private FormNewAcademiccs FormNewAcads;

        public Panel Panel8
        {
            get { return panel8; }
        }

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
            CourseViewBSIT viewForm = new CourseViewBSIT(this);
            viewForm.Show();
        }

        private void BtnLMCS_Click(object sender, EventArgs e)
        {
            CourseViewBSCS viewForm = new CourseViewBSCS(this); 
            viewForm.Show(); 
        }

        private void BtnLMTM_Click(object sender, EventArgs e)
        {
            CourseViewBSTM viewForm = new CourseViewBSTM(this);
            viewForm.Show();
        }

        private void BtnLMOAD_Click(object sender, EventArgs e)
        {
            CourseViewBSOAD viewForm = new CourseViewBSOAD(this);
            viewForm.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void BtnLMHM_Click(object sender, EventArgs e)
        {
            CourseViewBSHM viewForm = new CourseViewBSHM(this);
            viewForm.Show();
        }

        private void BtnLMLED_Click(object sender, EventArgs e)
        {
            CourseViewBTLED viewForm = new CourseViewBTLED(this);
            viewForm.Show();
        }

        private void BtnCED_Click(object sender, EventArgs e)
        {
            CourseViewBECED viewForm = new CourseViewBECED(this);
            viewForm.Show();
        }


        public void UpdateCourseBannerImage(string courseCode)
        {
            // Check if PbBanner exists in Panel8
            MessageBox.Show($"Panel8 contains {this.Panel8.Controls.Count} controls.");

            // Now attempt to get PbBanner from Panel8
            PictureBox pictureBox = this.Panel8.Controls["PbBanner"] as PictureBox;

            if (pictureBox != null)
            {
                // Update the image based on the course code
                switch (courseCode)
                {
                    case "BSCS":
                        string bsImagePath = @"C:/Users/w/source/repos/Enrollment/Enrollment_System/Resources/BSCS_Banner.JPG";
                        if (System.IO.File.Exists(bsImagePath))
                        {
                            pictureBox.Image = Image.FromFile(bsImagePath);
                        }
                        else
                        {
                            MessageBox.Show("BSCS image file not found!");
                        }
                        break;

                    case "BSIT":
                        string bsitImagePath = @"path_to_bsit_image.jpg";
                        if (System.IO.File.Exists(bsitImagePath))
                        {
                            pictureBox.Image = Image.FromFile(bsitImagePath);
                        }
                        else
                        {
                            MessageBox.Show("BSIT image file not found!");
                        }
                        break;

                    // Add more cases for other courses as needed
                    default:
                        pictureBox.Image = null;
                        break;
                }

                // Ensure the PictureBox is correctly sized to fit the image
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("PbBanner control is null or not found inside Panel8.");
            }
        }



    }
}
