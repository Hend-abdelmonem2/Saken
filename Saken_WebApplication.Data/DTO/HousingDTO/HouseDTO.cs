using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO.HousingDTO
{
    public class HouseDTO
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
        public double? rate { get; set; }

        public string photoUrl { get; set; }
        public string? OwnerName { get; set; }
        public string? ownerId { get; set; }
    }
}

