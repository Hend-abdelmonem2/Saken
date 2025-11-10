using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.Models.Guid
{
    public class Agent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        // البيانات الخاصة بالدليل
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        // طريقة استلام العمولة
        public string CommissionMethod { get; set; } // مثلاً: "Bank", "VodafoneCash", ...

        public string AccountType { get; set; } // حساب شخصي / تجاري إلخ

        public string AccountNumber { get; set; }

        public string AdditionalInfo { get; set; }

    }
}
