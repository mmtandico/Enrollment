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
    public partial class CourseViewBSOAD : Form
    {
        private FormCourse parentForm;
        public CourseViewBSOAD(FormCourse form)
        {
            InitializeComponent();
            parentForm = form;
        }

        private void BtnEnroll1_Click(object sender, EventArgs e)
        {

            if (parentForm.Panel8.Tag != null && parentForm.Panel8.Tag.ToString() != "BSOAD")
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

            SessionManager.SelectedCourse = "Bachelor of Science in Office Administration";
            parentForm.Panel8.Tag = "BSOAD";

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
                CourseBSOAD courseForm = new CourseBSOAD
                {
                    TopLevel = false,
                    Dock = DockStyle.Fill
                };
                parentForm.Panel8.Controls.Add(courseForm);
                courseForm.Show();


                if (parentForm.Panel8.Tag.ToString() == "BSOAD")
                {
                    parentForm.UpdateCourseBannerImage("BSOAD");
                }
            };

            newAcademicForm.ShowDialog();


            this.Close();

        }

        private void BtnBack1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
