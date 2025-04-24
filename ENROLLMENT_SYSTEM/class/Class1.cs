using System;
using System.Windows.Forms;
using Enrollment_System;
using System.Linq;

public static class UIHelper
{
    public static void ApplyAdminVisibility(Control control)
    {
        string[] allowedRoles = { "admin", "super_admin", "cashier" };
        control.Visible = allowedRoles.Any(role => SessionManager.HasRole(role));
    }
}
