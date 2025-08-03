using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Data.DTO.HousingDTO
{
   public  class HousingDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public decimal PricePerMeter { get; set; }
        public int AreaInMeters { get; set; }
        public int Floor { get; set; }
        public string HousingType { get; set; }
        public string FurnishingStatus { get; set; }
        public string RentalType { get; set; }

        public int RentDurationValue { get; set; }
        public string RentDurationUnit { get; set; }
        public int NumberOfRooms { get; set; }
        public bool HasKitchen { get; set; }
        public bool HasBathroom { get; set; }
        public bool HasLivingRoom { get; set; }


        public bool HasBed { get; set; }
        public bool HasWardrobe { get; set; }
        public bool HasChair { get; set; }

        public bool HasFridge { get; set; }
        public bool HasStove { get; set; }
        public bool HasWasher { get; set; }
        public bool HasFan { get; set; }
        public bool HasTV { get; set; }
        public bool HasInternet { get; set; }

        public bool HasGas { get; set; }
        public bool HasElectricity { get; set; }
        public bool HasWater { get; set; }

        public string TargetTenantType { get; set; }
        public string TargetTenantDescription { get; set; }

        public decimal DepositAmount { get; set; }
        public decimal InsuranceAmount { get; set; }
        public decimal CommissionAmount { get; set; }

        public IFormFile? Photo { get; set; }
        public string photoUrl { get; set; }


        public List<IFormFile>? ExtraPhotos { get; set; }

        public string HousingUrl { get; set; }
        public string? OwnerName { get; set; }
        public string? OwnerPhoneNumber { get; set; }
        public string? OwnerEmail { get; set; }
        public string? ownerId { get; set; }

        public List<DateTime> InspectionDates { get; set; }
    }
}
