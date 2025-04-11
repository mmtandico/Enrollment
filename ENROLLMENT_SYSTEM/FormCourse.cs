using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Enrollment_System
{
    public partial class FormCourse : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
        private FormNewAcademiccs FormNewAcads;

        public Panel Panel8
        {
            get { return panel8; }
        }
        public FormCourse()
        {
            InitializeComponent();
            InitializeBannerPictureBox();
            LoadWelcomeMessage();
            LoadEnrolledCourseBanner();
        }

        private void InitializeBannerPictureBox()
        {
            this.PboxBanner = new PictureBox();
            this.PboxBanner.BackColor = System.Drawing.Color.Transparent;
            this.PboxBanner.Location = new System.Drawing.Point(0, 0);
            this.PboxBanner.Name = "PboxBanner";
            this.PboxBanner.Size = new System.Drawing.Size(1230, 229);
            this.PboxBanner.TabIndex = 31;
            this.PboxBanner.TabStop = false;
            this.PboxBanner.SizeMode = PictureBoxSizeMode.StretchImage;
            this.panel8.Controls.Add(this.PboxBanner);

        }

        private void LoadWelcomeMessage()
        {
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

        private void LoadEnrolledCourseBanner()
        {
            if (SessionManager.StudentId <= 0)
            {
                MessageBox.Show("No student information available. Please log in as a student.");
                return;
            }

            string courseCode = GetEnrolledCourseCode(SessionManager.StudentId);
            if (!string.IsNullOrEmpty(courseCode))
            {
                UpdateCourseBannerImage(courseCode);
                SessionManager.SelectedCourse = courseCode;
            }
            else
            {
                MessageBox.Show("You are not currently enrolled in any course.");
                SetDefaultBanner();
            }
        }

        private string GetEnrolledCourseCode(int studentId)
        {
            string courseCode = null;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"SELECT c.course_code 
                                   FROM student_enrollments se
                                   JOIN courses c ON se.course_id = c.course_id
                                   WHERE se.student_id = @StudentId
                                   AND se.status IN ('Enrolled', 'Completed', 'Pending', 'Drop')
                                   ORDER BY se.enrollment_id DESC
                                   LIMIT 1";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentId", studentId);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        courseCode = result?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error accessing database: {ex.Message}");
            }
            return courseCode;
        }

        public void UpdateCourseBannerImage(string courseCode)
        {
            try
            {
                string imagePath = $@"C:\Users\w\source\repos\Enrollment\Enrollment_System\Resources\BANNER_{courseCode}.png";

                if (!File.Exists(imagePath))
                {
                    MessageBox.Show($"Banner image not found for {courseCode} at:\n{imagePath}");
                    SetDefaultBanner();
                    return;
                }

                // Load the new image
                Image newImage = Image.FromFile(imagePath);


                // Dispose old image if exists
                if (PboxBanner.Image != null)
                {
                    Image oldImage = PboxBanner.Image;
                    PboxBanner.Image = null;
                    oldImage.Dispose();
                }

                PboxBanner.Image = newImage;
                BringBannerToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading banner: {ex.Message}");
                SetDefaultBanner();
            }
        }

        private void SetDefaultBanner()
        {
            string defaultImagePath = @"C:\Users\w\source\repos\Enrollment\Enrollment_System\Resources\BACKGROUNDCOLOR.png";

            try
            {
                if (File.Exists(defaultImagePath))
                {
                    PboxBanner.Image = Image.FromFile(defaultImagePath);
                }
                else
                {
                    PboxBanner.Image = null;
                }
            }
            catch
            {
                PboxBanner.Image = null;
            }
        }

        #region Navigation Buttons
        private void BtnHome_Click(object sender, EventArgs e)
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
            DialogResult result = MessageBox.Show("Are you sure you want to log out?",
                                               "Logout Confirmation",
                                               MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SessionManager.Logout();
                new FormLogin().Show();
                this.Close();
            }
        }
        #endregion
        public void BringBannerToFront()
        {
            if (PboxBanner != null && panel8.Controls.Contains(PboxBanner))
            {
                PboxBanner.BringToFront();
            }
        }

        #region Course Selection Buttons
        private void BtnBSCS_Click(object sender, EventArgs e)
        {

            UpdateCourseBannerImage("BSCS");
        }

        private void BtnBSIT_Click(object sender, EventArgs e)
        {

            UpdateCourseBannerImage("BSIT");
        }

        private void BtnBSTM_Click(object sender, EventArgs e)
        {
            UpdateCourseBannerImage("BSTM");
        }

        private void BtnBSHM_Click(object sender, EventArgs e)
        {
            UpdateCourseBannerImage("BSHM");
        }

        private void BtnBSOAD_Click(object sender, EventArgs e)
        {
            UpdateCourseBannerImage("BSOAD");
        }

        private void BtnBTLED_Click(object sender, EventArgs e)
        {
            UpdateCourseBannerImage("BTLED");
        }

        private void BtnBECED_Click(object sender, EventArgs e)
        {
            UpdateCourseBannerImage("BECED");
        }
        #endregion
        private void BtnHome_Click_1(object sender, EventArgs e)
        {
            this.Close();
            new FormHome().Show();
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

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}