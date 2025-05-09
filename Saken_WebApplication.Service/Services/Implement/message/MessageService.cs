using Saken_WebApplication.Data.DTO.message;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces.message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Implement.message
{
    public class MessageService: IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task SendMessageAsync(MessageDto dto)
        {
            var message = new Message
            {
                SenderId = dto.SenderId,
                ReceiverId = dto.ReceiverId,
                Content = dto.Content,
                SentAt = DateTime.UtcNow
            };

            await _messageRepository.SendMessageAsync(message);
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesAsync(string userId1, string userId2)
        {
            var messages = await _messageRepository.GetMessagesAsync(userId1, userId2);

            return messages.Select(m => new MessageDto
            {
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.Content
            }).ToList();
        }
    }
}
