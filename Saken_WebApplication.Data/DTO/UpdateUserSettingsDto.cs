using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO
{
    public  class UpdateUserSettingsDto
    {
        public bool IsNotificationsEnabled { get; set; }
        public string ThemeMode { get; set; }
    }
}
