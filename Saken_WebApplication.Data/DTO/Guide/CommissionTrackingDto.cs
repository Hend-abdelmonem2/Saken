using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Data.DTO.Guide
{
    public class CommissionTrackingDto
    {
        public int HousingId { get; set; }
        public string HousingTitle { get; set; }
        public CommissionStatus CurrentStatus { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
