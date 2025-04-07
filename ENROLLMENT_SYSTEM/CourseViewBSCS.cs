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
    public partial class CourseViewBSCS : Form
    {
        private FormNewAcademiccs FormNewAcads;
        private FormCourse parentForm;

        public CourseViewBSCS(FormCourse form)
        {
            InitializeComponent();
            FormNewAcads = new FormNewAcademiccs();
            parentForm = form;
        }


        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnEnroll_Click(object sender, EventArgs e)
        {
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
                    return; // Cancel loading if user says no
                }
            }

            CourseBSCS courseForm = new CourseBSCS();
            courseForm.TopLevel = false;

            parentForm.Panel8.Controls.Clear();
            parentForm.Panel8.Controls.Add(courseForm);
            courseForm.BringToFront();
            parentForm.Panel8.Tag = "BSCS";
            courseForm.Show();

            this.Close();

        }

        private void BtnBack1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
