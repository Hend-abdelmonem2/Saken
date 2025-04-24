using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO
{
    public class RegisterModelDTO
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [MaxLength(50, ErrorMessage = "Full Name cannot exceed 50 characters.")]
        [DefaultValue("FullName")]
        public string FullName { get; set; }



        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [DefaultValue("user@example.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [DefaultValue("01234567890")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [MaxLength(64, ErrorMessage = "Password cannot exceed 64 characters.")]
        [DefaultValue("SecurePass123")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [MaxLength(64, ErrorMessage = "Password cannot exceed 64 characters.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DefaultValue("SecurePass123")]
        public string ConfirmPassword { get; set; }
        public string address { get; set; }

        public IFormFile? Photo { get; set; }

          public string Role { get; set; }
    }
}
