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
        public Panel panel8;

        public CourseViewBSCS()
        {
            InitializeComponent();
            FormNewAcads = new FormNewAcademiccs();
            
        }


        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnEnroll_Click(object sender, EventArgs e)
        {

          
 

        }


    }
}
