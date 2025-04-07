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
    public partial class CourseViewBSHM : Form
    {
        private FormCourse parentForm;
        public CourseViewBSHM(FormCourse form)
        {
            InitializeComponent();
            parentForm = form;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnEnroll1_Click(object sender, EventArgs e)
        {
            CourseBSHM courseForm = new CourseBSHM();
            courseForm.TopLevel = false;

            parentForm.Panel8.Controls.Clear();
            parentForm.Panel8.Controls.Add(courseForm);
            courseForm.BringToFront();
            courseForm.Show();

            this.Close();
        }

        private void BtnBack1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
