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
        #endregion

        #region Public Methods
        /// Initializes a new user session
        public static void Login(long userId, string email, string role, string firstName, string lastName, int studentId = 0)
        {
            UserId = userId;
            UserEmail = email;
            UserRole = role;
            FirstName = firstName;
            LastName = lastName;
            StudentId = studentId;
            LoginTime = DateTime.Now;

            // Log the login event
            LogSessionActivity($"User logged in: {email}");
        }


        /// Clears the current user session
        public static void Logout()
        {
            // Log the logout event
            LogSessionActivity($"User logged out: {UserEmail}");

            // Clear all session data
            UserId = 0;
            UserEmail = null;
            UserRole = null;
            FirstName = null;
            LastName = null;
            SelectedCourse = null;
            StudentId = 0;
        }

        public static bool HasRole(string requiredRole)
        {
            return string.Equals(UserRole, requiredRole, StringComparison.OrdinalIgnoreCase);
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

            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now}] {message}");
        }
        #endregion
    }
}