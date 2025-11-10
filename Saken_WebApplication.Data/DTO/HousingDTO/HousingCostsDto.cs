using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO.HousingDTO
{
    public class HousingCostsDto
    {
        public decimal MonthlyPrice { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal InsuranceAmount { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal RentTotal { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
