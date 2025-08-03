using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.DTO.HousingDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces.housing
{
    public interface IHousingService
    {
        Task AddHousingAsync(HousingDto dto, string landlordId);
        Task<InspectionRequestResponseDto> SubmitInspectionRequestAsync(InspectionRequestDto dto);
        Task<bool> SaveHousingAsync(string userId, int housingId);
        Task AddReservationAsync(ReservationDto dto, string userId);
        Task UpdateHousingAsync(int id, HousingDto dto);
        Task<(bool success, string message, bool isFrozen)> ToggleFreezeAsync(int id);


        Task<List<InspectionSlotDetailsDto>> GetAvailableSlotsAsync(int housingId);
        Task<List<InspectionRequestResponseDto>> GetInspectionRequestsForOwnerAsync(string ownerId);
        Task<IEnumerable<HouseDTO>> GetAllHousesAsync();

        Task<IEnumerable<HouseDTO>> SearchHousesAsync(string searchKey);

        Task<bool> DeleteHousingAsync(int houseId);
        Task<OwnerHousingGroupedDto> GetGroupedHousingsForLandlordAsync(string landlordId);
        Task<List<TenantReservationDto>> GetReservationsForTenantAsync(string userId);

        Task<List<HouseDTO>> GetHousingsByLowestPriceAsync();
        Task<List<HouseDTO>> GetHousingsByHighestRatingAsync();
        Task<HousingDetailsDto?> GetHousingByIdAsync(int id);
        Task<List<HouseDTO>> GetSavedHousingsAsync(string userId);

    }
}

