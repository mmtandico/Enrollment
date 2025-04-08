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
    public partial class CourseViewBTLED : Form
    {
        private FormCourse parentForm;
        public CourseViewBTLED(FormCourse form)
        {
            InitializeComponent();
            parentForm = form;
        }

        private void BtnEnroll1_Click(object sender, EventArgs e)
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
            SessionManager.SelectedCourse = "Bachelor of Technology and Livelihood Education";

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

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
