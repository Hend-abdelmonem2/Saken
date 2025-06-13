using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.Models
{
   public class AppSettings
    {
        public int Id { get; set; }
        public string AppName { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string SupportEmail { get; set; } = string.Empty;
        public bool EnableNotifications { get; set; }
    }
}
