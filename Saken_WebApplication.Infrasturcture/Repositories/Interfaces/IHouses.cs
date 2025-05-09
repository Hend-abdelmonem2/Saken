using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Interfaces
{
   public  interface IHouses
    {
        Task<UserPreferences> GetUserPreferencesAsync(string userId );
        Task<List<Housing>> GetRecommendedHousingsAsync(UserPreferences preferences , string? location = null);
    }
}
