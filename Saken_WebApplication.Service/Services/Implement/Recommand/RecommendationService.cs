using Microsoft.AspNetCore.Http;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Preferences;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using Saken_WebApplication.Service.Services.Interfaces.recommend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Implement.Recommand
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserPreferencesRepository _repo;
        private readonly IHousingRepository _housingRepo;
        public RecommendationService(IHttpContextAccessor httpContextAccessor, IUserPreferencesRepository repo, IHousingRepository housingRepo)
        {
            _httpContextAccessor = httpContextAccessor;
            _repo = repo;
            _housingRepo = housingRepo;
        }
        public async Task<IEnumerable<HousingRecommendationDto>> GetRecommendedHousesAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                throw new UnauthorizedAccessException("User not logged in");

            var pref = await _repo.GetByUserIdAsync(userId);
            if (pref == null)
                return Enumerable.Empty<HousingRecommendationDto>();

            var houses = await _housingRepo.GetAllHousesAsync();
            var recommendations = houses.Select(h => new HousingRecommendationDto
            {
                Address = h.address,
                Price = h.price,
                Photo = h.Photo,
                MatchScore =
                  (h.address.Contains(pref.location, StringComparison.OrdinalIgnoreCase) ? 1 : 0) +
                  (h.price >= pref.budgetMin && h.price <= pref.budgetMax ? 1 : 0) +
                  (h.type == pref.PreferredPropertyType ? 1 : 0) +
                  (h.furnishingStatus == pref.PreferredFurnishing ? 1 : 0) +
                  (h.rentalPeriod == pref.PreferredDuration ? 1 : 0) +
                  (h.targetCustomers == pref.PreferredTargetCustomer ? 1 : 0)
            })
           .Where(r => r.MatchScore > 0) // ✅ شرط التصفية
           .OrderByDescending(r => r.MatchScore)
           .ToList();
            return recommendations;
        }
    }
}
