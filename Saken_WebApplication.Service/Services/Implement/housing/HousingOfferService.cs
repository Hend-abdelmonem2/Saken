using Microsoft.AspNetCore.Http;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using Saken_WebApplication.Service.Services.Interfaces.Notifications;

namespace Saken_WebApplication.Service.Services.Implement.housing
{
    public class HousingOfferService : IHousingOfferService
    {
        private readonly IHousingOfferRepository _offerRepo;
        private readonly IAuthService _authService;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHousingRepository _housingRepository;


        public HousingOfferService(IHousingOfferRepository offerRepo, IAuthService authService, INotificationService notificationService, IHttpContextAccessor httpContextAccessor, IHousingRepository housingRepository)
        {
            _offerRepo = offerRepo;
            _authService = authService;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _housingRepository = housingRepository;
        }
        public async Task<BaseResponse<IEnumerable<OfferDto>>> GetActiveOffersAsync()
        {
            var offers = await _offerRepo.GetActiveOffersAsync();
            if (offers == null || !offers.Any())
                return BaseResponse<IEnumerable<OfferDto>>.Failure("No Offer Found");

            var response = offers.Select(o => new OfferDto
            {
                HousingId = o.HousingId,
                Title = o.Housing.Title,
                Address = o.Housing.Address,
                DiscountedPrice = o.DiscountedPricePerMeter ?? o.Housing.PricePerMeter,
                Rating = o.Housing.HousingRatingAverage,
                Rooms = o.Housing.NumberOfRooms,
                PhotoUrl = o.Housing.PhotoUrl,
                offerType = o.OfferType
            });
            return BaseResponse<IEnumerable<OfferDto>>.SuccessResponse(response, "Retrived Active Offer");
        }
        public async Task<BaseResponse<IEnumerable<HousingOffer>>> GetOffersByHousingIdAsync(int housingId)
        {
            var result = await _offerRepo.GetOffersByHousingIdAsync(housingId);
            if (result == null)
                return BaseResponse<IEnumerable<HousingOffer>>.Failure("No Offer Found For this house");


            return BaseResponse<IEnumerable<HousingOffer>>.SuccessResponse(result, "Offer For House");
        }
        public async Task<BaseResponse<string>> CreateOfferAsync(HousingOfferDto dto)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User
               .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
                return new BaseResponse<string>(false, " لم يتم العثور على المستخدم");
            var housing = await _housingRepository.GetByIdAsync(dto.HousingId);
            if (housing == null)
                return new BaseResponse<string>(false, " السكن غير موجود");
            if (housing.OwnerId != currentUserId)
                return new BaseResponse<string>(false, " غير مسموح لك بإضافة عرض على هذا السكن");
            var offer = new HousingOffer
            {
                HousingId = dto.HousingId,
                OfferType = dto.OfferType,
                Description = dto.Description,
                DiscountedPricePerMeter = dto.DiscountedPricePerMeter,
                DiscountedInsuranceAmount = dto.DiscountedInsuranceAmount,
                IsCommissionFree = dto.IsCommissionFree,
                IsFirstMonthFree = dto.IsFirstMonthFree,
                IncludesFreeInternet = dto.IncludesFreeInternet,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
            };

            await _offerRepo.AddOfferAsync(offer);
            await _offerRepo.SaveChangesAsync();
            var tenants = await _authService.GetUsersByRoleAsync(currentUserId, "Tenant");
            if (tenants.Data != null)

                foreach (var tenant in tenants.Data)
                {
                    await _notificationService.SendNotificationAsync(
                        tenant.Id,
                        " عرض جديد!",
                        $"تمت إضافة عرض جديد للسكن. تفقده الآن!"
                    );
                }

            return new BaseResponse<string>(true, " تم إنشاء العرض بنجاح");
        }
    }

}
