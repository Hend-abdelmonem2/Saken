using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Service.Services.Interfaces.housing
{
    public interface IHousingService
    {
        Task<BaseResponse<string>> AddHousingAsync(HousingDto dto, string landlordId);
        Task<BaseResponse<InspectionRequestResponseDto>> SubmitInspectionRequestAsync(InspectionRequestDto dto);
        Task<BaseResponse<bool>> SaveHousingAsync(string userId, int housingId);
        Task<BaseResponse> UpdateHousingAsync(int id, UpdateHousingDto dto, string userId);
        Task<BaseResponse<bool>> ToggleFreezeAsync(int id);
        Task<BaseResponse<bool>> DeleteHousingAsync(int housingId, string userId, bool isAdmin);
        Task<BaseResponse<IEnumerable<HouseDTO>>> GetAllHousesAsync(string currentUserId);


        Task<BaseResponse<List<InspectionSlotDetailsDto>>> GetAvailableSlotsAsync(int housingId);
        Task<BaseResponse<OwnerHousingGroupedDto>> GetGroupedHousingsForLandlordAsync(string landlordId);

        Task<BaseResponse<HousingDetailsDto?>> GetHousingByIdAsync(int id);
        Task<BaseResponse<List<HouseDTO>>> GetHousingsByHighestRatingAsync(string currentUserId);
        Task<BaseResponse<List<HouseDTO>>> GetHousingsByLowestPriceAsync(string currentUserId);
        Task<BaseResponse<List<HouseDTO>>> GetHousingsByTypeAsync(string currentUserId, PropertyType type);
        Task<BaseResponse<List<InspectionRequestResponseDto>>> GetInspectionRequestsForOwnerAsync(string ownerId);

        Task<BaseResponse<List<HouseDTO>>> GetSavedHousingsAsync(string userId);
        Task<BaseResponse<bool>> ApproveHousingAsync(int housingId);

        Task<BaseResponse<string>> RejectHouseAsync(int houseId, string reason);

        Task<BaseResponse<IEnumerable<HouseDTO>>> SearchHousesAsync(string searchKey);

        Task<BaseResponse<IEnumerable<HouseDTO>>> SearchByAddressAsync(string? address, double? lat, double? lng, double radiusKm = 10);

        Task<(double lat, double lng)?> GetCoordinatesAsync(string address);

        Task<BaseResponse<IEnumerable<HousingDto>>> GetFilteredHousesAsync(string? address, string? housingType, string? furnishingStatus, decimal? minPrice, decimal? maxPrice);
        Task<BaseResponse<int>> GetHousesCountAsync();
        Task<BaseResponse<IEnumerable<HouseDTO>>> GetPendingHousesAsync();


        //     Task<List<TenantReservationDto>> GetReservationsForTenantAsync(string userId);




    }
}

