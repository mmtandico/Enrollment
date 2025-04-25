using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;

namespace Enrollment_System
{
    public partial class CourseViewBSCS : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
        private FormEnrollment enrollmentForm;
        private MySqlConnection dbConnection;
        private FormCourse parentForm;
        private Image bannerImage;

        public CourseViewBSCS(FormCourse form)
        {
            InitializeComponent();
            parentForm = form;
            bannerImage = parentForm.GetCurrentBannerImage()?.Clone() as Image;
            this.FormClosing += CourseViewBSCS_FormClosing;
        }

        private void BtnBack_Click(object sender, EventArgs e) => this.Close();
        private void BtnBack1_Click(object sender, EventArgs e) => this.Close();

        private void CourseViewBSCS_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!parentForm.IsDisposed)
            {
                parentForm.SetBannerImage(bannerImage);
            }
            bannerImage?.Dispose();
            dbConnection?.Dispose();
        }

        private void SwitchToPersonalInfoForm()
        {
            var personalInfoForm = new FormPersonalInfo();
            personalInfoForm.StartPosition = FormStartPosition.CenterParent;
            personalInfoForm.Show();
            this.Hide();
        }

        private void BtnEnroll_Click(object sender, EventArgs e)
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

            if (!ConfirmCourseSelection("BSCS", "Bachelor of Science in Computer Science"))
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
                    string query = @"SELECT COUNT(*) 
                          FROM student_enrollments 
                          WHERE student_id = @StudentId 
                          AND status IN ('Pending', 'Payment Pending')";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", SessionManager.StudentId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking pending enrollments: " + ex.Message);
                return false;
            }
        }

        private void ShowBothEnrollmentForms()
        {
            SessionManager.SelectedCourse = "Bachelor of Science in Computer Science";
            parentForm.Panel8.Tag = "BSCS";
            this.Hide();
            parentForm.Hide();

            enrollmentForm = new FormEnrollment()
            {
                StartPosition = FormStartPosition.CenterParent
            };

            using (var academicForm = new FormNewAcademiccs())
            {
                academicForm.StartPosition = FormStartPosition.CenterParent;
                enrollmentForm.Show();
                academicForm.ShowDialog();
            }

            HandleEnrollmentCompletion();
        }

        private bool ConfirmCourseSelection(string courseCode, string courseName)
        {
            if (parentForm.Panel8.Tag?.ToString() == courseCode)
            {
                return true;
            }

            if (IsStudentEnrolledInCourse(courseCode))
            {
                return true;
            }

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
                    string query = @"SELECT COUNT(*) 
                            FROM student_enrollments se
                            JOIN courses c ON se.course_id = c.course_id
                            WHERE se.student_id = @StudentId
                            AND c.course_code = @CourseCode
                            AND se.status != 'Dropped'";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentId", SessionManager.StudentId);
                        cmd.Parameters.AddWithValue("@CourseCode", courseCode);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
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
            parentForm.Panel8.Controls.Clear();
            var courseForm = new CourseBSCS()
            {
                TopLevel = false,
                Dock = DockStyle.Fill
            };
            parentForm.Panel8.Controls.Add(courseForm);
            courseForm.Show();

            if (parentForm.Panel8.Tag?.ToString() == "BSCS")
            {
                parentForm.UpdateCourseBannerImage("BSCS");
            }

            parentForm.Show();

            this.Close();
        }
    }
}
