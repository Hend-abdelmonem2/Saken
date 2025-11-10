using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Data.DTO.HousingDTO
{
    public class OfferDto
    {
        public int HousingId { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public decimal DiscountedPrice { get; set; }
        public OfferType offerType { get; set; }
        public double? Rating { get; set; }
        public int Rooms { get; set; }
        public string PhotoUrl { get; set; }
    }
}
