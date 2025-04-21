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
    public partial class FormPayment : Form
    {
        public FormPayment()
        {
            InitializeComponent();
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            // Create an OpenFileDialog instance
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a Picture",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif", // Filter for image files
                Multiselect = false // Allow only one file to be selected
            };

            // Display the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Load the selected image into the PictureBox
                    PBProfPayment.Image = Image.FromFile(openFileDialog.FileName);

                    // Optional: Adjust PictureBox size mode to fit the image
                    PBProfPayment.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                catch (Exception ex)
                {
                    // Display an error message if something goes wrong
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
