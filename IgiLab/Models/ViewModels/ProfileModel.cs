using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IgiLab.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using IgiLab.Constants.Enums;
using EntityCore;

namespace IgiLab.Models.ViewModels
{
    public class ProfileModel
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string Username { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        
        public int Followers { get; set; }
        public int Following { get; set; }

        public string Role { get; set; }
        public string AvatarPath { get; set; }

        public SubscriptionStatus SubscriptionStatus { get; set; }

        public List<Post> Posts{ get; set; }

        public ProfileModel(User user)
        {
            Id = user.Id;
            Email = user.Email;
            Username = user.Username;
            Lastname = user.Lastname;
            Firstname = user.Firstname;
            AvatarPath = user.AvatarPath;
        }
    }
}
