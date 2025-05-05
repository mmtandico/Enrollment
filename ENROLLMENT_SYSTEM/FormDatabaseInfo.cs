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
        private readonly string connectionString = DatabaseConfig.ConnectionString;


        public FormDatabaseInfo()
        {
            InitializeComponent();
            ApplyButtonEffects();

            if (!string.IsNullOrEmpty(SessionManager.LastName) && !string.IsNullOrEmpty(SessionManager.FirstName))
            {
                LblWelcome.Text = $"{SessionManager.LastName}, {SessionManager.FirstName[0]}.";
            }
            else if (!string.IsNullOrEmpty(SessionManager.LastName))
            {
                LblWelcome.Text = $"{SessionManager.LastName}";
            }
            else if (!string.IsNullOrEmpty(SessionManager.FirstName))
            {
                LblWelcome.Text = $"{SessionManager.FirstName[0]}.";
            }
            else
            {
                LblWelcome.Text = "";
            }
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

            CheckAdminButtonVisibility();

            if (SessionManager.HasRole("cashier") || SessionManager.HasRole("admin"))
            {
                BtnHome.Hide();
                BtnCourses.Hide();
                BtnEnrollment.Hide();
                BtnPI.Hide();
            }

            if (SessionManager.IsLoggedIn)
            {
                LoadUserProfilePic((int)SessionManager.UserId);
            }
        }

        private void CheckAdminButtonVisibility()
        {
            try
            {
                if (!SessionManager.IsLoggedIn)
                {
                    
                    BtnAdmin.Visible = false;
                    BtnStudent.Visible = false;
                    BtnCourse.Visible = false;
                    BtnEnroll.Visible = false;
                    BtnDashB.Visible = false;
                    return;
                }
              
                string userRole = SessionManager.UserRole?.ToString().ToLower();

                if (userRole == "cashier")
                {
                    BtnAdmin.Visible = false;
                    BtnStudent.Visible = false;
                    BtnCourse.Visible = false;
                    BtnEnroll.Visible = true;
                    BtnDashB.Visible = true;

                    AdjustCashierLayout();
                }

                else if (userRole == "admin")
                {
                    BtnAdmin.Visible = false;
                    BtnStudent.Visible = true;
                    BtnCourse.Visible = true;
                    BtnEnroll.Visible = true;
                    BtnDashB.Visible = true;

                    AdjustAdminLayout();
                }

                else
                {
                    BtnAdmin.Visible = true;
                    BtnStudent.Visible = true;
                    BtnCourse.Visible = true;
                    BtnEnroll.Visible = true;
                    BtnDashB.Visible = true;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error checking user role: " + ex.Message);
                RestrictAllButtons();
            }
        }

        private void AdjustCashierLayout()
        {
            
            int startY = BtnDashB.Top; 
            int buttonSpacing = 10; 
            BtnDashB.Top = startY;
            BtnEnroll.Top = BtnDashB.Bottom + buttonSpacing;

           
        }

        private void AdjustAdminLayout()
        {
           
            int startY = BtnDashB.Top;
            int buttonSpacing = 10;

            BtnDashB.Top = startY;
            BtnEnroll.Top = BtnDashB.Bottom + buttonSpacing;
            BtnStudent.Top = BtnEnroll.Bottom + buttonSpacing;
            BtnCourse.Top = BtnStudent.Bottom + buttonSpacing;
          
        }

        private void RestrictAllButtons()
        {
            BtnAdmin.Visible = false;
            BtnStudent.Visible = false;
            BtnCourse.Visible = false;
            BtnEnroll.Visible = false;
            BtnDashB.Visible = true; 
        }

        private void LoadForm(Form form)
        {
            
            foreach (Control control in MAINPANEL.Controls)
            {
                control.Dispose();
            }

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;  
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

        private void BtnAdmin_Click(object sender, EventArgs e)
        {
            LoadForm(new AdminUser());
        }
    }
}
