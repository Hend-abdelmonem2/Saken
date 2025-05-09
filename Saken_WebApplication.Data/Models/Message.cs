using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.Models
{
   public class Message
    {
        [Key]
        public int m_Id { get; set; }

        [ForeignKey("Sender")]
        public string SenderId { get; set; }

        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime SentAt { get; set; }

        // Navigation properties
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
    }
}
