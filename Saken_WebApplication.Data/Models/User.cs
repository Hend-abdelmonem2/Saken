using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.Models
{
    public class User:IdentityUser
    
    {
        public User()
    {
        Properties = new HashSet<Housing>();
        Reservations = new HashSet<Reservation>();
        SentMessages = new HashSet<Message>();
        ReceivedMessages = new HashSet<Message>();
        ReviewsSubmitted = new HashSet<Review>();
        ReviewsReceived = new HashSet<Review>();
    }

    /* [Key]
     public int u_Id { get; set; }

      [Required]
       [StringLength(100)]
       public string name { get; set; }

       [Required]
       [EmailAddress]
       public string email { get; set; }

       [Phone]
       public string phone { get; set; }
     [Required]
       public string password { get; set; }*/
    [MaxLength(50)]
    public string FullName { get; set; }

    public string? address { get; set; }
    public string? profilePicture { get; set; }

    public DateTime createdAt { get; set; }

    public double rate { get; set; }

    public bool IsNotificationsEnabled { get; set; } = true;
    public string ThemeMode { get; set; } = "Light";

        //public bool favorite { get; set; }

        [Required]
    public string Role { get; set; }
    [MaxLength(8)]
    public string? ResetCode { get; set; }

    [MaxLength(8)]
    public string? TwoFactorCode { get; set; }
    public DateTime? TwoFactorCodeExpiration { get; set; }
    public DateTime? TwoFactorSentAt { get; set; } //  وقت إرسال الكود الأخير

    public int TwoFactorAttempts { get; set; } = 0; // عدد المحاولات خلال الساعة
    public DateTime? LastTwoFactorAttempt { get; set; } // آخر وقت للمحاولة

    // 🔹 جديد: محاولات الفشل وقفل الحساب
    public int FailedTwoFactorAttempts { get; set; } = 0;
    public DateTime? LockoutEnd { get; set; } // متى ينتهي القفل؟

    public List<RefreshToken>? RefreshTokens { get; set; }

    // Navigation properties
    public virtual ICollection<Housing> Properties { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; }
    public virtual UserPreferences Preferences { get; set; }
    public virtual ICollection<Message> SentMessages { get; set; }
    public virtual ICollection<Message> ReceivedMessages { get; set; }
    public virtual ICollection<Review> ReviewsSubmitted { get; set; } // Reviews written by this user
    public virtual ICollection<Review> ReviewsReceived { get; set; }
}
}
