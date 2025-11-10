using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO.Guide
{
    public class RegisterAgentDto
    {
        
        public string FullName { get; set; }


        public string Role { get; set; } = "AGENT"; // دايمًا Agent


        public string Password { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public string? address { get; set; }

        public IFormFile? Photo { get; set; }

        public string? PhotoUrl { get; set; }

        // البيانات الخاصة بالـ Agent

        public string FirstName { get; set; }


        public string LastName { get; set; }

        public string CommissionMethod { get; set; }

        public string AccountType { get; set; }

        public string AccountNumber { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
