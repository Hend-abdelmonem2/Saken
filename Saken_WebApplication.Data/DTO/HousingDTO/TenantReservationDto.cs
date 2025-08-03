using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO.HousingDTO
{
    public class TenantReservationDto
    {
        public string HousingTitle { get; set; }
        public string Address { get; set; }
        public DateTime ReservationDate { get; set; }
        public string OwnerName { get; set; }
        public decimal PricePerMonth { get; set; }
        public string ImageUrl { get; set; }
    }
}
