using NuGet.Protocol.Core.Types;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Repositories.Implement;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Implement
{
    public  class HousingFilterService : IHousingFilterService
    {
        private readonly IHouses _housingRepository;
        public HousingFilterService(IHouses housingRepository)
        {
            _housingRepository = housingRepository; 
        }
        public async Task<List<HousingDto>> GetRecommendedHousingsAsync(string userId, string? location = null)
        {
            var preferences = await _housingRepository.GetUserPreferencesAsync(userId);

            List<Saken_WebApplication.Data.Models.Housing> housings;

            if (preferences == null)
            {
                // نبحث باستخدام الموقع فقط لو التفضيلات مش موجودة
                housings = await _housingRepository.GetRecommendedHousingsAsync(new UserPreferences(), location);
            }
            else
            {
                // نبحث باستخدام التفضيلات والموقع
                housings = await _housingRepository.GetRecommendedHousingsAsync(preferences, location);
            }

            if (housings == null || !housings.Any())
                return new List<HousingDto>();

            var housingDtos = housings.Select(h => new HousingDto
            {

                HousingType = h.HousingType.ToString(),
                PricePerMeter = h.PricePerMeter,
                Address = h.Address,
                // photoUrl = h.PhotoUrl,
                FurnishingStatus = h.FurnishingStatus.ToString(),
                TargetTenantType = h.TargetTenantType.ToString(),
                RentDurationUnit = h.RentdurationUnit.ToString(),
                DepositAmount = h.DepositAmount,
                InsuranceAmount = h.InsuranceAmount,
                CommissionAmount = h.CommissionAmount,
                HousingUrl = h.HousingUrl,
                RentalType = h.RentalType.ToString(),
                ownerId = h.OwnerId,

            }).ToList();

            return housingDtos;
        }

    }
    }
    


