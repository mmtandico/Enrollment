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

            
            if (parentForm.Panel8.Tag != null && parentForm.Panel8.Tag.ToString() != "BSCS")
            {
                DialogResult result = MessageBox.Show(
                    $"You’ve already picked the course \"{parentForm.Panel8.Tag}\".\nDo you want to change it?",
                    "Confirm Course Change",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.No)
                    return;
            }

            SessionManager.SelectedCourse = "Bachelor of Science in Computer Science";
            parentForm.Panel8.Tag = "BSCS";

            FormEnrollment enrollmentForm = new FormEnrollment
            {
                StartPosition = FormStartPosition.CenterParent
            };
            enrollmentForm.Show();

            FormNewAcademiccs newAcademicForm = new FormNewAcademiccs
            {
                StartPosition = FormStartPosition.CenterParent
            };

           
            newAcademicForm.FormClosed += (s, args) =>
            {
               
                parentForm.Panel8.Controls.Clear();
                CourseBSCS courseForm = new CourseBSCS
                {
                    TopLevel = false,
                    Dock = DockStyle.Fill
                };
                parentForm.Panel8.Controls.Add(courseForm);
                courseForm.Show();

                
                if (parentForm.Panel8.Tag.ToString() == "BSCS")
                {
                    parentForm.UpdateCourseBannerImage("BSCS");
                }
            };

            newAcademicForm.ShowDialog(); 

            
            this.Close();
        }


        private void BtnBack1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
