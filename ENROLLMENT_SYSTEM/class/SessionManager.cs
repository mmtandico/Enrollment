using System;
using System.Drawing;

namespace Enrollment_System
{
    /// <summary>
    /// Manages user session information throughout the application
    /// </summary>
    public static class SessionManager
    {
        #region Properties
        public static long UserId { get; private set; }
        public static string UserEmail { get; private set; }
        public static string UserRole { get; private set; }
        public static string FirstName { get; set; }
        public static string LastName { get; set; }
        public static string SelectedCourse { get; set; }
        public static string CurrentBannerCourse { get; set; }
        public static Image CurrentBannerImage { get; set; }
        public static string FullName => $"{FirstName} {LastName}".Trim();
        public static string Initials => GetInitials();
        public static bool IsLoggedIn => UserId > 0;
        public static DateTime LoginTime { get; private set; }
        public static TimeSpan SessionDuration => DateTime.Now - LoginTime;
        public static int StudentId { get; set; }

        // Role-based access properties
        public static bool IsAdmin => UserRole?.Equals("admin", StringComparison.OrdinalIgnoreCase) ?? false;
        public static bool IsSuperAdmin => UserRole?.Equals("super_admin", StringComparison.OrdinalIgnoreCase) ?? false;
        public static bool IsAdminOrSuperAdmin => IsAdmin || IsSuperAdmin;

        

        // Admin/SuperAdmin student viewing functionality
        public static int CurrentViewingStudentId { get; set; }
        public static string CurrentViewingStudentNo { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initializes a new user session
        /// </summary>
        public static void Login(long userId, string email, string role, string firstName, string lastName, int studentId = 0)
        {
            UserId = userId;
            UserEmail = email;
            UserRole = role;
            FirstName = firstName;
            LastName = lastName;
            StudentId = studentId;
            LoginTime = DateTime.Now;
            CurrentViewingStudentId = 0;
            CurrentViewingStudentNo = null;

            LogSessionActivity($"User logged in: {email} (Role: {role})");
        }

        /// <summary>
        /// Clears the current user session
        /// </summary>
        public static void Logout()
        {
            LogSessionActivity($"User logged out: {UserEmail} (Session duration: {SessionDuration.TotalMinutes:F1} minutes)");

            // Clear all session data
            UserId = 0;
            UserEmail = null;
            UserRole = null;
            FirstName = null;
            LastName = null;
            SelectedCourse = null;
            StudentId = 0;
            CurrentViewingStudentId = 0;
            CurrentViewingStudentNo = null;
        }

        /// <summary>
        /// Checks if user has the specified role
        /// </summary>
        public static bool HasRole(string requiredRole)
        {
            return string.Equals(UserRole, requiredRole, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Sets the student currently being viewed by admin or super admin
        /// </summary>
        public static void SetViewingStudent(int studentId, string studentNo)
        {
            if (!IsAdminOrSuperAdmin) return;

            CurrentViewingStudentId = studentId;
            CurrentViewingStudentNo = studentNo;
            LogSessionActivity($"Admin now viewing student: {studentNo} (ID: {studentId})");
        }

        /// <summary>
        /// Clears the currently viewed student
        /// </summary>
        public static void ClearViewingStudent()
        {
            if (!IsAdminOrSuperAdmin) return;

            LogSessionActivity($"Admin stopped viewing student: {CurrentViewingStudentNo}");
            CurrentViewingStudentId = 0;
            CurrentViewingStudentNo = null;
        }
        #endregion

        #region Private Methods
        private static string GetInitials()
        {
            if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName))
                return string.Empty;

            string firstInitial = FirstName?.Length > 0 ? FirstName[0].ToString() : string.Empty;
            string lastInitial = LastName?.Length > 0 ? LastName[0].ToString() : string.Empty;

            return $"{firstInitial}{lastInitial}".ToUpper();
        }

        private static void LogSessionActivity(string message)
        {
            // In production, you might want to write to a log file or database
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
        }
        #endregion
    }
}
