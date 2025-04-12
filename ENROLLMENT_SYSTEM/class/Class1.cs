using System;
using System.Windows.Forms;
using Enrollment_System;

public static class UIHelper
{
    public static void ApplyAdminVisibility(Control control)
    {
        control.Visible = SessionManager.HasRole("admin");
    }
}
