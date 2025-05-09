using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Implement
{
    public class MessageRepository: IMessageRepository
    {
        private readonly ApplicationDBContext _context;
        public MessageRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task SendMessageAsync(Saken_WebApplication.Data.Models.Message message)
        {
            await _context.messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Saken_WebApplication.Data.Models.Message>> GetMessagesAsync(string userId1, string userId2)
        {
            return await _context.messages
                .Where(m =>
                    (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                    (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }
    }
}
