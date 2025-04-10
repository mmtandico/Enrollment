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

            DataGridEnrollment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridEnrollment.Columns["ColOpen"].Width = 50; 
            DataGridEnrollment.Columns["ColClose"].Width = 50;
            DataGridEnrollment.Columns["ColOpen"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridEnrollment.Columns["ColClose"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridEnrollment.RowTemplate.Height = 40;
            DataGridEnrollment.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

    

            DataGridEnrollment.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridEnrollment.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewImageColumn colOpen = (DataGridViewImageColumn)DataGridEnrollment.Columns["ColOpen"];
            colOpen.ImageLayout = DataGridViewImageCellLayout.Zoom;

            DataGridViewImageColumn colClose = (DataGridViewImageColumn)DataGridEnrollment.Columns["ColClose"];
            colClose.ImageLayout = DataGridViewImageCellLayout.Zoom;

            foreach (DataGridViewColumn col in DataGridEnrollment.Columns)
            {
                col.Frozen = false; 
                col.Resizable = DataGridViewTriState.True; 
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
            if (e.RowIndex >= 0)
            {
                
                if (DataGridEnrollment.Columns[e.ColumnIndex].Name == "ColOpen")
                {
                    
                    using (FormNewAcademiccs editForm = new FormNewAcademiccs())
                    {
                        editForm.StartPosition = FormStartPosition.CenterParent;
                        DialogResult result = editForm.ShowDialog();

                        
                        if (result == DialogResult.OK)
                        {
                            LoadEnrollmentData();
                        }
                    }
                }

                
                else if (DataGridEnrollment.Columns[e.ColumnIndex].Name == "ColClose")
                {
                    DialogResult confirmResult = MessageBox.Show("Are you sure you want to drop this enrollment?",
                                                              "Confirm Deletion",
                                                              MessageBoxButtons.YesNo,
                                                              MessageBoxIcon.Warning);

                    if (confirmResult == DialogResult.Yes)
                    {
                        string enrollmentId = DataGridEnrollment.Rows[e.RowIndex].Cells["enrollment_id"].Value.ToString();
                        string studentName = $"{DataGridEnrollment.Rows[e.RowIndex].Cells["last_name"].Value}, {DataGridEnrollment.Rows[e.RowIndex].Cells["first_name"].Value}";

                        try
                        {
                            
                            bool isDeleted = DeleteEnrollment(enrollmentId);

                            if (isDeleted)
                            {
                                MessageBox.Show($"Enrollment for {studentName} dropped successfully.",
                                              "Deleted",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Information);

                              
                                DataGridEnrollment.Rows.RemoveAt(e.RowIndex);
                            }
                            else
                            {
                                MessageBox.Show("Failed to drop enrollment.",
                                              "Error",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error dropping enrollment: {ex.Message}",
                                          "Error",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private bool DeleteEnrollment(string enrollmentId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=localhost;database=PDM_Enrollment_DB;user=root;password=;"))
                {
                    conn.Open();

                    string query = "DELETE FROM student_enrollments WHERE enrollment_id = @EnrollmentId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EnrollmentId", enrollmentId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error deleting enrollment: " + ex.Message);
            }
        }
    }
}