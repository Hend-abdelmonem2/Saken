using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO
{
    public class TokenDTO
    {
        public string UserId { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }


        public string Token { get; set; }
    }
}
