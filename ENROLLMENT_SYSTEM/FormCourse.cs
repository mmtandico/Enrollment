﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Reflection;
using System.Linq;
using System.Diagnostics;

namespace Enrollment_System
{
    public partial class FormCourse : Form
    {
        private readonly string connectionString = DatabaseConfig.ConnectionString;

        private FormNewAcademiccs FormNewAcads;

        public Panel Panel8
        {
            get { return panel8; }
        }
        public FormCourse()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            UIHelper.ApplyAdminVisibility(BtnDataBase);
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

        public Image GetCurrentBannerImage()
        {
            return PboxBanner?.Image;
        }

        public void SetBannerImage(Image image)
        {
            if (PboxBanner == null || PboxBanner.IsDisposed)
                return;

            if (InvokeRequired)
            {
                Invoke(new Action<Image>(SetBannerImage), image);
                return;
            }

            try
            {
                Image oldImage = PboxBanner.Image;

                if (image == null)
                {
                    PboxBanner.Visible = false;
                    if (oldImage != null)
                    {
                        oldImage.Dispose();
                    }
                    return;
                }

              
                Image newImage = new Bitmap(image); 

                PboxBanner.SizeMode = PictureBoxSizeMode.StretchImage;
                PboxBanner.Image = newImage;
                PboxBanner.Visible = true; 

                if (oldImage != null && oldImage != image)
                {
                    oldImage.Dispose();
                }

                BringBannerToFront();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error setting banner: {ex.Message}");

                try
                {
                    PboxBanner.SizeMode = PictureBoxSizeMode.StretchImage;
                    PboxBanner.Image = Properties.Resources.BANNERPDM;
                    PboxBanner.Visible = true;
                }
                catch
                {
                    PboxBanner.Image = null;
                    PboxBanner.Visible = false;
                }
            }
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

        private void FormCourse_Activated(object sender, EventArgs e)
        {
            LoadEnrolledCourseBanner();
        }

        private void LoadEnrolledCourseBanner()
        {
            // Check session first
            if (SessionManager.CurrentBannerImage != null)
            {
                SetBannerImage(SessionManager.CurrentBannerImage);
                return;
            }

            // Rest of your existing banner loading logic
            if (SessionManager.StudentId <= 0)
            {
                SetDefaultBanner();
                return;
            }

            string courseCode = GetEnrolledCourseCode(SessionManager.StudentId);
            if (!string.IsNullOrEmpty(courseCode))
            {
                UpdateCourseBannerImage(courseCode);
                SessionManager.SelectedCourse = courseCode;
                SessionManager.CurrentBannerCourse = courseCode;
            }
            else
            {
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
                Assembly assembly = Assembly.GetExecutingAssembly();
                string resourceName = $"Enrollment_System.Resources.BANNER_{courseCode}.png";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        // Create a new image and store it
                        var newImage = Image.FromStream(stream);

                        // Set the image (this will handle disposal of old image)
                        SetBannerImage(newImage);

                        // Store in session (clone it)
                        SessionManager.CurrentBannerImage = new Bitmap(newImage);
                        SessionManager.CurrentBannerCourse = courseCode;
                    }
                    else
                    {
                        MessageBox.Show($"Banner image not found for {courseCode} in embedded resources.");
                        SetDefaultBanner();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading banner from resources: {ex.Message}");
                SetDefaultBanner();
            }
        }

        private void SetDefaultBanner()
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string resourceName = "Enrollment_System.Resources.BACKGROUNDCOLOR.png";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        if (PboxBanner.Image != null)
                        {
                            PboxBanner.Image.Dispose();
                        }

                        var image = Image.FromStream(stream);
                        PboxBanner.Image = image;
                        SessionManager.CurrentBannerImage = (Image)image.Clone();
                        SessionManager.CurrentBannerCourse = null;
                    }
                }
            }
            catch
            {
                PboxBanner.Image = null;
                SessionManager.CurrentBannerImage = null;
                SessionManager.CurrentBannerCourse = null;
            }
        }


        #region Navigation Buttons
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

        private void CloseAllForms()
        {
            if (currentCourseView != null && !currentCourseView.IsDisposed)
            {
                currentCourseView.FormClosed -= CurrentFormClosed;
                currentCourseView.Close();
                currentCourseView.Dispose();
                currentCourseView = null;
            }

            foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
            {
                if (form != this && !(form is FormLogin) && !form.IsDisposed)
                {
                    form.Close();
                    form.Dispose();
                }
            }
        }


        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out?", "Logout Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                SessionManager.CurrentBannerImage?.Dispose();
                SessionManager.CurrentBannerImage = null;
                SessionManager.CurrentBannerCourse = null;

                CloseAllForms();
                SessionManager.Logout();
                new FormLogin().Show();
                this.Close();
            }
        }


        public void BringBannerToFront()
        {
            if (PboxBanner != null && panel8.Controls.Contains(PboxBanner))
            {
                PboxBanner.BringToFront();
            }
        }


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

        private static Form currentCourseView = null;

        private void ShowCourseViewForm(Form newForm)
        {
            if (newForm == null)
            {
                MessageBox.Show("Error: Form cannot be null");
                return;
            }

            try
            {
                if (currentCourseView != null)
                {
                    if (currentCourseView.GetType() == newForm.GetType())
                    {
                        if (currentCourseView.WindowState == FormWindowState.Minimized)
                            currentCourseView.WindowState = FormWindowState.Normal;

                        currentCourseView.BringToFront();
                        currentCourseView.Activate();
                        newForm.Dispose();
                        return;
                    }

                    currentCourseView.FormClosed -= CurrentFormClosed;
                    currentCourseView.Close();
                    currentCourseView.Dispose();
                    currentCourseView = null;
                }

                currentCourseView = newForm;
                currentCourseView.FormClosed += CurrentFormClosed;
                currentCourseView.Show();

                SetBannerImage(null);
            }
            catch (Exception ex)
            {
                newForm.Dispose();
                MessageBox.Show($"Error showing form: {ex.Message}");
            }
        }

        private void CurrentFormClosed(object sender, FormClosedEventArgs e)
        {

            if (currentCourseView != null && !currentCourseView.IsDisposed)
            {
                currentCourseView.Dispose();
            }
            currentCourseView = null;
        }


        private void BtnLMIT_Click(object sender, EventArgs e)
        {
            ShowCourseViewForm(new CourseViewBSIT(this));
        }

        private void BtnLMCS_Click(object sender, EventArgs e)
        {
            ShowCourseViewForm(new CourseViewBSCS(this));
        }

        private void BtnLMTM_Click(object sender, EventArgs e)
        {
            ShowCourseViewForm(new CourseViewBSTM(this));
        }

        private void BtnLMOAD_Click(object sender, EventArgs e)
        {
            ShowCourseViewForm(new CourseViewBSOAD(this));
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void BtnLMHM_Click(object sender, EventArgs e)
        {
            ShowCourseViewForm(new CourseViewBSHM(this));
        }

        private void BtnLMLED_Click(object sender, EventArgs e)
        {
            ShowCourseViewForm(new CourseViewBTLED(this));
        }

        private void BtnCED_Click(object sender, EventArgs e)
        {
            ShowCourseViewForm(new CourseViewBECED(this));
        }


        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}