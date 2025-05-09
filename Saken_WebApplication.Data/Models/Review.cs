using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Data.Models
{
    public class Review
    {
        [Key]
        public int r_Id { get; set; }

        [Required]
        [ForeignKey("Reviewer")]
        public string ReviewerId { get; set; }

        [ForeignKey("ReviewedUser")]
        public string? ReviewedUserId { get; set; } // تغيير من int إلى string وجعلها Nullable

        public int? HousingId { get; set; } // يمكن أن يكون فارغًا

        [Required]
        public ReviewType ReviewType { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual User Reviewer { get; set; }
        public virtual User? ReviewedUser { get; set; } // Nullable لتجنب المشاكل
        public virtual Housing? Housing { get; set; }
    }
}
