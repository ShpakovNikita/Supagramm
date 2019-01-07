using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityCore;

namespace IgiLab.Models.ViewModels.Admin
{
    public class UserAggregationModel
    {
        public List<User> Users { get; set; }
        public UserSearchModel UserSearchModel { get; set; }

        public UserAggregationModel()
        {
            Users = new List<User>();
        }
    }
}
