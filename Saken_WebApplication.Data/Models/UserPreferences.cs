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
    public class UserPreferences
    {
        [Key]
        public int p_Id { get; set; }

        [ForeignKey("User")]
        public string userId { get; set; }

        public string location { get; set; }

        public PropertyType PreferredPropertyType { get; set; }

        public decimal budgetMin { get; set; }

        public decimal budgetMax { get; set; }

        public FurnishingStatus PreferredFurnishing { get; set; }

        public RentalDuration PreferredDuration { get; set; }

        public UserRole PreferredTenantType { get; set; }
        public TargetCustomerType PreferredTargetCustomer { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
}
