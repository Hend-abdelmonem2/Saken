using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO.HousingDTO
{
    public class ReservationResponseDto
    {
        public string ReservationNumber { get; set; }
        public DateTime StartDateTime { get; set; }
        public int DurationInMonths { get; set; }
        public string LandlordName { get; set; }

        // معلومات السكن
        public string HousingType { get; set; }
        public string Location { get; set; }
        public int RoomCount { get; set; }
        public decimal MonthlyPrice { get; set; }
        public double? Rating { get; set; }
        public string ImageUrl { get; set; }
    }
}
