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
    public partial class FormNewAcademiccs : Form
    {
        private string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
        private long loggedInUserId;
        public string CourseText
        {
            get { return TxtCourse.Text; }
            set { TxtCourse.Text = value; } // Update TxtCourse with the passed value
        }

        public FormNewAcademiccs()
        {
            InitializeComponent();
            PicBoxID.Image = Properties.Resources.PROFILE;
            PicBoxID.SizeMode = PictureBoxSizeMode.StretchImage;
            loggedInUserId = SessionManager.UserId;
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
                    
                    TxtGradePdfPath.Text = openFileDialog.FileName;
                }
            }
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    PicBoxID.Image = Image.FromFile(openFileDialog.FileName);
                    PicBoxID.SizeMode = PictureBoxSizeMode.StretchImage;

                    //byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);

                    //SaveProfilePicture(imageBytes);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    long studentId = -1;

                    
                    string checkStudentQuery = "SELECT student_id FROM students WHERE user_id = @UserID";
                    using (var checkCmd = new MySqlCommand(checkStudentQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        object result = checkCmd.ExecuteScalar();

                        if (result != null)
                        {
                            studentId = Convert.ToInt64(result);
                        }
                        else
                        {
                            MessageBox.Show("Student not found. Please check your user account.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    
                    int courseId = GetCourseIdFromText(CmbCourse.SelectedItem.ToString());

                    
                    string enrollmentQuery = @"
INSERT INTO student_enrollments 
    (student_id, course_id, academic_year, semester, year_level, grade_pdf_path, status)
VALUES 
    (@StudentID, @CourseID, @AcademicYear, @Semester, @YearLevel, @GradePdfPath, @Status)
ON DUPLICATE KEY UPDATE 
    academic_year = VALUES(academic_year),
    semester = VALUES(semester),
    year_level = VALUES(year_level),
    grade_pdf_path = VALUES(grade_pdf_path),
    status = VALUES(status)";

                    ExecuteQuery(conn, enrollmentQuery,
                        new MySqlParameter("@StudentID", studentId),
                        new MySqlParameter("@CourseID", courseId),
                        new MySqlParameter("@AcademicYear", TxtSchoolYear.Text),
                        new MySqlParameter("@Semester", CmbSem.SelectedItem?.ToString().Split(' ')[0]),
                        new MySqlParameter("@YearLevel", CmbYrLvl.SelectedItem?.ToString().Split(' ')[0]),
                        new MySqlParameter("@GradePdfPath", TxtGradePdfPath.Text),
                        new MySqlParameter("@Status", "Pending")
                    );

                    MessageBox.Show("Enrollment information updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUserData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormNewAcademiccs_Load(object sender, EventArgs e)
        {
            LoadUserData();
            LoadCourseList();
            LoadUserData();
            LoadCourseList();

            // Select course if it was set by previous form (via SessionManager)
            if (!string.IsNullOrEmpty(SessionManager.SelectedCourse) &&
                CmbCourse.Items.Contains(SessionManager.SelectedCourse))
            {
                CmbCourse.SelectedItem = SessionManager.SelectedCourse;
                SessionManager.SelectedCourse = null;
            }

        }

        private void LoadCourseList()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT course_name FROM courses ORDER BY course_name";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            CmbCourse.Items.Clear();

                            if (!reader.HasRows)
                            {
                                MessageBox.Show("No courses found in the database.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            while (reader.Read())
                            {
                                string courseName = reader["course_name"].ToString();
                                CmbCourse.Items.Add(courseName);
                            }

                            if (CmbCourse.Items.Count == 0)
                            {
                                MessageBox.Show("No courses were added to the ComboBox.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading course list: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadUserData()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                   
                    string checkUserQuery = "SELECT COUNT(*) FROM students WHERE user_id = @UserID";
                    using (var checkCmd = new MySqlCommand(checkUserQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (userExists == 0)
                        {
                            string insertQuery = @"
                    INSERT INTO students (user_id, student_no, student_lrn, first_name, middle_name, last_name, birth_date, age, sex, civil_status, nationality) 
                    VALUES (@UserID, '', '', '', '', '', '2000-01-01', 0, 'Male', '', '')";

                            using (var insertCmd = new MySqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    
                    string query = @"
            SELECT 
                s.student_id, 
                IFNULL(s.student_no, '') AS student_no, 
                IFNULL(s.first_name, '') AS first_name, 
                IFNULL(s.middle_name, '') AS middle_name, 
                IFNULL(s.last_name, '') AS last_name, 
                IFNULL(s.birth_date, '2000-01-01') AS birth_date, 
                IFNULL(s.age, 0) AS age, 
                IFNULL(s.sex, 'Unknown') AS sex, 
                IFNULL(s.civil_status, '') AS civil_status, 
                IFNULL(s.nationality, '') AS nationality, 
                s.profile_picture,
                se.academic_year, 
                se.semester, 
                se.year_level, 
                se.status,
                c.course_code,
                c.course_name
            FROM students s
            LEFT JOIN student_enrollments se ON s.student_id = se.student_id
            LEFT JOIN courses c ON se.course_id = c.course_id
            WHERE s.user_id = @UserID
            ORDER BY s.student_id DESC LIMIT 1";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                TxtStudentNo.Text = reader["student_no"].ToString();
                                TxtFirstName.Text = reader["first_name"].ToString();
                                TxtMiddleName.Text = reader["middle_name"].ToString();
                                TxtLastName.Text = reader["last_name"].ToString();
                                TxtSchoolYear.Text = reader["academic_year"].ToString();
                                CmbSem.Text = reader["semester"].ToString();
                                CmbYrLvl.Text = reader["year_level"].ToString();
                                CmbCourse.Text = reader["course_name"].ToString();

                                if (reader["profile_picture"] != DBNull.Value)
                                {
                                    byte[] imageBytes = (byte[])reader["profile_picture"];
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        PicBoxID.Image = Image.FromStream(ms);
                                    }
                                }
                                else
                                {
                                    PicBoxID.Image = Properties.Resources.PROFILE;
                                }
                                PicBoxID.SizeMode = PictureBoxSizeMode.StretchImage;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetText(string message)
        {
            TxtCourse.Text = message;
        }

        private void TxtCourse_TextChanged(object sender, EventArgs e)
        {
            
        }

        private int GetCourseIdFromText(string courseText)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT course_id FROM courses WHERE course_name = @CourseName";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseName", courseText);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        int courseId;
                        if (int.TryParse(result.ToString(), out courseId))
                        {
                            return courseId;
                        }
                        else
                        {
                            throw new Exception("Invalid course ID format.");
                        }
                    }
                    else
                    {
                        throw new Exception("Course ID not found for the provided course name.");
                    }
                }
            }
        }


        private void ExecuteQuery(MySqlConnection conn, string query, params MySqlParameter[] parameters)
        {
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddRange(parameters);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
