using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Data.Models
{
    public class Reservation
    {
        [Key]
        public int res_Id { get; set; }

        [ForeignKey("Housing")]
        public int HousingId { get; set; }

        [ForeignKey("Tenant")]
        public string UserId { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        public decimal AmountPaid { get; set; }

        [Required]
        public ReservationStatus Status { get; set; }

        // Navigation properties
        public virtual Housing Housing { get; set; }
        public virtual User Tenant { get; set; }
    }
}
