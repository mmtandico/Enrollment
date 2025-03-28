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
    public partial class FormPersonalInfo : Form
    {
        private Image[] images;
        private int currentImageIndex = 0;

        public FormPersonalInfo()
        {
            InitializeComponent();

            
            images = new Image[]
            {
                Properties.Resources.P9700277_1_1024x576,
                Properties.Resources.Graduation_2019_1_1536x1024,
                Properties.Resources.BGIMAGE1,
                Properties.Resources.BGIMAGE2
            };

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;


            pictureBox1.Image = images[currentImageIndex];


            timer1.Interval = 3000;
            timer1.Start();


            pictureBox2.Image = Properties.Resources.PROFILE; 
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

    
            BtnUpload.Click += BtnUpload_Click;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnHome_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormHome().Show();
        }

        private void BtnCourses_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormCourse().Show();
        }

        private void BtnEnrollment_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormEnrollment().Show();
        }

        private void BtnDataBase_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormDatabaseInfo().Show();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                new FormLogin().Show();
                this.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            currentImageIndex++;
            if (currentImageIndex >= images.Length)
            {
                currentImageIndex = 0;
            }

            pictureBox1.Image = images[currentImageIndex];
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
           
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    
                    string filePath = openFileDialog.FileName;

                    pictureBox2.Image = Image.FromFile(filePath);

                    pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DisableFieldsInGroupBox(groupBox1);
            DisableFieldsInGroupBox(groupBox2);
            DisableFieldsInGroupBox(groupBox3);
          

            MessageBox.Show("Fields have been saved and locked. They are now unclickable.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DisableFieldsInGroupBox(GroupBox groupBox)
        {
            foreach (Control control in groupBox.Controls)
            {
                if (control is TextBox || control is ComboBox || control is DateTimePicker || control is Panel)
                {
                    control.Enabled = false; 
                    control.BackColor = SystemColors.ControlLight;
                }

                if (control is Panel) 
                   {
                    foreach (Control subControl in panel11.Controls)
                    {
                        if (subControl is TextBox || subControl is ComboBox || subControl is DateTimePicker)
                        {
                            subControl.Enabled = false; 
                        }
                    }
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            EnableFieldsInGroupBox(groupBox1);
            EnableFieldsInGroupBox(groupBox2);
            EnableFieldsInGroupBox(groupBox3);
         
            MessageBox.Show("Fields are now unlocked for editing.", "Edit Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void EnableFieldsInGroupBox(GroupBox groupBox)
        {
            foreach (Control control in groupBox.Controls)
            {
                if (control is TextBox || control is ComboBox || control is DateTimePicker || control is Panel)
                {
                    control.Enabled = true; 
                    control.BackColor = SystemColors.Window;
                }

                
                if (control is Panel)
                    {
                    foreach (Control subControl in panel11.Controls)
                    {
                        if (subControl is TextBox || subControl is ComboBox || subControl is DateTimePicker)
                        {
                            subControl.Enabled = true; 
                        }
                    }
                }
            }
        }
    }
}
