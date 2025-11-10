using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string PhotoUrl { get; set; }

        [Required]
        public string OwnerUserId { get; set; }
        public string ContactUserId { get; set; }

        public bool IsBlocked { get; set; }

        public string Role { get; set; }

        [ForeignKey(nameof(OwnerUserId))]
        public virtual User OwnerUser { get; set; }

        [ForeignKey(nameof(ContactUserId))]
        public virtual User ContactUser { get; set; }
    }
}
