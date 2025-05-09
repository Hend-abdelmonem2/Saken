using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Data.DTO.UserPreferences
{
   public class UserPreferencesDto
    {
        public string Location { get; set; }
        public string PreferredPropertyType { get; set; }
        public decimal BudgetMin { get; set; }
        public decimal BudgetMax { get; set; }
        public string PreferredFurnishing { get; set; }
        public string PreferredDuration { get; set; }
        public string PreferredTenantType { get; set; }
        public string PreferredTargetCustomer { get; set; }
    }
}
