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
        public string Type { get; set; }
        public decimal Price { get; set; }
        public string Address { get; set; }
        public IFormFile Photo { get; set; }
        public string Status { get; set; }
        public string FurnishingStatus { get; set; }
        public string TargetCustomers { get; set; }
        public string RentalPeriod { get; set; }
        public decimal Deposit { get; set; }
        public decimal Rent { get; set; }
        public decimal Insurance { get; set; }
        public decimal Commission { get; set; }
        public string ParticipationLink { get; set; }
        public string RentalType { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string LandlordId { get; set; }
        public string photoUrl { get; set; }

    }
}
