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
            SavedByUsers = new HashSet<SavedHousing>();
        }

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public decimal PricePerMeter { get; set; }
        public int AreaInMeters { get; set; }
        public int Floor { get; set; }

        public PropertyType HousingType { get; set; } // Apartment, Studio, Room, Bed
        public FurnishingStatus FurnishingStatus { get; set; } // Furnished, Empty
        public RentalType RentalType { get; set; } // New, Old

        public bool IsAvailable { get; set; }

        // مدة الإيجار
        public int RentDurationValue { get; set; }
        public RentDurationUnit RentdurationUnit { get; set; }

        // لحساب تاريخ التوفر التالي
        public DateTime? LastRentedDate { get; set; }

        [NotMapped]
        public DateTime? AvailableFrom
        {
            get
            {
                if (LastRentedDate == null)
                    return null;

                return RentdurationUnit switch
                {
                    RentDurationUnit.Day => LastRentedDate.Value.AddDays(RentDurationValue),
                    RentDurationUnit.Week => LastRentedDate.Value.AddDays(RentDurationValue * 7),
                    RentDurationUnit.Month => LastRentedDate.Value.AddMonths(RentDurationValue),
                    RentDurationUnit.Year => LastRentedDate.Value.AddYears(RentDurationValue),
                    _ => null
                };
            }
        }

        // الشكل الداخلي
        public int NumberOfRooms { get; set; }
        public bool HasKitchen { get; set; }
        public bool HasBathroom { get; set; }
        public bool HasLivingRoom { get; set; }

        // الأثاث
        public bool HasBed { get; set; }
        public bool HasWardrobe { get; set; }
        public bool HasChair { get; set; }

        // الأجهزة
        public bool HasFridge { get; set; }
        public bool HasStove { get; set; }
        public bool HasWasher { get; set; }
        public bool HasFan { get; set; }
        public bool HasTV { get; set; }
        public bool HasInternet { get; set; }

        // المرافق
        public bool HasGas { get; set; }
        public bool HasElectricity { get; set; }
        public bool HasWater { get; set; }

        // المستأجر المستهدف
        public TargetCustomerType TargetTenantType { get; set; }
        public string TargetTenantDescription { get; set; }

        // المدفوعات
        public decimal DepositAmount { get; set; }
        public decimal InsuranceAmount { get; set; }
        public decimal CommissionAmount { get; set; }

        // لينك المشاركة
        public string HousingUrl { get; set; }
        public double? HousingRatingAverage { get; set; } = 0;
        public int? HousingRatingCount { get; set; } = 0;
        public bool IsFrozen { get; set; } = false;
        public string? PhotoUrl { get; set; }
        // العلاقات
        public ICollection<HousingPhoto> Photos { get; set; } = new List<HousingPhoto>();
        public ICollection<InspectionSlot> InspectionSlots { get; set; } = new List<InspectionSlot>();

        public string OwnerId { get; set; }
        public User Owner { get; set; }


        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public ICollection<SavedHousing> SavedByUsers { get; set; }
    }
}
