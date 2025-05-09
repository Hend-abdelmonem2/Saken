using Saken_WebApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Preferences
{
    public  interface IUserPreferencesRepository
    {

        Task<UserPreferences> GetByUserIdAsync(string userId);
        Task AddAsync(UserPreferences preferences);
        Task UpdateAsync(UserPreferences preferences);
        Task SaveChangesAsync();
    }
}
