using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Preferences;
using Saken_WebApplication.Service.Services.Interfaces.recommend;
using System.Security.Claims;

namespace Saken_WebApplication.Service.Services.Implement.Recommand
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserPreferencesRepository _repo;
        private readonly IHousingRepository _housingRepo;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDBContext _context;

        public RecommendationService(
            IHttpContextAccessor httpContextAccessor,
            IUserPreferencesRepository repo,
            IHousingRepository housingRepo,
            IUserRepository userRepository,
            ApplicationDBContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _repo = repo;
            _housingRepo = housingRepo;
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<BaseResponse<IEnumerable<HousingRecommendationDto>>> GetRecommendedHousesAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return new BaseResponse<IEnumerable<HousingRecommendationDto>>(false, "User not logged in");

            var pref = await _repo.GetByUserIdAsync(userId);
            var houses = await _housingRepo.GetAllHousesAsync();
            var savedIds = await _context.SavedHousing
                  .Where(s => s.UserId == userId)
                  .Select(s => s.HousingId)
                  .ToListAsync();

            var likedIds = await _context.Likes
                .Where(l => l.UserId == userId && l.EntityType == "Housing")
                .Select(l => l.EntityId)
                .ToListAsync();

            // في حالة عدم وجود تفضيلات محفوظة
            if (pref == null)
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null || string.IsNullOrWhiteSpace(user.address))
                    return new BaseResponse<IEnumerable<HousingRecommendationDto>>(true, "No preferences or address found", Enumerable.Empty<HousingRecommendationDto>());



                var addressRecommendations = houses
                    .Where(h => !string.IsNullOrEmpty(h.Address) &&
                                h.Address.Contains(user.address, StringComparison.OrdinalIgnoreCase))

                    .Select(h => new HousingRecommendationDto
                    {
                        Address = h.Address,
                        Price = h.PricePerMeter,
                        Photo = h.PhotoUrl,
                        IsFavorite = likedIds.Contains(h.Id.ToString()),
                        IsSaved = savedIds.Contains(h.Id),
                        MatchScore = 1
                    })
                    .ToList();

                return new BaseResponse<IEnumerable<HousingRecommendationDto>>(true, "Recommendations based on address", addressRecommendations);
            }

            // في حالة وجود تفضيلات
            var recommendations = houses.Select(h => new HousingRecommendationDto
            {
                Address = h.Address,
                Price = h.PricePerMeter,
                Photo = h.PhotoUrl,
                IsFavorite = likedIds.Contains(h.Id.ToString()),
                IsSaved = savedIds.Contains(h.Id),

                MatchScore =
                    (!string.IsNullOrEmpty(pref.location) && h.Address.Contains(pref.location, StringComparison.OrdinalIgnoreCase) ? 1 : 0) +
                    (h.PricePerMeter >= pref.budgetMin && h.PricePerMeter <= pref.budgetMax ? 1 : 0) +
                    (h.HousingType == pref.PreferredPropertyType ? 1 : 0) +
                    (h.FurnishingStatus == pref.PreferredFurnishing ? 1 : 0) +
                    (h.TargetTenantType == pref.PreferredTargetCustomer ? 1 : 0)
            })
            .Where(r => r.MatchScore > 0)
            .OrderByDescending(r => r.MatchScore)
            .ToList();

            if (!recommendations.Any())
                return new BaseResponse<IEnumerable<HousingRecommendationDto>>(true, "No matching recommendations found", recommendations);

            return new BaseResponse<IEnumerable<HousingRecommendationDto>>(true, "Recommendations retrieved successfully", recommendations);
        }

        public async Task<BaseResponse<IEnumerable<UserRecommendationDto>>> GetRecommendedUsersAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                throw new UnauthorizedAccessException("User not logged in");

            var likedIds = await _context.Likes
           .Where(l => l.UserId == userId && l.EntityType == "User")
           .Select(l => l.EntityId)
           .ToListAsync();
            var currentPref = await _repo.GetByUserIdAsync(userId);
            if (currentPref == null)
                return BaseResponse<IEnumerable<UserRecommendationDto>>.Failure("No preferences found");

            var allPrefs = await _repo.GetAllAsync();
            var otherUsers = allPrefs.Where(p => p.userId != userId);

            var recommendations = new List<UserRecommendationDto>();

            foreach (var pref in otherUsers)
            {
                int score = 0;

                // 1- المكان
                if (!string.IsNullOrEmpty(currentPref.location) && !string.IsNullOrEmpty(pref.location) &&
                    currentPref.location.Equals(pref.location, StringComparison.OrdinalIgnoreCase))
                    score++;

                // 2- نوع العقار
                if (currentPref.PreferredPropertyType == pref.PreferredPropertyType)
                    score++;

                // 3- الفئة المستهدفة
                if (currentPref.PreferredTargetCustomer == pref.PreferredTargetCustomer)
                    score++;

                // 4- الميزانية
                if (currentPref.budgetMin <= pref.budgetMax && currentPref.budgetMax >= pref.budgetMin)
                    score++;

                // 5- الفرش ومدة الإيجار
                if (currentPref.PreferredFurnishing != null && currentPref.PreferredFurnishing == pref.PreferredFurnishing)
                    score++;
                if (currentPref.PreferredDuration != null && currentPref.PreferredDuration == pref.PreferredDuration)
                    score++;

                if (score > 0)
                {
                    var user = await _userRepository.GetByIdAsync(pref.userId);
                    recommendations.Add(new UserRecommendationDto
                    {
                        UserId = pref.userId,
                        FullName = user?.FullName,
                        PhotoUrl = user?.profilePicture,
                        IsFavorite = likedIds.Contains(pref.userId),
                        MatchScore = score,
                        UserType = pref.PreferredTenantType.ToString()
                    });
                }
            }

            if (!recommendations.Any())
                return BaseResponse<IEnumerable<UserRecommendationDto>>.SuccessResponse(recommendations, "No matching recommendations found");

            var result = recommendations.OrderByDescending(r => r.MatchScore).ToList();
            return BaseResponse<IEnumerable<UserRecommendationDto>>.SuccessResponse(result, "تم جلب الترشيحات بنجاح");
        }


        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // نصف قطر الأرض بالكيلومتر
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }
}
