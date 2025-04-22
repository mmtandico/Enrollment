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
using System.IO;

namespace Enrollment_System
{
    public partial class FormPayment : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
        private int paymentId;
        private string selectedImagePath;


        public FormPayment(int id)
        {
            InitializeComponent();

            paymentId = id;
            LoadProofPaymentImage();
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
                    PBProfPayment.SizeMode = PictureBoxSizeMode.Zoom;
                    selectedImagePath = openFileDialog.FileName;

                }
                catch (Exception ex)
                {
                    // Display an error message if something goes wrong
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FormPayment_Load(object sender, EventArgs e)
        {
            LoadProofPaymentImage();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            SaveProofPayments();
        }

        private void SaveProofPayments()
        {
            if (PBProfPayment.Image != null && !string.IsNullOrEmpty(selectedImagePath))
            {
                try
                {
                    byte[] imageBytes;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        PBProfPayment.Image.Save(ms, PBProfPayment.Image.RawFormat);
                        imageBytes = ms.ToArray();
                    }

                    string query = "UPDATE payments SET proof_of_payment = @image, proof_image_path = @path WHERE payment_id = @id";

                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@image", imageBytes);
                            cmd.Parameters.AddWithValue("@path", selectedImagePath);
                            cmd.Parameters.AddWithValue("@id", paymentId);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Proof of payment uploaded successfully!");
                            }
                            else
                            {
                                MessageBox.Show("Failed to upload proof of payment.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please upload an image first.");
            }
        }

        private void LoadProofPaymentImage()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT proof_image_path FROM payments WHERE payment_id = @PaymentId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PaymentId", paymentId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {
                                    string imagePath = reader["proof_image_path"].ToString();

                                    if (File.Exists(imagePath))
                                    {
                                        PBProfPayment.Image = Image.FromFile(imagePath);
                                        PBProfPayment.SizeMode = PictureBoxSizeMode.Zoom;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Image file does not exist at the specified path.");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No image path found in the database.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("No data found for the provided payment_id.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading proof image: " + ex.Message);
            }
        }

       

    }
}
