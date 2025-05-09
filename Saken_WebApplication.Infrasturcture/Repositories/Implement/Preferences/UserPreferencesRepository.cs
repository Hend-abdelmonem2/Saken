using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Preferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Implement.Preferences
{
    public class UserPreferencesRepository: IUserPreferencesRepository
    {
        private readonly ApplicationDBContext _context;
        public UserPreferencesRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<UserPreferences> GetByUserIdAsync(string userId)
        {
            return await _context.UserPreferences
                .FirstOrDefaultAsync(up => up.userId == userId);
        }
        public async Task AddAsync(UserPreferences preferences)
        {
            await _context.UserPreferences.AddAsync(preferences);
        }
        public async Task UpdateAsync(UserPreferences preferences)
        {
            _context.UserPreferences.Update(preferences);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
