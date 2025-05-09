using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO.HousingDTO
{
    public class HouseFilterDto
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsAvailable { get; set; }

        [DefaultValue("price")]
        public string OrderBy { get; set; }

        string sortBy { get; set; } = "asc";

    }
}
