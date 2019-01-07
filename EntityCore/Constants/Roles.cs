using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityCore.Constants
{
    public static class Roles
    {
        public static string ADMIN = "admin";
        public static string USER = "user";

        public static string DEFAULT_ROLE { get => USER; }
    }
}
