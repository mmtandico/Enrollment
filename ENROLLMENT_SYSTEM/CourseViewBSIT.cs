using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Diagnostics;

namespace Enrollment_System
{
    public partial class CourseViewBSIT : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
        private FormEnrollment enrollmentForm;
        private MySqlConnection dbConnection;
        private FormCourse parentForm;
        private Image bannerImage;

        public CourseViewBSIT(FormCourse form)
        {
            InitializeComponent();
            parentForm = form;
            bannerImage = parentForm.GetCurrentBannerImage()?.Clone() as Image;
            this.FormClosing += CourseViewBSIT_FormClosing;
        }

        private void CourseViewBSIT_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!parentForm.IsDisposed && bannerImage != null)
                {
                    // Clone the image to avoid disposal issues
                    using (var tempImage = new Bitmap(bannerImage))
                    {
                        parentForm.SetBannerImage(tempImage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error if debugger is attached
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine($"Error restoring banner: {ex.Message}");
                }
            }
            finally
            {
                // Clean up resources
                dbConnection?.Dispose();
                bannerImage?.Dispose();
                bannerImage = null;
            }
        }

        private void BtnBack_Click(object sender, EventArgs e) => this.Close();
        private void BtnBack1_Click(object sender, EventArgs e) => this.Close();

        private void SwitchToPersonalInfoForm()
        {
            var personalInfoForm = new FormPersonalInfo
            {
                StartPosition = FormStartPosition.CenterParent
            };
            personalInfoForm.Show();
            this.Hide();
        }

        private void BtnEnroll1_Click(object sender, EventArgs e)
        {
            if (!ValidationHelper.IsPersonalInfoComplete(SessionManager.UserId))
            {
                ValidationHelper.ShowValidationError(this);
                SwitchToPersonalInfoForm();
                return;
            }

            if (HasPendingEnrollment())
            {
                MessageBox.Show("You already have a pending enrollment request. Please wait for it to be processed before creating a new one.",
                              "Pending Enrollment Exists",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            if (!ConfirmCourseSelection("BSIT", "Bachelor of Science in Information Technology"))
                return;

            ShowBothEnrollmentForms();
        }

        private bool HasPendingEnrollment()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    const string query = @"SELECT COUNT(*) 
                                        FROM student_enrollments 
                                        WHERE student_id = @StudentId 
                                        AND status IN ('Pending', 'Payment Pending')";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", SessionManager.StudentId);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking pending enrollments: {ex.Message}");
                return false;
            }
        }

        private void ShowBothEnrollmentForms()
        {
            SessionManager.SelectedCourse = "Bachelor of Science in Information Technology";
            parentForm.Panel8.Tag = "BSIT";

            var mainParent = parentForm;
            this.Hide();
            mainParent.Hide();

            enrollmentForm = new FormEnrollment
            {
                StartPosition = FormStartPosition.CenterParent
            };

            enrollmentForm.FormClosed += (s, args) =>
            {
                if (!this.IsDisposed)
                {
                    if (!mainParent.IsDisposed)
                    {
                        try
                        {
                            mainParent.Invoke((MethodInvoker)delegate
                            {
                                if (!mainParent.IsDisposed && !mainParent.Panel8.IsDisposed)
                                {
                                    mainParent.Show();
                                    HandleEnrollmentCompletion(mainParent);
                                }
                            });
                        }
                        catch (ObjectDisposedException)
                        {
                           
                        }
                    }
                    this.Close();
                }
            };

            using (var academicForm = new FormNewAcademiccs())
            {
                academicForm.StartPosition = FormStartPosition.CenterParent;
                enrollmentForm.Show();
                academicForm.ShowDialog();
            }
        }

        private bool ConfirmCourseSelection(string courseCode, string courseName)
        {
            if (IsStudentEnrolledInCourse(courseCode))
                return true;

            return MessageBox.Show(
                $"You are not currently enrolled in this course.\nDo you want to proceed with enrollment in\n{courseName}?",
                "Confirm Course Enrollment",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            ) == DialogResult.Yes;
        }

        private bool IsStudentEnrolledInCourse(string courseCode)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    const string query = @"SELECT COUNT(*) 
                                           FROM student_enrollments se
                                           JOIN courses c ON se.course_id = c.course_id
                                           WHERE se.student_id = @StudentId
                                           AND c.course_code = @CourseCode
                                           AND se.status IN ('Enrolled', 'Completed')";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", SessionManager.StudentId);
                        cmd.Parameters.AddWithValue("@CourseCode", courseCode);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void HandleEnrollmentCompletion(FormCourse mainParent)
        {
            if (mainParent.IsDisposed || mainParent.Panel8.IsDisposed)
                return;

            try
            {
                mainParent.Panel8.Controls.Clear();
                var courseForm = new CourseBSIT
                {
                    TopLevel = false,
                    Dock = DockStyle.Fill
                };
                mainParent.Panel8.Controls.Add(courseForm);
                courseForm.Show();

                if (mainParent.Panel8.Tag?.ToString() == "BSIT")
                    mainParent.UpdateCourseBannerImage("BSIT");
            }
            catch (ObjectDisposedException)
            {
                // Handle disposal gracefully
            }
        }

        private void panel5_Paint(object sender, PaintEventArgs e) { }
        private void label28_Click(object sender, EventArgs e) { }
        private void label29_Click(object sender, EventArgs e) { }
        private void label30_Click(object sender, EventArgs e) { }
        private void label27_Click(object sender, EventArgs e) { }
        private void label24_Click(object sender, EventArgs e) { }
        private void label25_Click(object sender, EventArgs e) { }
        private void label26_Click(object sender, EventArgs e) { }
        private void label17_Click(object sender, EventArgs e) { }
        private void label22_Click(object sender, EventArgs e) { }
        private void label23_Click(object sender, EventArgs e) { }
        private void pictureBox8_Click(object sender, EventArgs e) { }
        private void pictureBox7_Click(object sender, EventArgs e) { }
        private void pictureBox4_Click(object sender, EventArgs e) { }
        private void panel6_Paint(object sender, PaintEventArgs e) { }
        private void label16_Click(object sender, EventArgs e) { }
        private void panel4_Paint(object sender, PaintEventArgs e) { }
        private void label44_Click(object sender, EventArgs e) { }
        private void label38_Click(object sender, EventArgs e) { }
        private void label39_Click(object sender, EventArgs e) { }
        private void label40_Click(object sender, EventArgs e) { }
        private void label41_Click(object sender, EventArgs e) { }
        private void label33_Click(object sender, EventArgs e) { }
        private void label35_Click(object sender, EventArgs e) { }
        private void label36_Click(object sender, EventArgs e) { }
        private void label37_Click(object sender, EventArgs e) { }
        private void label34_Click(object sender, EventArgs e) { }
        private void label21_Click(object sender, EventArgs e) { }
        private void label31_Click(object sender, EventArgs e) { }
        private void label32_Click(object sender, EventArgs e) { }
        private void pictureBox9_Click(object sender, EventArgs e) { }
        private void pictureBox6_Click(object sender, EventArgs e) { }
        private void pictureBox5_Click(object sender, EventArgs e) { }
        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void panel10_Paint(object sender, PaintEventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void panel9_Paint(object sender, PaintEventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label43_Click(object sender, EventArgs e) { }
        private void BtnEnroll_Click(object sender, EventArgs e) { }
        private void label11_Click(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) => this.Close();
        private void button2_Click(object sender, EventArgs e) { }
    }
}