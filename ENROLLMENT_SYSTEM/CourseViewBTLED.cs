using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Enrollment_System
{
    public partial class CourseViewBTLED : Form
    {
        private MySqlConnection dbConnection;
        private FormCourse parentForm;

        public CourseViewBTLED(FormCourse form)
        {
            InitializeComponent();
            parentForm = form;
            this.FormClosing += CourseViewBTLED_FormClosing;
        }

        private void CourseViewBTLED_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dbConnection != null)
            {
                dbConnection.Dispose();
                dbConnection = null;
            }
        }

        private void BtnEnroll1_Click(object sender, EventArgs e)
        {
            if (parentForm.Panel8.Tag != null && parentForm.Panel8.Tag.ToString() != "BTLED")
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

            SessionManager.SelectedCourse = "Bachelor of Technology and Livelihood Education";
            parentForm.Panel8.Tag = "BTLED";

            FormEnrollment enrollmentForm = new FormEnrollment
            {
                StartPosition = FormStartPosition.CenterParent
            };

            enrollmentForm.Show();
            parentForm.Hide();
            this.Hide();

            FormNewAcademiccs newAcademicForm = new FormNewAcademiccs
            {
                StartPosition = FormStartPosition.CenterParent

            };

            newAcademicForm.FormClosed += (s, args) =>
            {
                parentForm.Panel8.Controls.Clear();
                CourseBSIT courseForm = new CourseBSIT
                {
                    TopLevel = false,
                    Dock = DockStyle.Fill
                };
                parentForm.Panel8.Controls.Add(courseForm);
                courseForm.Show();

                if (parentForm.Panel8.Tag.ToString() == "BTLED")
                {
                    parentForm.UpdateCourseBannerImage("BTLED");
                }

                parentForm.Close();
                this.Close();
            };

            newAcademicForm.ShowDialog();

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
