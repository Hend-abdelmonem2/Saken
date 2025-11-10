using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;
using Saken_WebApplication;
using Saken_WebApplication.Data.Response;

namespace Saken_WebApplication.Infrasturcture.Repositories.Interfaces
{
    public interface IHousingRepository
    {
        Task<BaseResponse<string>> AddHousingAsync(Housing housing);
        Task<BaseResponse<InspectionRequestResponseDto>> SubmitInspectionRequestAsync(InspectionRequestDto dto);
        Task<BaseResponse<bool>>SaveHousingAsync(string userId, int housingId);
        Task UpdateAsync(Housing housing);

        Task<InspectionSlot> GetSlotWithHousingAsync(int slotId);

        Task<List<InspectionSlot>> GetAvailableSlotsByHousingIdAsync(int housingId);
        Task<List<InspectionRequestResponseDto>> GetInspectionRequestsByOwnerAsync(string ownerId);
        Task<IEnumerable<Housing>> GetAllHousesAsync();
        Task<Housing?> GetByIdAsync(int id);

        Task<Housing> GetHousingCostsByIdAsync(int housingId);
        Task<IEnumerable<Saken_WebApplication.Data.Models.Housing>> GetByOwnerIdAsync(string ownerId);


        Task<List<Housing>> GetHousingsByTypeAsync(PropertyType type);

        Task<List<HouseDTO>> GetSavedHousingsAsync(string userId);
        Task<bool> DeleteHousingAsync(int houseId);
        Task<Housing?> GetByIdWithPhotosAsync(int id);
        Task<List<Housing>> GetHousingsForLandlordIdAsync(string landlordId);

        Task<List<Housing>> GetHousingsOrderedByPriceAsync();
        Task<List<Housing>> GetHousingsOrderedByRatingAsync();
        Task<DateTime?> GetAvailableFromAsync(int housingId);
        Task<IEnumerable<Housing>> SearchHousesAsync(string searchKey);

        Task<IEnumerable<Housing>> SearchByAddressAsync(string? address, double? lat, double? lng, double radiusKm = 10);

        Task<List<Housing>> FilterHousesAsync(string? address, string? housingType, string? furnishingStatus, decimal? minPrice, decimal? maxPrice);




        Task ApproveHousingAsync(int id);
        Task RejectHouseAsync(int Id, string reason);

        Task<IEnumerable<Housing>> GetPendingHousesAsync();
        Task SaveChangesAsync();
    }
}
