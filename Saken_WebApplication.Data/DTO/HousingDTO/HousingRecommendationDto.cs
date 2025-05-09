using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO.HousingDTO
{
    public  class HousingRecommendationDto
    {
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string Photo { get; set; }
        public int MatchScore { get; set; }
    }
}
