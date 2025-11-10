using Saken_WebApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Contact
{
   public interface IContactRepository
    {
        Task AddContactAsync(Saken_WebApplication.Data.Models.Contact contact);

        Task<List<Saken_WebApplication.Data.Models.Contact>> GetAllowedContactsAsync(string ownerUserId);

        Task<List<Saken_WebApplication.Data.Models.Contact>> GetBlockedContactsAsync(string ownerUserId);

        Task BlockContactAsync(int contactId, bool block);

        Task<bool> ContactExistsAsync(string ownerUserId, string targetUserId);
    }
}
