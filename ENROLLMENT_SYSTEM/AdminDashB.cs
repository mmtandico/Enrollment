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
    public partial class AdminDashB : Form
    {
        private string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";

        public AdminDashB()
        {
            InitializeComponent();
            InitializeDataGridView();
            StyleDataGridAdmin();
            LoadDashboardData();

        }    
          
        private void InitializeDataGridView()
        {
            DataGridAdmin.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridAdmin.RowTemplate.Height = 40;
            DataGridAdmin.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            DataGridAdmin.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridAdmin.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            foreach (DataGridViewColumn col in DataGridAdmin.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
        }

        private void LoadDashboardData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    
                    string currentAcademicYear = $"{DateTime.Now.Year}-{DateTime.Now.Year + 1}";
                    string currentSemester = GetCurrentSemester();

                    
                    LblUserTotal.Text = GetCount(connection, "SELECT COUNT(*) FROM users").ToString();

                    
                    LblStudentTotalNo.Text = GetCount(connection,
                        $@"SELECT COUNT(DISTINCT se.student_id) 
                           FROM student_enrollments se
                           WHERE se.academic_year = '{currentAcademicYear}'
                           AND se.semester = '{currentSemester}'
                           AND se.status = 'Enrolled'").ToString();

                    LblEnrollmentTotal.Text = GetCount(connection,
                        $@"SELECT COUNT(*) 
                           FROM student_enrollments
                           WHERE academic_year = '{currentAcademicYear}'
                           AND semester = '{currentSemester}'
                           AND status = 'Enrolled'").ToString();
                    
                    LblBtledTotal.Text = GetEnrolledCountByCourse(connection, "BTLED", currentAcademicYear, currentSemester);
                    LblBecedTotal.Text = GetEnrolledCountByCourse(connection, "ECED", currentAcademicYear, currentSemester);
                    LblBsoadTotal.Text = GetEnrolledCountByCourse(connection, "BSOAD", currentAcademicYear, currentSemester);
                    LblBshmTotal.Text = GetEnrolledCountByCourse(connection, "BSHM", currentAcademicYear, currentSemester);
                    LblBstmTotal.Text = GetEnrolledCountByCourse(connection, "BSTM", currentAcademicYear, currentSemester);
                    LblBsitTotal.Text = GetEnrolledCountByCourse(connection, "BSIT", currentAcademicYear, currentSemester);
                    LblBscsTotal.Text = GetEnrolledCountByCourse(connection, "BSCS", currentAcademicYear, currentSemester);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetCountsToZero();
            }
        }

        private string GetCurrentSemester()
        {
            
            return DateTime.Now.Month >= 6 ? "1st" : "2nd"; // Assuming 1st sem is June-Nov, 2nd sem is Dec-May
        }

        private string GetEnrolledCountByCourse(MySqlConnection connection, string courseCode, string academicYear, string semester)
        {
            string query = @"SELECT COUNT(*) 
                           FROM student_enrollments se
                           JOIN courses c ON se.course_id = c.course_id
                           WHERE c.course_code = @courseCode
                           AND se.academic_year = @academicYear
                           AND se.semester = @semester
                           AND se.status = 'Enrolled'";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@courseCode", courseCode);
                command.Parameters.AddWithValue("@academicYear", academicYear);
                command.Parameters.AddWithValue("@semester", semester);
                return command.ExecuteScalar().ToString();
            }
        }


        private int GetCount(MySqlConnection connection, string query)
        {
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private void ResetCountsToZero()
        {
            LblBtledTotal.Text = "0";
            LblBecedTotal.Text = "0";
            LblBsoadTotal.Text = "0";
            LblBshmTotal.Text = "0";
            LblBstmTotal.Text = "0";
            LblBsitTotal.Text = "0";
            LblBscsTotal.Text = "0";
            LblUserTotal.Text = "0";
            LblEnrollmentTotal.Text = "0";
            LblStudentTotalNo.Text = "0";
        }


        private void DataGridAdmin_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AdminDashB_Activated(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        private void AdminDashB_Load(object sender, EventArgs e)
        {
            monthCalendar1.Width = 300;
            monthCalendar1.Height = 300;


            DataGridAdmin.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridAdmin.AllowUserToResizeColumns = false;
            DataGridAdmin.AllowUserToResizeRows = false;
            DataGridAdmin.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn column in DataGridAdmin.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            DataGridAdmin.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridAdmin.Columns[0].Width = 50;
        }



        private void StyleDataGridAdmin()
        {

            DataGridAdmin.BorderStyle = BorderStyle.None;


            DataGridAdmin.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);


            DataGridAdmin.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridAdmin.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);


            DataGridAdmin.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridAdmin.DefaultCellStyle.SelectionForeColor = Color.White;


            DataGridAdmin.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33); // Rich brown
            DataGridAdmin.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridAdmin.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridAdmin.EnableHeadersVisualStyles = false;


            DataGridAdmin.GridColor = Color.BurlyWood;


            DataGridAdmin.DefaultCellStyle.Font = new Font("Segoe UI", 10);


            DataGridAdmin.RowTemplate.Height = 35;


            DataGridAdmin.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            foreach (DataGridViewColumn column in DataGridAdmin.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }


        }
    }
}
