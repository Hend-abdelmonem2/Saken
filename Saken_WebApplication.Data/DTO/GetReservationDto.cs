using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Data.DTO
{
    public  class GetReservationDto
    {
        public int HousingId { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime ReservationDate { get; set; }
        public ReservationStatus Status { get; set; }

        // optional display fields
        public string TenantName { get; set; }
        public string HousingAddress { get; set; }
    }
}
