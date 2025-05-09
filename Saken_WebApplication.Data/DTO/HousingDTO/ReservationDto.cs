using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Data.DTO.HousingDTO
{
   public class ReservationDto
    {

        public int HousingId { get; set; }
        public decimal AmountPaid { get; set; }
       // public ReservationStatus Status { get; set; }  // Use string for status or create a separate enum if needed
    }
}
