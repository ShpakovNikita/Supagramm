using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IgiLab.Constants.Enums;
using EntityCore.Constants;
using EntityCore;

namespace IgiLab.Models.ViewModels.Admin
{
    public class UserSearchModel
    {
        public string Username { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }

        public string Role { get; set; }

        public UserSearchModel()
        {
            Role = Roles.DEFAULT_ROLE;
        }

        public void EmptifyNulls()
        {
            if (String.IsNullOrEmpty(Username))
            {
                Username = "";
            }

            if (String.IsNullOrEmpty(Lastname))
            {
                Lastname = "";
            }

            if (String.IsNullOrEmpty(Firstname))
            {
                Firstname = "";
            }
        }

        public User CastToUser()
        {
            User user = new User();

            user.Username = Username;
            user.Lastname = Lastname;
            user.Firstname = Firstname;
            user.Role = Role;

            return user;
        }
    }
}
