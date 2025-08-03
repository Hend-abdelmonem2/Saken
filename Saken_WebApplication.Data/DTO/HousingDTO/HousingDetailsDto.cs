using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO.HousingDTO
{
    public class HousingDetailsDto
    {
        public string Title { get; set; }
        public string Address { get; set; }
        public decimal PricePerMeter { get; set; }
        public int AreaInMeters { get; set; }
        public int Floor { get; set; }
        public string HousingType { get; set; }

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

        // الحجز
        public int RentDurationInMonths { get; set; }

        public decimal DepositAmount { get; set; }
        public decimal InsuranceAmount { get; set; }
        public decimal CommissionAmount { get; set; }

        public string HousingUrl { get; set; }

        public List<string> PhotoUrls { get; set; }
        public List<InspectionSlotDetailsDto> InspectionSlots { get; set; }

        // معلومات المالك
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
    }
}
