using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using IgiLab.Validation;
using EntityCore;

// This is localized view model
namespace IgiLab.Models.ViewModels
{
    public class RegisterModel
    {
        [RegularExpression(ValidationRegexp.Email, ErrorMessage = "EmailRegexp")]
        [Required(ErrorMessage = "NoEmail")]
        public string Email { get; set; }

        [RegularExpression(ValidationRegexp.Password, ErrorMessage = "PasswordMessage")]
        [Required(ErrorMessage = "Nopassword")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "PasswordConfirmMessage")]
        public string ConfirmPassword { get; set; }

        [RegularExpression(ValidationRegexp.Username, ErrorMessage = "NameMessage")]
        [Required(ErrorMessage = "NoLastname")]
        public string Lastname { get; set; }

        [RegularExpression(ValidationRegexp.Username, ErrorMessage = "NameMessage")]
        [Required(ErrorMessage = "NoFirstname")]
        public string Firstname { get; set; }

        [RegularExpression(ValidationRegexp.Username, ErrorMessage = "NameMessage")]
        [Required(ErrorMessage = "NoUsername")]
        public string Username { get; set; }
    }
}
