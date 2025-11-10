using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO.Guide
{
    public class CreateAgentDto
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }


        [EmailAddress]
        public string Email { get; set; }


        [Phone]
        public string PhoneNumber { get; set; }

        public string CommissionMethod { get; set; } // "Bank", "Cash", etc.

        public string AccountType { get; set; }

        public string AccountNumber { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
