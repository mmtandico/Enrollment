using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Diagnostics;

namespace Enrollment_System
{
    public partial class CourseViewBTLED : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
        private FormEnrollment enrollmentForm;
        private MySqlConnection dbConnection;
        private FormCourse parentForm;
        private Image bannerImage;

        public CourseViewBTLED(FormCourse form)
        {
            InitializeComponent();
            parentForm = form;
            bannerImage = parentForm.GetCurrentBannerImage()?.Clone() as Image;
            this.FormClosing += CourseViewBTLED_FormClosing;
        }

        private void BtnBack_Click(object sender, EventArgs e) => this.Close();
        private void BtnBack1_Click(object sender, EventArgs e) => this.Close();

        private void CourseViewBTLED_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Safely restore banner image to parent form
            if (!parentForm.IsDisposed && bannerImage != null)
            {
                try
                {
                    using (var clonedImage = new Bitmap(bannerImage))
                    {
                        parentForm.SetBannerImage(clonedImage);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error restoring banner: {ex.Message}");
                }
                finally
                {
                    bannerImage?.Dispose();
                }
            }

            // Clean up resources
            dbConnection?.Dispose();
            enrollmentForm?.Dispose();

            // Clear references
            bannerImage = null;
            parentForm = null;
        }

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
            try
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

                if (!ConfirmCourseSelection("BTLED", "Bachelor of Technology and Livelihood Education"))
                    return;

                ShowBothEnrollmentForms();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                Debug.WriteLine($"Error checking pending enrollments: {ex.Message}");
                return false;
            }
        }

        private void ShowBothEnrollmentForms()
        {
            SessionManager.SelectedCourse = "Bachelor of Technology and Livelihood Education";
            parentForm.Panel8.Tag = "BTLED";

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

        private void HandleEnrollmentCompletion(FormCourse mainParent)
        {
            if (mainParent.IsDisposed || mainParent.Panel8.IsDisposed)
                return;

            try
            {
                mainParent.Panel8.Controls.Clear();
                var courseForm = new CourseBTLED
                {
                    TopLevel = false,
                    Dock = DockStyle.Fill
                };
                mainParent.Panel8.Controls.Add(courseForm);
                courseForm.Show();

                if (mainParent.Panel8.Tag?.ToString() == "BTLED")
                    mainParent.UpdateCourseBannerImage("BTLED");
            }
            catch (ObjectDisposedException)
            {
                // Handle disposed objects
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

        private void HandleEnrollmentCompletion()
        {
            // Check if parent form and its panel still exist
            if (parentForm.IsDisposed || parentForm.Panel8.IsDisposed)
            {
                this.Close();
                return;
            }

            parentForm.Panel8.Controls.Clear();
            var courseForm = new CourseBTLED
            {
                TopLevel = false,
                Dock = DockStyle.Fill
            };
            parentForm.Panel8.Controls.Add(courseForm);
            courseForm.Show();

            if (parentForm.Panel8.Tag?.ToString() == "BTLED")
                parentForm.UpdateCourseBannerImage("BTLED");

            this.Close();
        }
    }
}