using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Enrollment_System
{
    public partial class StudentHistory : Form
    {
        private readonly string connectionString = DatabaseConfig.ConnectionString;
        public int EnrollmentId { get; set; }
        public string StudentName { get; set; }

        public StudentHistory()
        {
            InitializeComponent();
            InitializeDataGridView();
            StyleDataGridHistory();
            this.Load += StudentHistory_Load;

        }

        private void InitializeDataGridView()
        {
            DataGridHistory.AutoGenerateColumns = false;
            DataGridHistory.Columns.Clear(); // Clear existing just in case

            DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn
            {
                Name = "no_enrollment",
                HeaderText = "No.",
                DataPropertyName = "no_enrollment", // now bound to manually created column
                Width = 50,
                ReadOnly = true
            };


            DataGridViewTextBoxColumn colYear = new DataGridViewTextBoxColumn
            {
                Name = "academic_year",
                HeaderText = "Academic Year",
                DataPropertyName = "academic_year",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            DataGridViewTextBoxColumn colSem = new DataGridViewTextBoxColumn
            {
                Name = "semester",
                HeaderText = "Semester",
                DataPropertyName = "semester",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            DataGridViewTextBoxColumn colLevel = new DataGridViewTextBoxColumn
            {
                Name = "year_level",
                HeaderText = "Year Level",
                DataPropertyName = "year_level",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            DataGridViewTextBoxColumn colPrev = new DataGridViewTextBoxColumn
            {
                Name = "previous_section",
                HeaderText = "Previous Section",
                DataPropertyName = "previous_section",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            DataGridViewTextBoxColumn colCurr = new DataGridViewTextBoxColumn
            {
                Name = "current_section",
                HeaderText = "Current Section",
                DataPropertyName = "current_section",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            DataGridViewTextBoxColumn colDate = new DataGridViewTextBoxColumn
            {
                Name = "effective_date",
                HeaderText = "Effective Date",
                DataPropertyName = "effective_date",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            DataGridViewTextBoxColumn colChangedBy = new DataGridViewTextBoxColumn
            {
                Name = "changed_by",
                HeaderText = "Changed By",
                DataPropertyName = "changed_by",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            // Add columns to the DataGridView
            DataGridHistory.Columns.AddRange(new DataGridViewColumn[]
            {
                colNo, colYear, colSem, colLevel, colPrev, colCurr, colDate, colChangedBy
            });

            // Center align all columns except the first one
            for (int i = 1; i < DataGridHistory.Columns.Count; i++)
            {
                DataGridHistory.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            DataGridHistory.RowTemplate.Height = 35;
            DataGridHistory.AllowUserToResizeColumns = false;
            DataGridHistory.AllowUserToResizeRows = false;
            DataGridHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            DataGridHistory.Sorted += DataGridHistory_Sorted;
        }

        private void StyleDataGridHistory()
        {
            DataGridHistory.BorderStyle = BorderStyle.None;

            // Alternating row colors
            DataGridHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);
            DataGridHistory.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridHistory.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            // Selection style
            DataGridHistory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridHistory.DefaultCellStyle.SelectionForeColor = Color.White;

            // Header style
            DataGridHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridHistory.EnableHeadersVisualStyles = false;

            // Grid lines
            DataGridHistory.GridColor = Color.BurlyWood;

            // Font
            DataGridHistory.DefaultCellStyle.Font = new Font("Segoe UI", 10);
        }

        private void StudentHistory_Load(object sender, EventArgs e)
        {
            if (this.EnrollmentId > 0)
            {
                LoadAcademicHistory();
                this.Text = $"Academic History - {this.StudentName}";
            }
        }

        private void DataGridHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Optional: Handle cell click if needed
        }

        public void LoadAcademicHistory()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                SELECT 
                    ah.history_id,
                    se.academic_year,
                    se.semester,
                    se.year_level,
                    ah.previous_section,
                    ah.current_section,
                    ah.effective_date,
                    u.email AS changed_by
                FROM academic_history ah
                JOIN users u ON ah.changed_by = u.user_id
                JOIN student_enrollments se ON ah.enrollment_id = se.enrollment_id
                WHERE ah.enrollment_id = @enrollmentId
                ORDER BY ah.effective_date DESC;
            ";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@enrollmentId", this.EnrollmentId);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Add "no_enrollment" column manually for row numbers
                            dt.Columns.Add("no_enrollment", typeof(int));

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["no_enrollment"] = i + 1;
                            }

                            DataGridHistory.DataSource = null;
                            DataGridHistory.DataSource = dt;
                            DataGridHistory.Refresh();

                            if (!string.IsNullOrEmpty(this.StudentName))
                            {
                                this.Text = $"Academic History - {this.StudentName}";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading academic history: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void DataGridHistory_Sorted(object sender, EventArgs e)
        {
            UpdateRowNumbers();
        }

        private void UpdateRowNumbers()
        {
            if (DataGridHistory.Rows.Count == 0) return;

            for (int i = 0; i < DataGridHistory.Rows.Count; i++)
            {
                if (DataGridHistory.Rows[i].IsNewRow) continue;
                DataGridHistory.Rows[i].Cells["no_enrollment"].Value = (i + 1).ToString();
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
