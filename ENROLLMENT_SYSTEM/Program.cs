using System;
using System.Windows.Forms;

namespace Enrollment_System
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FormLogin loginForm = new FormLogin();
            Application.Run(loginForm);
        }
    }
}
