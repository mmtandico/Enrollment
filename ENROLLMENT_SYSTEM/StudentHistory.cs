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
    public partial class StudentHistory : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
        public int EnrollmentId { get; set; }
        public string StudentName { get; set; }

        public StudentHistory()
        {
            InitializeComponent();

            DataGridHistory.Sorted += DataGridHistory_Sorted;

            foreach (DataGridViewColumn col in DataGridHistory.Columns)
            {
                col.Frozen = false;
            }
            DataGridHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridHistory.Columns["ColOpen1"].Width = 50;
            DataGridHistory.Columns["ColOpen1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridHistory.RowTemplate.Height = 40;
            DataGridHistory.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;


            DataGridHistory.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewImageColumn colOpen2 = (DataGridViewImageColumn)DataGridHistory.Columns["ColOpen1"];
            colOpen2.ImageLayout = DataGridViewImageCellLayout.Zoom;

            foreach (DataGridViewColumn col in DataGridHistory.Columns)
            {
                //col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StyleDataGridHistory()
        {
            DataGridHistory.BorderStyle = BorderStyle.None;

            DataGridHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridHistory.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridHistory.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridHistory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridHistory.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridHistory.EnableHeadersVisualStyles = false;

            DataGridHistory.GridColor = Color.BurlyWood;

            DataGridHistory.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridHistory.RowTemplate.Height = 35;

            DataGridHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn column in DataGridHistory.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }

        }

        private void StudentHistory_Load(object sender, EventArgs e)
        {
            if (this.EnrollmentId > 0)
            {
                LoadAcademicHistory();
            }

            DataGridHistory.AutoGenerateColumns = false;
            history_id.DataPropertyName = "history_id";
            academic_year.DataPropertyName = "academic_yea";
            academic_year.DataPropertyName = "academic_year";
            semester.DataPropertyName = "semester";
            year_level.DataPropertyName = "year_level";
            previous_section.DataPropertyName = "previous_section";
            current_section.DataPropertyName = "current_section";
            effective_date.DataPropertyName = "effective_date";
            changed_by.DataPropertyName = "changed_by";

            DataGridHistory.AllowUserToResizeColumns = false;
            DataGridHistory.AllowUserToResizeRows = false;
            DataGridHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn column in DataGridHistory.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            int totalCols2 = DataGridHistory.Columns.Count;
            DataGridHistory.Columns[totalCols2 - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridHistory.Columns[totalCols2 - 1].Width = 40;

            DataGridHistory.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridHistory.Columns[0].Width = 50;
            DataGridHistory.Columns[1].Width = 50;
            DataGridHistory.Columns[2].Width = 50;
            DataGridHistory.Columns[3].Width = 50;
            DataGridHistory.Columns[4].Width = 50;
            DataGridHistory.Columns[5].Width = 50;
            DataGridHistory.Columns[6].Width = 50;
            DataGridHistory.Columns[7].Width = 100;
            DataGridHistory.Columns[8].Width = 50;
            DataGridHistory.RowTemplate.Height = 35;
            for (int i = 0; i <= 8; i++)
            {
                DataGridHistory.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }



            foreach (DataGridViewColumn col in DataGridHistory.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }

            CustomizeDataGridHistory();
            StyleDataGridHistory();
        }

        private void CustomizeDataGridHistory()
        {
            DataGridHistory.BorderStyle = BorderStyle.None;

            DataGridHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridHistory.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridHistory.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridHistory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridHistory.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridHistory.EnableHeadersVisualStyles = false;

            DataGridHistory.GridColor = Color.BurlyWood;

            DataGridHistory.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridHistory.RowTemplate.Height = 35;

            DataGridHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            foreach (DataGridViewColumn column in DataGridHistory.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }
        }

        private void DataGridHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
                            ah.previous_section,
                            ah.current_section,
                            ah.effective_date,
                            u.email AS changed_by,
                            se.academic_year,
                            se.semester,
                            se.year_level
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
                            DataGridHistory.AutoGenerateColumns = false;

                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataGridHistory.DataSource = dt;
                            UpdateRowNumbers();

                            // Optionally set the form title to include student name
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

        private void UpdateRowNumbers()
        {
            if (DataGridHistory.Rows.Count == 0) return;

            int noColumnIndex = DataGridHistory.Columns[0].Index;

            for (int i = 0; i < DataGridHistory.Rows.Count; i++)
            {
                if (DataGridHistory.Rows[i].IsNewRow) continue;
                DataGridHistory.Rows[i].Cells[noColumnIndex].Value = (i + 1).ToString();
            }
        }

        private void DataGridHistory_Sorted(object sender, EventArgs e)
        {
            UpdateRowNumbers();
        }
    }
}