using System;
using System.Windows.Forms;

namespace Enrollment_System
{
    public partial class CourseViewBSCS : Form
    {
        private FormCourse parentForm;

        public CourseViewBSCS(FormCourse form)
        {
            InitializeComponent();
            parentForm = form;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnEnroll_Click(object sender, EventArgs e)
        {
            // Confirm course change if already selected
            if (parentForm.Panel8.Tag != null)
            {
                DialogResult result = MessageBox.Show(
                    $"You’ve already picked the course \"{parentForm.Panel8.Tag}\".\nDo you want to change it?",
                    "Confirm Course Change",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            // Store selected course name
            SessionManager.SelectedCourse = "Bachelor of Science in Computer Science";

            // Optionally update Panel8 tag if you want to reflect the change
            parentForm.Panel8.Tag = "BSCS";

            // Open the enrollment form (popup)
            FormNewAcademiccs enrollmentForm = new FormNewAcademiccs();
            enrollmentForm.StartPosition = FormStartPosition.CenterParent;
            enrollmentForm.ShowDialog(); // Modal

            // Close this form after showing the enrollment form
            this.Close();
        }


        private void BtnBack1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
