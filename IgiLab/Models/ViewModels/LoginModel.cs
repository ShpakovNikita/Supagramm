using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using EntityCore;


namespace IgiLab.Models.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "No username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "No password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
