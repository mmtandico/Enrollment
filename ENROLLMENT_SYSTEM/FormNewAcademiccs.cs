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
    public partial class FormNewAcademiccs : Form
    {
        public string CourseText
        {
            get { return TxtCourse.Text; }
            set { TxtCourse.Text = value; } // Update TxtCourse with the passed value
        }

        public FormNewAcademiccs()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "All files (*.*)|*.*"; 
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    
                    TxtFilePath.Text = openFileDialog.FileName;
                }
            }
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox2.Image = Image.FromFile(openFileDialog.FileName);
                    pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

                    //byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);

                    //SaveProfilePicture(imageBytes);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

        }

        private void FormNewAcademiccs_Load(object sender, EventArgs e)
        {
            
        }

        public void SetText(string message)
        {
            TxtCourse.Text = message;
        }

        private void TxtCourse_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
