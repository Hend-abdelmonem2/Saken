using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Infrasturcture.Repositories.Implement
{
    public  class Housing:IHouses
   
    {
        private readonly ApplicationDBContext _context;
        public Housing(ApplicationDBContext context)
        {
            _context = context;

        }
        public async Task<UserPreferences> GetUserPreferencesAsync(string userId)
        {
            return await _context.UserPreferences
                .FirstOrDefaultAsync(p => p.userId == userId);
        }
        public async Task<List<Saken_WebApplication.Data.Models.Housing>> GetRecommendedHousingsAsync(UserPreferences preferences, string? location)
        {
            var query = _context.houses
                .Include(h => h.Owner)
                .AsQueryable();
            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(h => h.Address.Contains(location));

            }

            if (preferences.PreferredPropertyType != null)
                query = query.Where(h => h.HousingType == preferences.PreferredPropertyType);

            if (preferences.budgetMin > 0)
                query = query.Where(h => h.PricePerMeter >= preferences.budgetMin);

            if (preferences.budgetMax > 0)
                query = query.Where(h => h.PricePerMeter <= preferences.budgetMax);

            if (preferences.PreferredFurnishing != null)
                query = query.Where(h => h.FurnishingStatus == preferences.PreferredFurnishing);

            /* if (preferences.PreferredDuration != null)
                 query = query.Where(h => h.RentdurationUnit == preferences.PreferredDuration);*/
            if (preferences.PreferredTargetCustomer != null)
                query = query.Where(h => h.TargetTenantType == preferences.PreferredTargetCustomer);

            return await query.ToListAsync();


        }

    }
}
