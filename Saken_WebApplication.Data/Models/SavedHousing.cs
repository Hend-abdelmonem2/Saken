using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.Models
{
   public class SavedHousing
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public User user { get; set; }

        public int HousingId { get; set; }
        public Housing Housing { get; set; }

        public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    }
}
