using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Diagnostics;


namespace Enrollment_System
{
    public partial class CourseViewBSOAD : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
        private FormEnrollment enrollmentForm;
        private MySqlConnection dbConnection;
        private FormCourse parentForm;
        private Image bannerImage;

        public CourseViewBSOAD(FormCourse form)
        {
            InitializeComponent();
            parentForm = form;
            bannerImage = parentForm.GetCurrentBannerImage()?.Clone() as Image;
            this.FormClosing += CourseViewBSOAD_FormClosing;
        }

        private void CourseViewBSOAD_FormClosing(object sender, FormClosingEventArgs e)
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

            if (!ConfirmCourseSelection("BSOAD", "Bachelor of Science in Office Administration"))
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
            SessionManager.SelectedCourse = "Bachelor of Science in Office Administration";
            parentForm.Panel8.Tag = "BSOAD";

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
            if (parentForm.Panel8.Tag?.ToString() == courseCode || IsStudentEnrolledInCourse(courseCode))
                return true;

            return MessageBox.Show(
                $"You've been already enrolled.\nDo you want Change to your course to\n {courseName}?",
                "Confirm Course Change",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
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
                                        AND se.status != 'Dropped'";

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
                var courseForm = new CourseBSOAD
                {
                    TopLevel = false,
                    Dock = DockStyle.Fill
                };
                mainParent.Panel8.Controls.Add(courseForm);
                courseForm.Show();

                if (mainParent.Panel8.Tag?.ToString() == "BSOAD")
                    mainParent.UpdateCourseBannerImage("BSOAD");
            }
            catch (ObjectDisposedException)
            {
               
            }
        }
    }
}