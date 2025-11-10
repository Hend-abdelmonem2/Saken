using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Implement.contact
{
   public class ContactRepository:IContactRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public ContactRepository( ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
            
        }
        public async Task AddContactAsync(Contact contact)
        {
            _dbContext.contacts.Add(contact);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Contact>> GetAllowedContactsAsync(string ownerUserId)
        {
            return await _dbContext.contacts
                .Where(c => c.OwnerUserId == ownerUserId && !c.IsBlocked)
                .ToListAsync();
        }

        public async Task<List<Contact>> GetBlockedContactsAsync(string ownerUserId)
        {
            return await _dbContext.contacts
                .Where(c => c.OwnerUserId == ownerUserId && c.IsBlocked)
                .ToListAsync();
        }

        public async Task BlockContactAsync(int contactId, bool block)
        {
            var contact = await _dbContext.contacts.FindAsync(contactId);
            if (contact != null)
            {
                contact.IsBlocked = block;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ContactExistsAsync(string ownerUserId, string targetUserId)
        {
            return await _dbContext.contacts
                .AnyAsync(c => c.OwnerUserId == ownerUserId && c.ContactUserId == targetUserId);
        }
    }
}
