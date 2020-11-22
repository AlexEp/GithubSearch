using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GithubSearch.API.Models
{
    public class UserRegisterModal
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [MinLength(8, ErrorMessage = "password must at least 8 charts")]
        [MaxLength(250, ErrorMessage = "password can be max 250 characters long")]
        [DataType(DataType.Password)]
        [RegularExpression("(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,250}$")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "ConfirmPassword")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "password and Confirm passwor do not match")]
        public string ConfirmPassword { get; set; }

        [BindNever]
        internal DateTime Created { get; set; }


        public UserRegisterModal()
        {
            Created = DateTime.UtcNow;
        }
    }
}
