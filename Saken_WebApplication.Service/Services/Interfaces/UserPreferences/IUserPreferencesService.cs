using Saken_WebApplication.Data.DTO.UserPreferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces.UserPreferences
{
    public interface IUserPreferencesService
    {
        Task SavePreferencesAsync( UserPreferencesDto model);
        Task<UserPreferencesDto> GetPreferencesAsync(string userId);
    }
}
