using System;
using System.Diagnostics;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

public static class ValidationHelper
{
    private static readonly string ConnectionString = "server=localhost;database=PDM_Enrollment_DB;user=root;password=;";

    public static bool IsPersonalInfoComplete(long userId)
    {
        try
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        s.student_no, s.student_lrn, s.first_name, s.last_name, s.birth_date,
                        s.sex, s.nationality, c.phone_no, a.barangay, a.city, a.province,
                        g.first_name AS guardian_first, g.last_name AS guardian_last,
                        g.contact_number AS guardian_contact
                    FROM students s
                    LEFT JOIN contact_info c ON s.student_id = c.student_id
                    LEFT JOIN addresses a ON s.student_id = a.student_id
                    LEFT JOIN student_guardians sg ON s.student_id = sg.student_id
                    LEFT JOIN parents_guardians g ON sg.guardian_id = g.guardian_id
                    WHERE s.user_id = @UserID";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Check all required fields
                            if (!IsFieldValid(reader["student_no"])) return false;
                            if (!IsFieldValid(reader["student_lrn"])) return false;
                            if (!IsFieldValid(reader["first_name"])) return false;
                            if (!IsFieldValid(reader["last_name"])) return false;
                            if (reader["birth_date"] == DBNull.Value) return false;
                            if (!IsFieldValid(reader["sex"])) return false;
                            if (!IsFieldValid(reader["phone_no"])) return false;
                            if (!IsFieldValid(reader["barangay"])) return false;
                            if (!IsFieldValid(reader["city"])) return false;
                            if (!IsFieldValid(reader["province"])) return false;
                            if (!IsFieldValid(reader["guardian_first"])) return false;
                            if (!IsFieldValid(reader["guardian_last"])) return false;
                            if (!IsFieldValid(reader["guardian_contact"])) return false;

                            return true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log the error
            Debug.WriteLine($"Error validating personal info: {ex.Message}");
        }

        return false;
    }

    private static bool IsFieldValid(object dbValue)
    {
        return dbValue != DBNull.Value && !string.IsNullOrWhiteSpace(dbValue.ToString());
    }

    public static void ShowValidationError(IWin32Window owner = null)
    {
        MessageBox.Show(owner,
            "Please complete your personal information before proceeding.",
            "Information Required",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
    }
}