using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.Models
{
   public class InspectionSlot
    {
        public int Id { get; set; }

        public DateTime StartDateTime { get; set; }
        public bool IsBooked { get; set; } = false;

        // العلاقة مع السكن
        public int HousingId { get; set; }
        public Housing Housing { get; set; }
    }
}
