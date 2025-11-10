using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO.message
{
    public class GetMessageDto
    {
        public string OtherUserId { get; set; }
        public string OtherUserName { get; set; }
        public string OtherUserImage { get; set; }
        public string OtherUserRole { get; set; }

        public IEnumerable<MessageDto> Messages { get; set; }
    }
}
