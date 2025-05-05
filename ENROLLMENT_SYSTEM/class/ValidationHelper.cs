using System;
using System.Diagnostics;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Enrollment_System;

public static class ValidationHelper
{
    public static bool IsPersonalInfoComplete(long userId)
    {
        try
        {
            using (var conn = new MySqlConnection(DatabaseConfig.ConnectionString))
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
                            return
                                IsFieldValid(reader["student_no"]) &&
                                IsFieldValid(reader["student_lrn"]) &&
                                IsFieldValid(reader["first_name"]) &&
                                IsFieldValid(reader["last_name"]) &&
                                reader["birth_date"] != DBNull.Value &&
                                IsFieldValid(reader["sex"]) &&
                                IsFieldValid(reader["phone_no"]) &&
                                IsFieldValid(reader["barangay"]) &&
                                IsFieldValid(reader["city"]) &&
                                IsFieldValid(reader["province"]) &&
                                IsFieldValid(reader["guardian_first"]) &&
                                IsFieldValid(reader["guardian_last"]) &&
                                IsFieldValid(reader["guardian_contact"]);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
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
