using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using IgiLab.Validation;
using EntityCore;

namespace IgiLab.Models.ViewModels
{
    public class EditProfileModel
    {
        [RegularExpression(ValidationRegexp.Password, ErrorMessage = "Password should contain minimum eight characters, at least one letter and one number")]
        [Required(ErrorMessage = "No password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password typed incorrectly")]
        public string ConfirmPassword { get; set; }

        [RegularExpression(ValidationRegexp.Username, ErrorMessage = "This field shouldn't contain underscores or other special symbols and be at least 4 to 15 characters long")]
        [Required(ErrorMessage = "No Lastname")]
        public string Lastname { get; set; }

        [RegularExpression(ValidationRegexp.Username, ErrorMessage = "This field shouldn't contain underscores or other special symbols and be at least 4 to 15 characters long")]
        [Required(ErrorMessage = "No Firstname")]
        public string Firstname { get; set; }

    }
}
