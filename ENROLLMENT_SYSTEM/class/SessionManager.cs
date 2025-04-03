using System;

namespace Enrollment_System
{
    public static class SessionManager
    {
        public static long UserId { get; private set; }
        public static string UserEmail { get; private set; }
        public static string UserRole { get; private set; }
        public static string FirstName { get; private set; }
        public static string LastName { get; private set; }

        public static bool IsLoggedIn => UserId > 0;

        public static void Login(long userId, string email, string role, string firstName, string lastName)
        {
            UserId = userId;
            UserEmail = email;
            UserRole = role;
            FirstName = firstName;
            LastName = lastName;
        }

        public static void Logout()
        {
            UserId = 0;
            UserEmail = null;
            UserRole = null;
            FirstName = null;
            LastName = null;
        }
    }
}
