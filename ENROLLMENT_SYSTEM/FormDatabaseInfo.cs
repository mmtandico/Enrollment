using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace Enrollment_System
{

    public partial class FormDatabaseInfo : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";

        public FormDatabaseInfo()
        {
            InitializeComponent();
            ApplyButtonEffects();

        }


        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnPI_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormPersonalInfo().Show();
        }

        private void BtnEnrollment_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormEnrollment().Show();
        }

        private void BtnCourses_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormCourse().Show();
        }

        private void BtnHome_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormHome().Show();
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

        private void FormDatabaseInfo_Load(object sender, EventArgs e)
        {
            ApplyButtonEffects();
            LoadForm(new AdminDashB());


            if (SessionManager.IsLoggedIn)
            {
                LoadUserProfilePic((int)SessionManager.UserId);
            }
        }



        // Function to load forms inside MAINPANEL
        private void LoadForm(Form form)
        {
            // Remove any existing form in MAINPANEL
            foreach (Control control in MAINPANEL.Controls)
            {
                control.Dispose();
            }

            // Set the new form as a child of MAINPANEL
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;  // Make the form fill the panel
            MAINPANEL.Controls.Add(form);
            form.Show();
        }

        private void LoadUserProfilePic(int userId)
        {

            string query = "SELECT profile_picture FROM students WHERE user_id = @userId";

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {


                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);


                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["profile_picture"] != DBNull.Value)
                                {
                                    byte[] imageBytes = (byte[])reader["profile_picture"];
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        PBoxLoginUser.Image = Image.FromStream(ms);
                                    }
                                }
                                else
                                {
                                    PBoxLoginUser.Image = Properties.Resources.PROFILE;
                                }

                                PBoxLoginUser.SizeMode = PictureBoxSizeMode.StretchImage;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading profile picture: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PBoxLoginUser.Image = Properties.Resources.PROFILE;
            }
        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ApplyButtonEffects()
        {

        }


        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

        }

        private void MAINPANEL_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DataGridAdmin_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BtnStudent_Click(object sender, EventArgs e)
        {
            LoadForm(new AdminStudents());
        }

        private void BtnDashB_Click(object sender, EventArgs e)
        {
            LoadForm(new AdminDashB());
        }

        private void BtnEnroll_Click(object sender, EventArgs e)
        {
            LoadForm(new AdminEnrollment());
        }

        private void BtnCourse_Click(object sender, EventArgs e)
        {
            LoadForm(new AdminCourse());
        }
    }
}
