using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Data.Models
{
    public class HousingOffer
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public int HousingId { get; set; }
        public Housing Housing { get; set; }

       
        [Required]
        public OfferType OfferType { get; set; }


        public string? Description { get; set; }


        public decimal? DiscountedPricePerMeter { get; set; }
        public decimal? DiscountedInsuranceAmount { get; set; }
        public bool? IsCommissionFree { get; set; }
        public bool? IsFirstMonthFree { get; set; }
        public bool? IncludesFreeInternet { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive => DateTime.Now >= StartDate && DateTime.Now <= EndDate;
    }
}
