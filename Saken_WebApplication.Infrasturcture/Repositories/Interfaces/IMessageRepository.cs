using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Interfaces
{
    public  interface IMessageRepository
    {
        Task SendMessageAsync(Saken_WebApplication.Data.Models.Message message);
        Task<IEnumerable< Saken_WebApplication.Data.Models.Message>> GetMessagesAsync(string userId1, string userId2);
    }
}
