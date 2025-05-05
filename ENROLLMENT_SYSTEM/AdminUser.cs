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
using BCrypt.Net;

namespace Enrollment_System
{
    public partial class AdminUser : Form
    {
        private readonly string connectionString = DatabaseConfig.ConnectionString;


        public AdminUser()
        {
            InitializeComponent();
            InitializeDataGridView();
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;

            DataGridAdmins.DataBindingComplete += (s, e) => UpdateRowNumbersAdmins();
            DataGridUsers.DataBindingComplete += (s, e) => UpdateRowNumbersUsers();

            LoadAdmins();
            LoadUsers();
            DataGridAdmins.Sorted += DataGridAdmins_Sorted;
            DataGridUsers.Sorted += DataGridUsers_Sorted;

        }

        private void AdminUser_Load(object sender, EventArgs e)
        {
            DataGridAdmins.CellContentClick += DataGridAdmins_CellContentClick;
            DataGridUsers.CellContentClick += DataGridUsers_CellContentClick;
            DataGridAdmins.Sorted += DataGridAdmins_Sorted;
            DataGridUsers.Sorted += DataGridUsers_Sorted;
            //admins
            DataGridAdmins.AutoGenerateColumns = false;
            user_id.DataPropertyName = "user_id";
            email.DataPropertyName = "email";
            password.DataPropertyName = "password";
            role.DataPropertyName = "role";
            is_verified.DataPropertyName = "is_verified";
            created_at.DataPropertyName = "created_at";

            //user
            DataGridUsers.AutoGenerateColumns = false;
            user_id_ol.DataPropertyName = "user_id";
            email_ol.DataPropertyName = "email";
            password_ol.DataPropertyName = "password";
            role_ol.DataPropertyName = "role";
            is_verified_ol.DataPropertyName = "is_verified";
            created_at_ol.DataPropertyName = "created_at";

          

            DataGridUsers.AllowUserToResizeColumns = false;
            DataGridUsers.AllowUserToResizeRows = false;
            DataGridUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn column in DataGridUsers.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            int totalCols1 = DataGridUsers.Columns.Count;
            DataGridUsers.Columns[totalCols1 - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridUsers.Columns[totalCols1 - 1].Width = 40;
            DataGridUsers.Columns[totalCols1 - 2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridUsers.Columns[totalCols1 - 2].Width = 40;
            DataGridUsers.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridUsers.Columns[0].Width = 50;
            DataGridUsers.Columns[1].Width = 50;
            DataGridUsers.Columns[2].Width = 250;
            DataGridUsers.Columns[3].Width = 300;
            DataGridUsers.Columns[4].Width = 75;
            DataGridUsers.Columns[5].Width = 75;
            DataGridUsers.Columns[6].Width = 125;
            DataGridUsers.RowTemplate.Height = 35;
            DataGridUsers.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridUsers.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridUsers.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridUsers.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridUsers.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;



            foreach (DataGridViewColumn col in DataGridUsers.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
            ////////////////////////////////////////////////
            DataGridAdmins.AllowUserToResizeColumns = false;
            DataGridAdmins.AllowUserToResizeRows = false;
            DataGridAdmins.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn column in DataGridAdmins.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            int totalCols = DataGridAdmins.Columns.Count;
            DataGridAdmins.Columns[totalCols - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridAdmins.Columns[totalCols - 1].Width = 40;
            DataGridAdmins.Columns[totalCols - 2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridAdmins.Columns[totalCols - 2].Width = 40;
            DataGridAdmins.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridAdmins.Columns[0].Width = 50;
            DataGridAdmins.Columns[1].Width = 50;
            DataGridAdmins.Columns[2].Width = 250;
            DataGridAdmins.Columns[3].Width = 300;
            DataGridAdmins.Columns[4].Width = 75;
            DataGridAdmins.Columns[5].Width = 75;
            DataGridAdmins.Columns[6].Width = 125;
            DataGridAdmins.RowTemplate.Height = 35;
            DataGridAdmins.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridAdmins.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridAdmins.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridAdmins.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridAdmins.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            foreach (DataGridViewColumn col in DataGridAdmins.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }

            CustomizeDataGridUsers();
            CustomizeDataGridAdmins();
            StyleTwoTabControl();
        }

        private void InitializeDataGridView()
        {
            foreach (DataGridViewColumn col in DataGridUsers.Columns)
            {
                col.Frozen = false;
            }
            DataGridUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridUsers.Columns["ColOpen2"].Width = 50;
            DataGridUsers.Columns["ColClose2"].Width = 50;
            DataGridUsers.Columns["ColOpen2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridUsers.Columns["ColClose2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridUsers.RowTemplate.Height = 40;
            DataGridUsers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;


            DataGridUsers.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridUsers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewImageColumn colOpen1 = (DataGridViewImageColumn)DataGridUsers.Columns["ColOpen2"];
            colOpen1.ImageLayout = DataGridViewImageCellLayout.Zoom;

            DataGridViewImageColumn colClose1 = (DataGridViewImageColumn)DataGridUsers.Columns["ColClose2"];
            colClose1.ImageLayout = DataGridViewImageCellLayout.Zoom;

            foreach (DataGridViewColumn col in DataGridUsers.Columns)
            {
                //col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
            //////////////////////////
            foreach (DataGridViewColumn col in DataGridAdmins.Columns)
            {
                col.Frozen = false;
                //col.Resizable = DataGridViewTriState.True;
            }
            DataGridAdmins.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridAdmins.Columns["ColOpen"].Width = 50;
            DataGridAdmins.Columns["ColClose"].Width = 50;
            DataGridAdmins.Columns["ColOpen"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridAdmins.Columns["ColClose"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridAdmins.RowTemplate.Height = 40;
            DataGridAdmins.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;


            DataGridAdmins.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridAdmins.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewImageColumn colOpen = (DataGridViewImageColumn)DataGridAdmins.Columns["ColOpen"];
            colOpen.ImageLayout = DataGridViewImageCellLayout.Zoom;

            DataGridViewImageColumn colClose = (DataGridViewImageColumn)DataGridAdmins.Columns["ColClose"];
            colClose.ImageLayout = DataGridViewImageCellLayout.Zoom;

            foreach (DataGridViewColumn col in DataGridAdmins.Columns)
            {
                col.Frozen = false;
                col.Resizable = DataGridViewTriState.True;
            }
        }

        private void StyleTwoTabControl()
        {
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.ItemSize = new Size(160, 36);

            Color darkBrown = Color.FromArgb(94, 55, 30);
            Color mediumBrown = Color.FromArgb(139, 69, 19);
            Color lightBrown = Color.FromArgb(210, 180, 140);
            Color goldYellow = Color.FromArgb(218, 165, 32);
            Color cream = Color.FromArgb(253, 245, 230);

            tabControl1.DrawItem += (sender, e) =>
            {
                Graphics g = e.Graphics;
                TabPage currentTab = tabControl1.TabPages[e.Index];
                Rectangle tabRect = tabControl1.GetTabRect(e.Index);
                bool isSelected = tabControl1.SelectedIndex == e.Index;


                if (isSelected)
                {
                    tabRect.Inflate(0, 2);
                    tabRect.Y -= 2;
                }


                if (isSelected)
                {
                    using (var brush = new LinearGradientBrush(
                        tabRect,
                        Color.FromArgb(255, 230, 170),
                        goldYellow,
                        LinearGradientMode.Vertical))
                    {
                        g.FillRectangle(brush, tabRect);
                    }
                }
                else
                {
                    using (var brush = new SolidBrush(mediumBrown))
                    {
                        g.FillRectangle(brush, tabRect);
                    }
                }


                using (var pen = new Pen(isSelected ? goldYellow : darkBrown, isSelected ? 2f : 1f))
                {
                    g.DrawRectangle(pen, tabRect);
                }


                TextRenderer.DrawText(
                    g,
                    currentTab.Text,
                    new Font(tabControl1.Font.FontFamily, 9f,
                            isSelected ? FontStyle.Bold : FontStyle.Regular),
                    tabRect,
                    isSelected ? darkBrown : Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);


                if (isSelected)
                {
                    using (var pen = new Pen(goldYellow, 2))
                    {
                        int underlineY = tabRect.Bottom - 3;
                        g.DrawLine(pen, tabRect.Left + 10, underlineY,
                                    tabRect.Right - 10, underlineY);
                    }
                }
            };
        }

        private void CustomizeDataGridUsers()
        {
            DataGridUsers.BorderStyle = BorderStyle.None;

            DataGridUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridUsers.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridUsers.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridUsers.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridUsers.EnableHeadersVisualStyles = false;

            DataGridUsers.GridColor = Color.BurlyWood;

            DataGridUsers.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridUsers.RowTemplate.Height = 35;

            DataGridUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            foreach (DataGridViewColumn column in DataGridUsers.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }
        }

        private void CustomizeDataGridAdmins()
        {
            DataGridAdmins.BorderStyle = BorderStyle.None;

            DataGridAdmins.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);

            DataGridAdmins.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 240);
            DataGridAdmins.RowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 34, 20);

            DataGridAdmins.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 165, 32);
            DataGridAdmins.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridAdmins.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(101, 67, 33);
            DataGridAdmins.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridAdmins.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            DataGridAdmins.EnableHeadersVisualStyles = false;

            DataGridAdmins.GridColor = Color.BurlyWood;

            DataGridAdmins.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            DataGridAdmins.RowTemplate.Height = 35;

            DataGridAdmins.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            foreach (DataGridViewColumn column in DataGridAdmins.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
            }
        }

        private void UpdateRowNumbersAdmins()
        {
            if (DataGridAdmins.Rows.Count == 0) return;

            int noColumnIndex = DataGridAdmins.Columns["user_id"].Index - 1; 

            for (int i = 0; i < DataGridAdmins.Rows.Count; i++)
            {
                if (DataGridAdmins.Rows[i].IsNewRow) continue;
                DataGridAdmins.Rows[i].Cells[0].Value = (i + 1).ToString(); 
            }
        }

        private void UpdateRowNumbersUsers()
        {
            if (DataGridUsers.Rows.Count == 0) return;

            int noColumnIndex = DataGridUsers.Columns["user_id_ol"].Index - 1; 

            for (int i = 0; i < DataGridUsers.Rows.Count; i++)
            {
                if (DataGridUsers.Rows[i].IsNewRow) continue;
                DataGridUsers.Rows[i].Cells[0].Value = (i + 1).ToString(); 
            }
        }

        private void DataGridAdmins_Sorted(object sender, EventArgs e)
        {
            UpdateRowNumbersAdmins();
        }

        private void DataGridUsers_Sorted(object sender, EventArgs e)
        {
            UpdateRowNumbersUsers();
        }

        private void LoadAdmins()
        {
            try
            {
                
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT user_id, email, password_hash as password, role, is_verified, created_at FROM users where role = 'admin'";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        DataTable dataTable = new DataTable();

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            DataGridAdmins.AutoGenerateColumns = false;
                            DataTable dt = new DataTable();
                            adapter.Fill(dataTable);
                            DataGridAdmins.DataSource = dataTable;
                            DataGridAdmins.Refresh();
                            UpdateRowNumbersAdmins();
                        }
                        //DataGridAdmins.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void LoadUsers()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT user_id, email, password_hash as password, role, is_verified, created_at FROM users where role = 'user'";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        DataTable dataTable = new DataTable();

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            DataGridUsers.AutoGenerateColumns = false;
                            DataTable dt = new DataTable();
                            adapter.Fill(dataTable);
                            DataGridUsers.DataSource = dataTable;
                            DataGridUsers.Refresh();
                            UpdateRowNumbersUsers();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void PopulateFields(DataGridViewRow row)
        {
            try
            {
                
                bool isUsersGrid = row.DataGridView == DataGridUsers;

                string userIdColumn = isUsersGrid ? "user_id_ol" : "user_id";
                string emailColumn = isUsersGrid ? "email_ol" : "email";
                string roleColumn = isUsersGrid ? "role_ol" : "role";
                string verifiedColumn = isUsersGrid ? "is_verified_ol" : "is_verified";

                TxtUserID.Text = row.Cells[userIdColumn]?.Value?.ToString() ?? "";
                TxtEmail.Text = row.Cells[emailColumn]?.Value?.ToString() ?? "";
                TxtPass.Text = "***********";

                CmbRole.Text = row.Cells[roleColumn]?.Value?.ToString() ?? "";

                var isVerifiedCell = row.Cells[verifiedColumn];
                bool isVerified = isVerifiedCell?.Value != null && isVerifiedCell.Value != DBNull.Value && Convert.ToBoolean(isVerifiedCell.Value);
                CmbVerified.Text = isVerified ? "Yes" : "No";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading details: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridAdmins_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = DataGridAdmins.Rows[e.RowIndex];


            if (e.ColumnIndex != DataGridAdmins.Columns["ColOpen"].Index &&
                e.ColumnIndex != DataGridAdmins.Columns["ColClose"].Index)
            {

                PopulateFields(row);
            }


            if (e.ColumnIndex == DataGridAdmins.Columns["ColOpen"].Index)
            {

                PopulateFields(row);
            }


            if (e.ColumnIndex == DataGridAdmins.Columns["ColClose"].Index)
            {

                if (MessageBox.Show("Are you sure you want to delete this admin?", "Confirm Delete",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int userId = Convert.ToInt32(row.Cells["user_id"].Value);
                }
            }
        }


        private void DeleteAdmins(int subjectId)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string deleteLinkQuery = "DELETE FROM course_subjects WHERE subject_id = @id";
                    using (var linkCmd = new MySqlCommand(deleteLinkQuery, conn))
                    {
                        linkCmd.Parameters.AddWithValue("@id", subjectId);
                        linkCmd.ExecuteNonQuery();
                    }

                    string deleteQuery = "DELETE FROM subjects WHERE subject_id = @id";
                    using (var cmd = new MySqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", subjectId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Subject deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadAdmins();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting subject: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            TxtUserID.Clear();
            //TxtPass.Clear();
            //TxtEmail.Clear();
            //CmbRole.SelectedIndex = -1;
           // CmbVerified.SelectedIndex = -1;
        }
        
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                

                int userId = Convert.ToInt32(TxtUserID.Text);

                string email = TxtEmail.Text;
                string password = TxtPass.Text;
                string role = CmbRole.Text;
                bool isVerified = CmbVerified.Text == "Yes";

                string hashedPassword = string.IsNullOrEmpty(password) ? null : BCrypt.Net.BCrypt.HashPassword(password);

                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "UPDATE users SET email = @Email, password_hash = @PasswordHash, role = @Role, is_verified = @IsVerified WHERE user_id = @UserId";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@IsVerified", isVerified);
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("User updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAdmins();
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click_1(object sender, EventArgs e)
        {
            try
            {
                

                int userId = Convert.ToInt32(TxtUserID.Text);

                if (MessageBox.Show("Are you sure you want to delete this user?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        string query = "DELETE FROM users WHERE user_id = @UserId";
                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("User deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadAdmins();
                    LoadUsers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string email = TxtEmail.Text;
                string password = TxtPass.Text;
                string role = CmbRole.Text;
                bool isVerified = CmbVerified.Text == "Yes";

                // Validate inputs
                if (string.IsNullOrWhiteSpace(email))
                {
                    MessageBox.Show("Please enter an email address", "Validation Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Please enter a password", "Validation Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO users (email, password_hash, role, is_verified) 
                           VALUES (@Email, @PasswordHash, @Role, @IsVerified);
                           SELECT LAST_INSERT_ID();";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@IsVerified", isVerified);

                        int newUserId = Convert.ToInt32(cmd.ExecuteScalar());
                        TxtUserID.Text = newUserId.ToString();

                        MessageBox.Show($"User added successfully with ID: {newUserId}", "Success",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields();
                        LoadAdmins();
                        LoadUsers();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding user: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = DataGridUsers.Rows[e.RowIndex];

            if (e.ColumnIndex != DataGridUsers.Columns["ColOpen2"].Index &&
                e.ColumnIndex != DataGridUsers.Columns["ColClose2"].Index)
            {
                PopulateFields(row);
            }

            if (e.ColumnIndex == DataGridUsers.Columns["ColOpen2"].Index)
            {
                PopulateFields(row);
            }

            if (e.ColumnIndex == DataGridUsers.Columns["ColClose2"].Index)
            {
                if (MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int userId = Convert.ToInt32(row.Cells["user_id"].Value);
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshCurrentTab();
        }

        private void RefreshCurrentTab()
        {
            if (tabControl1.SelectedTab == tabPage3) // Admins tab
            {
                LoadAdmins();
                DataGridAdmins.Refresh();
                Application.DoEvents(); // Force UI update
                UpdateRowNumbersAdmins();
            }
            else if (tabControl1.SelectedTab == tabPage2) // Users tab
            {
                LoadUsers();
                DataGridUsers.Refresh();
                Application.DoEvents(); // Force UI update
                UpdateRowNumbersUsers();
            }
        }

        private int GetNextUserId()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT AUTO_INCREMENT " +
                                  "FROM information_schema.TABLES " +
                                  "WHERE TABLE_SCHEMA = DATABASE() " +
                                  "AND TABLE_NAME = 'users'";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting next user ID: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = textBox1.Text;

            if (DataGridUsers.DataSource is DataTable)
            {
                DataTable dt = (DataTable)DataGridUsers.DataSource;
                dt.DefaultView.RowFilter = $"CONVERT(email, System.String) LIKE '%{searchTerm}%'";
            }
        }
    }
}
