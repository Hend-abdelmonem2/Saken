using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.Models
{
   public  class Like
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int EntityId { get; set; } // ID للعنصر سواء كان Doctor أو Housing

        [Required]
        public string EntityType { get; set; } // مثلاً: "Doctor", "Housing"

        public virtual User User { get; set; }
    }
}
