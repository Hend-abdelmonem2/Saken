using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO.HousingDTO
{
   public class OwnerHousingGroupedDto
    {
        public List<HouseDTO> availableHousings { get; set;  } = new();

       
       

        public List<HouseDTO> RentedHousings { get; set; } = new();
    }
}
