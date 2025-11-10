using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Data.Models.Guid
{
   public class CommissionTracking
    {
        [Key]
        public int Id { get; set; }

        public string AgentId { get; set; }  // من جدول Identity
        public User Agent { get; set; }

        public int HousingId { get; set; }
        public Housing Housing { get; set; }

        public CommissionStatus CurrentStatus { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
