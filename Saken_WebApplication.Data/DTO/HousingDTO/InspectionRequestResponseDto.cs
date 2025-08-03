using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO.HousingDTO
{
    public class InspectionRequestResponseDto
    {
        public string RequestNumber { get; set; }
        public DateTime SlotDateTime { get; set; }
        public string OwnerName { get; set; }
        public string HousingType { get; set; }
        public string Address { get; set; }
        public int NumberOfRooms { get; set; }
        public decimal PricePerMonth { get; set; }
        public string? ImageUrl { get; set; }
        public float Rating { get; set; }
    }
}
