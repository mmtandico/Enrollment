using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace Enrollment_System
{
    public partial class AdminCashier : Form
    {
        private readonly string connectionString = DatabaseConfig.ConnectionString;

        private readonly int paymentId;
        //public bool IsPaymentValid { get; private set; }
        //public bool IsUniFastPayment { get { return chkUniFast.Checked; } }

        public bool IsPaymentValid
        {
            get
            {
                // If UniFast is checked, no need for payment details
                if (ChkUniFast.Checked) return true;

                // Otherwise, all payment fields must be filled
                return !string.IsNullOrWhiteSpace(TxtReferenceNo.Text) &&
                       !string.IsNullOrWhiteSpace(TxtTotalAmount.Text) &&
                       CmbPaymentMethod.SelectedItem != null;
            }
        }

        public bool IsUniFastPayment => ChkUniFast.Checked;

        public AdminCashier(int id)
        {
            InitializeComponent();
            paymentId = id;
            LoadProofPaymentImage();
            LoadStudentInfo();
            ChkUniFast_CheckedChanged(null, null);
            InitializeComponentCustom();
            TxtReferenceNo.TextChanged += ValidatePaymentFields;
            TxtTotalAmount.TextChanged += ValidatePaymentFields;
            CmbPaymentMethod.SelectedIndexChanged += ValidatePaymentFields;
            ChkUniFast.CheckedChanged += ValidatePaymentFields;
        }

        private void InitializeComponentCustom()
        {
            TxtTotalAmount.KeyPress += TxtTotalAmount_KeyPress;
        }

        private void ExitButton_Click(object sender, EventArgs e) => this.Close();

        private void LoadProofPaymentImage()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                using (var cmd = new MySqlCommand(
                    "SELECT proof_image_path FROM payments WHERE payment_id = @paymentId", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@paymentId", paymentId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() && !reader.IsDBNull(0))
                        {
                            string imagePath = reader["proof_image_path"].ToString();

                            if (File.Exists(imagePath))
                            {
                                PBProofOfPayment.Image = Image.FromFile(imagePath);
                                PBProofOfPayment.SizeMode = PictureBoxSizeMode.Zoom;
                            }
                            else
                            {
                                MessageBox.Show("Image file does not exist at the specified path.",
                                    "Image Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No proof of payment found for this payment.",
                                "Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading proof image: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStudentInfo()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                using (var cmd = new MySqlCommand(
                    @"SELECT 
                        s.student_no,
                        s.last_name,
                        s.first_name,
                        s.middle_name,
                        c.course_code,
                        (
                            SELECT SUM(sb.units)
                            FROM course_subjects cs
                            INNER JOIN subjects sb ON cs.subject_id = sb.subject_id
                            WHERE cs.course_id = se.course_id
                              AND cs.semester = se.semester
                              AND cs.year_level = se.year_level
                        ) AS total_units,
                        p.receipt_no,
                        p.payment_method,
                        p.amount_paid,
                        p.remarks,
                        p.is_unifast,
                        p.payment_status
                    FROM payments p
                    INNER JOIN student_enrollments se ON p.enrollment_id = se.enrollment_id
                    INNER JOIN students s ON se.student_id = s.student_id
                    INNER JOIN courses c ON se.course_id = c.course_id
                    WHERE p.payment_id = @paymentId", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@paymentId", paymentId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            TxtStudentNo.Text = reader["student_no"].ToString();
                            TxtLastName.Text = reader["last_name"].ToString();
                            TxtFirstName.Text = reader["first_name"].ToString();
                            TxtMiddleName.Text = reader["middle_name"].ToString();
                            TxtCourseCode.Text = reader["course_code"].ToString();
                            TxtTotalUnits.Text = reader["total_units"] != DBNull.Value ? reader["total_units"].ToString() : "0";
                            TxtReferenceNo.Text = reader["receipt_no"]?.ToString();

                            string method = reader["payment_method"]?.ToString();
                            if (!string.IsNullOrEmpty(method) && CmbPaymentMethod.Items.Contains(method))
                            {
                                CmbPaymentMethod.SelectedItem = method;
                            }

                            TxtTotalAmount.Text = reader["amount_paid"] != DBNull.Value ? reader["amount_paid"].ToString() : "";
                            TxtRemarks.Text = reader["remarks"]?.ToString();

                            bool isUniFast = reader["is_unifast"] != DBNull.Value && Convert.ToBoolean(reader["is_unifast"]);
                            ChkUniFast.Checked = isUniFast;
                            
                        }
                        else
                        {
                            MessageBox.Show("No student info found for this payment.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student info: " + ex.Message);
            }
        }
        

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ChkUniFast.Checked &&
                (string.IsNullOrWhiteSpace(TxtReferenceNo.Text) ||
                string.IsNullOrWhiteSpace(TxtTotalAmount.Text) ||
                CmbPaymentMethod.SelectedItem == null))
            {
                MessageBox.Show("Please fill in all payment details.");
                return;
            }

            if (ChkUniFast.Checked)
            {
                var confirm = MessageBox.Show("Are you sure you want to mark this as a UniFast payment?",
                    "Confirm UniFast Payment",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string status = ChkUniFast.Checked ? "UNIFAST COMPLETED" : "COMPLETED";
                    string query = $@"UPDATE payments 
                       SET payment_date = NOW(),
                           remarks = @remarks,
                           is_unifast = @isUniFast,
                           payment_status = '{status}'";

                    query += ChkUniFast.Checked
                        ? ", receipt_no = NULL, amount_paid = NULL, payment_method = NULL"
                        : ", receipt_no = @receiptNo, amount_paid = @amountPaid, payment_method = @paymentMethod";

                    query += " WHERE payment_id = @paymentId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@remarks", TxtRemarks.Text.Trim());
                        cmd.Parameters.AddWithValue("@isUniFast", ChkUniFast.Checked);
                        cmd.Parameters.AddWithValue("@paymentId", paymentId);

                        if (!ChkUniFast.Checked)
                        {
                            cmd.Parameters.AddWithValue("@receiptNo", TxtReferenceNo.Text.Trim());
                            cmd.Parameters.AddWithValue("@amountPaid", Convert.ToDecimal(TxtTotalAmount.Text));
                            cmd.Parameters.AddWithValue("@paymentMethod", CmbPaymentMethod.SelectedItem.ToString());
                        }

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            AddPaymentHistory(ChkUniFast.Checked ?
                                "Marked as UniFast payment (COMPLETED)" :
                                "Updated payment details (COMPLETED)");
                            MessageBox.Show("Payment marked as COMPLETED successfully.",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            MessageBox.Show("No changes were made or payment not found.",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid amount.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating payment: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddPaymentHistory(string action)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                using (var cmd = new MySqlCommand(
                    "INSERT INTO payment_history (payment_id, action, admin_id, action_date) " +
                    "VALUES (@paymentId, @action, @adminId, NOW())", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@paymentId", paymentId);
                    cmd.Parameters.AddWithValue("@action", action);
                    cmd.Parameters.AddWithValue("@adminId", SessionManager.UserId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch { /* Silently fail - not critical */ }
        }

        private void ChkUniFast_CheckedChanged(object sender, EventArgs e)
        {
            bool isUniFast = ChkUniFast.Checked;

            TxtReferenceNo.Enabled = !isUniFast;
            TxtTotalAmount.Enabled = !isUniFast;
            CmbPaymentMethod.Enabled = !isUniFast;

            Color disabledColor = Color.LightGray;
            Color enabledColor = SystemColors.Window;

            TxtReferenceNo.BackColor = isUniFast ? disabledColor : enabledColor;
            TxtTotalAmount.BackColor = isUniFast ? disabledColor : enabledColor;
            CmbPaymentMethod.BackColor = isUniFast ? disabledColor : enabledColor;

            if (isUniFast)
            {
                TxtReferenceNo.Text = "";
                TxtTotalAmount.Text = "";
                CmbPaymentMethod.SelectedIndex = -1;
                TxtRemarks.Text = "UniFast Grant Payment";
            }
        }

        private void TxtTotalAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;  
            }
          
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;  
            }
        }

        private void BtnViewHistory_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                using (var cmd = new MySqlCommand(
                    @"SELECT 
                        DATE_FORMAT(action_date, '%Y-%m-%d %H:%i') as date,
                        action,
                        (SELECT username FROM users WHERE user_id = admin_id) as admin
                    FROM payment_history
                    WHERE payment_id = @paymentId
                    ORDER BY action_date DESC", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@paymentId", paymentId);

                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());

                    if (dt.Rows.Count > 0)
                    {
                        string history = "Payment History:\n\n";
                        foreach (DataRow row in dt.Rows)
                        {
                            history += $"{row["date"]} - {row["admin"]} - {row["action"]}\n";
                        }
                        MessageBox.Show(history, "Payment History", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No history found for this payment.", "History", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading history: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ValidatePaymentFields(object sender, EventArgs e)
        {
            if (!ChkUniFast.Checked)
            {
                bool isValid = !string.IsNullOrWhiteSpace(TxtReferenceNo.Text) &&
                              !string.IsNullOrWhiteSpace(TxtTotalAmount.Text) &&
                              CmbPaymentMethod.SelectedItem != null;

                TxtReferenceNo.BackColor = string.IsNullOrWhiteSpace(TxtReferenceNo.Text) ? Color.LightPink : SystemColors.Window;
                TxtTotalAmount.BackColor = string.IsNullOrWhiteSpace(TxtTotalAmount.Text) ? Color.LightPink : SystemColors.Window;
                CmbPaymentMethod.BackColor = CmbPaymentMethod.SelectedItem == null ? Color.LightPink : SystemColors.Window;
            }
        }
    }
}