using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppManagers.Database;
using Microsoft.EntityFrameworkCore;

namespace AppManagers.ManagersImpl
{
    public class ManagerBase
    {
        protected Context db;

        public ManagerBase(Context context)
        {
            db = context;
        }
    }
}
