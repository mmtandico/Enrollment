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

namespace Enrollment_System
{
    public partial class FormEnrollment : Form
    {
        //private FormNewAcademiccs formNewAcademiccsInstance = null;

        private FormCourse mainForm;

        public FormEnrollment()
        {
            InitializeComponent();
            this.Activated += FormEnrollment_Activated;

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

        private void FormEnrollment_Activated(object sender, EventArgs e)
        {
            LoadEnrollmentData();
        }


        private void FormEnrollment_Load(object sender, EventArgs e)
        {
            LoadEnrollmentData();
        }


        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void BtnPI_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormPersonalInfo().Show();
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
        private void BtnAddAcademic_Click(object sender, EventArgs e)
        {
            using (FormNewAcademiccs formNewAcademiccs = new FormNewAcademiccs())
            {
                formNewAcademiccs.StartPosition = FormStartPosition.CenterParent;


                DialogResult result = formNewAcademiccs.ShowDialog();


                if (result == DialogResult.OK)
                {
                    LoadEnrollmentData();
                }
            }
        }

        private void LoadEnrollmentData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=localhost;database=PDM_Enrollment_DB;user=root;password=;"))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            se.enrollment_id,
                            s.student_id,
                            s.student_no,
                            s.last_name,
                            s.first_name,
                            s.middle_name,
                            c.course_code,
                            se.academic_year,
                            se.semester,
                            se.year_level,
                            se.status
                        FROM student_enrollments se
                        INNER JOIN students s ON se.student_id = s.student_id
                        INNER JOIN courses c ON se.course_id = c.course_id
                        WHERE s.user_id = @UserID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", SessionManager.UserId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Clear existing rows before adding new ones
                            DataGridEnrollment.Rows.Clear();

                            while (reader.Read())
                            {
                                DataGridEnrollment.Rows.Add(
                                    reader["enrollment_id"].ToString(),
                                    reader["student_no"].ToString(),
                                    reader["last_name"].ToString(),
                                    reader["first_name"].ToString(),
                                    reader["middle_name"].ToString(),
                                    reader["course_code"].ToString(),
                                    reader["academic_year"].ToString(),
                                    reader["semester"].ToString(),
                                    reader["year_level"].ToString(),
                                    reader["status"].ToString()
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading enrollment data: " + ex.Message);
            }
        }


        private void DataGridEnrollment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
    }
}