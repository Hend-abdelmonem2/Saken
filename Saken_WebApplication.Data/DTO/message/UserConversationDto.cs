using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO.message
{
    public class UserConversationDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string ProfileImage { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastMessageTime { get; set; }
    }
}
