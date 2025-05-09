using Microsoft.AspNetCore.Http;
using Saken_WebApplication.Data.DTO.UserPreferences;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Preferences;
using Saken_WebApplication.Service.Services.Interfaces.UserPreferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;
using UserPreferences = Saken_WebApplication.Data.Models.UserPreferences; // Add this alias to resolve ambiguity



namespace Saken_WebApplication.Service.Services.Implement.UserPreference
{
    public class UserPreferencesService: IUserPreferencesService
    {
        private readonly IUserPreferencesRepository _repo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserPreferencesService(IUserPreferencesRepository repo , IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task SavePreferencesAsync( UserPreferencesDto model)

        {
            var user = _httpContextAccessor.HttpContext?.User;
          var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new UnauthorizedAccessException("User ID not found in token.");


            var existing = await _repo.GetByUserIdAsync(userId);

            if (existing != null)
            {
                existing.location = model.Location;
                existing.PreferredPropertyType = Enum.Parse<PropertyType>(model.PreferredPropertyType, true);
                existing.budgetMin = model.BudgetMin;
                existing.budgetMax = model.BudgetMax;
                existing.PreferredFurnishing = Enum.Parse<FurnishingStatus>(model.PreferredFurnishing, true);
                existing.PreferredDuration = Enum.Parse<RentalDuration>(model.PreferredDuration, true);
                existing.PreferredTenantType = Enum.Parse<UserRole>(model.PreferredTenantType, true);
                existing.PreferredTargetCustomer = Enum.Parse<TargetCustomerType>(model.PreferredTargetCustomer, true);

                await _repo.UpdateAsync(existing);
            }
            else
            {
                var preferences = new UserPreferences
                {
                    userId = userId,
                    location = model.Location,
                    PreferredPropertyType = Enum.Parse<PropertyType>(model.PreferredPropertyType, true),
                    budgetMin = model.BudgetMin,
                    budgetMax = model.BudgetMax,
                    PreferredFurnishing = Enum.Parse<FurnishingStatus>(model.PreferredFurnishing, true),
                    PreferredDuration = Enum.Parse<RentalDuration>(model.PreferredDuration, true),
                    PreferredTenantType = Enum.Parse<UserRole>(model.PreferredTenantType, true),
                    PreferredTargetCustomer = Enum.Parse<TargetCustomerType>(model.PreferredTargetCustomer, true)
                };
                await _repo.AddAsync(preferences);
            }

            await _repo.SaveChangesAsync();


        }
        public async Task<UserPreferencesDto> GetPreferencesAsync(string userId)
        {
            var UserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserId == null)
                throw new UnauthorizedAccessException("User not logged in");

            var pref = await _repo.GetByUserIdAsync(userId);
            if (pref == null) return null;

            return new UserPreferencesDto
            {
                Location = pref.location,
                PreferredPropertyType = pref.PreferredPropertyType.ToString(),
                BudgetMin = pref.budgetMin,
                BudgetMax = pref.budgetMax,
                PreferredFurnishing = pref.PreferredFurnishing.ToString(),
                PreferredDuration = pref.PreferredDuration.ToString(),
                PreferredTenantType = pref.PreferredTenantType.ToString(),
                PreferredTargetCustomer = pref.PreferredTargetCustomer.ToString()
            };
        }



    }
}
