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
    public partial class AdminCashier : Form
    {
        private readonly string connectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";
        private int paymentId;
       


        public AdminCashier(int id)
        {
            InitializeComponent();
            paymentId = id;
            LoadProofPaymentImage();
            LoadStudentInfo();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadProofPaymentImage()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT proof_image_path
                FROM payments
                WHERE payment_id = @paymentId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
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
                                    MessageBox.Show("Image file does not exist at the specified path.", "Image Missing",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("No proof of payment found for this payment.", "Data Missing",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading proof image: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadStudentInfo()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                    SELECT 
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
                        p.remarks
                    FROM payments p
                    INNER JOIN student_enrollments se ON p.enrollment_id = se.enrollment_id
                    INNER JOIN students s ON se.student_id = s.student_id
                    INNER JOIN courses c ON se.course_id = c.course_id
                    WHERE p.payment_id = @paymentId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
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
                            }
                            else
                            {
                                MessageBox.Show("No student info found for this payment.");
                            }
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
            if (string.IsNullOrWhiteSpace(TxtReferenceNo.Text) ||
                string.IsNullOrWhiteSpace(TxtTotalAmount.Text) ||
                CmbPaymentMethod.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all payment details.");
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        UPDATE payments 
                        SET 
                            receipt_no = @receiptNo,
                            amount_paid = @amountPaid,
                            payment_method = @paymentMethod,
                            payment_date = NOW(),
                            remarks = @remarks
                        WHERE payment_id = @paymentId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@receiptNo", TxtReferenceNo.Text.Trim());
                        cmd.Parameters.AddWithValue("@amountPaid", Convert.ToDecimal(TxtTotalAmount.Text));
                        cmd.Parameters.AddWithValue("@paymentMethod", CmbPaymentMethod.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@remarks", TxtRemarks.Text.Trim());
                        cmd.Parameters.AddWithValue("@paymentId", paymentId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Payment updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No changes were made or payment not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating payment: " + ex.Message);
            }
        }
    }
}
