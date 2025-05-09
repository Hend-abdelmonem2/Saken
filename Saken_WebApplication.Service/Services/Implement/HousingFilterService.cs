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
        public async Task<List<HousingDto>> GetRecommendedHousingsAsync(string userId , string? location = null)
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

                Type = h.type.ToString(),
                Price = h.price,
                Address = h.address,
                PhotoUrl = h.Photo,
                Status = h.status.ToString(),
                FurnishingStatus = h.furnishingStatus.ToString(),
                TargetCustomers = h.targetCustomers.ToString(),
                RentalPeriod = h.rentalPeriod.ToString(),
                Deposit = h.deposit,
                Rent = h.rent,
                Insurance = h.insurance,
                Commission = h.commission,
                ParticipationLink = h.participationLink,
                RentalType = h.RentalType.ToString(),
                LandlordId = h.LandlordId,

            }).ToList();

            return housingDtos;
        }
    }
    }
    


