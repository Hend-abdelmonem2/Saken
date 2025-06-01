using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO.HousingDTO
{
   public  class ReservationContractDto
    {
        public string LandlordName { get; set; }
        public string LandlordPhone { get; set; }

        public string TenantName { get; set; }
        public string TenantPhone { get; set; }

        public string HousingAddress { get; set; }
        public DateTime ReservationDate { get; set; }
        public decimal AmountPaid { get; set; }
        public string Status { get; set; }

        public string ContractText { get; set; }
    }
}
