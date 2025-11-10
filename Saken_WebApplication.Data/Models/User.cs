using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.Models
{
    public class User : IdentityUser

    {
        public User()
        {
            Properties = new HashSet<Housing>();
            ReservationsAsTenant = new HashSet<Reservation>();
            ReservationsAsLandlord = new HashSet<Reservation>();
            SentMessages = new HashSet<Message>();
            ReceivedMessages = new HashSet<Message>();
            ReviewsSubmitted = new HashSet<Review>();
            ReviewsReceived = new HashSet<Review>();
        }

        [MaxLength(50)]
        public string FullName { get; set; }

        public string? address { get; set; }
        public string? profilePicture { get; set; }

        public DateTime? createdAt { get; set; }

        public double? UserRatingAverage { get; set; } = 0;
        public int? UserRatingCount { get; set; } = 0;

        public bool IsNotificationsEnabled { get; set; } = true;
        public string ThemeMode { get; set; } = "Light";
        public bool IsActive { get; set; } = true;



        [Required]
        public string Role { get; set; }
        [MaxLength(8)]
        public string? ResetCode { get; set; }

        [MaxLength(8)]
        public string? TwoFactorCode { get; set; }
        public DateTime? TwoFactorCodeExpiration { get; set; }
        public DateTime? TwoFactorSentAt { get; set; }

        public int? TwoFactorAttempts { get; set; } = 0; 
        public DateTime? LastTwoFactorAttempt { get; set; } 

       
        public int? FailedTwoFactorAttempts { get; set; } = 0;
        public DateTime? LockoutEnd { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; } = new();

        // Navigation properties
        public virtual ICollection<Housing> Properties { get; set; }
        public ICollection<Reservation> ReservationsAsTenant { get; set; }
        public ICollection<Reservation> ReservationsAsLandlord { get; set; }
        public virtual UserPreferences Preferences { get; set; }
        public virtual ICollection<Message> SentMessages { get; set; }
        public virtual ICollection<Message> ReceivedMessages { get; set; }
        public virtual ICollection<Review> ReviewsSubmitted { get; set; } 
        public virtual ICollection<Review> ReviewsReceived { get; set; }
        public ICollection<SavedHousing> SavedHousings { get; set; }
    }
}
