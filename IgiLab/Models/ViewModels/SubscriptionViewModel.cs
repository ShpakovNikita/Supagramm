using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IgiLab.Constants.Enums;
using EntityCore;

namespace IgiLab.Models.ViewModels
{
    public class SubscriptionViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }

        public string Role { get; set; }
        public string AvatarPath { get; set; }

        public SubscriptionStatus SubscriptionStatus { get; set; }

        public SubscriptionViewModel(User user)
        {
            Id = user.Id;
            Username = user.Username;
            Lastname = user.Lastname;
            Firstname = user.Firstname;
            Role = user.Role;
            AvatarPath = user.AvatarPath;
        }
    }
}
