using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enrollment_System
{
    public static class DatabaseConfig
    {
        // Static property to access the connection string
        public static string ConnectionString
        {
            get
            {
                return "server=localhost;database=PDM_Enrollment_DB1;user=root;password=;";
            }
        }
    }
}
