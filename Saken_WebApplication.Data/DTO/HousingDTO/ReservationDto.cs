using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Data.DTO.HousingDTO
{
    public class ReservationDto
    {

        public int HousingId { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public int DurationInMonths { get; set; }


        [Required]
        public string TenantFullName { get; set; }

        [Required]
        public string TenantPhoneNumber { get; set; }
    }
}
