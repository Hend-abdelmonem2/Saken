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
    public class Housing
    {
        public Housing()
        {
            Reviews = new HashSet<Review>();
            Reservations = new HashSet<Reservation>();
        }

        [Key]
        public int h_Id { get; set; }

        [Required]
        public PropertyType type { get; set; }

        [Required]
        public decimal price { get; set; }

        public double rating { get; set; }

        [Required]
        public string address { get; set; }

        public string Photo { get; set; }

        [Required]
        public HousingStatus status { get; set; }

        [Required]
        public FurnishingStatus furnishingStatus { get; set; }

        //public bool likes { get; set; }

        public DateTime? InspectionDate { get; set; }

        // Replace string with enum
        [Required]
        public TargetCustomerType targetCustomers { get; set; }

        public RentalDuration rentalPeriod { get; set; }

        public decimal deposit { get; set; }

        public decimal rent { get; set; }

        public decimal insurance { get; set; }

        public decimal commission { get; set; }

        public string participationLink { get; set; }

        public RentalType RentalType { get; set; }

        [ForeignKey("Landlord")]
        public string LandlordId { get; set; }

        // Navigation properties
        public virtual User Landlord { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
