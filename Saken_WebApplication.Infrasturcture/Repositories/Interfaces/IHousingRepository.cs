using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Interfaces
{
    public interface IHousingRepository
    {
        Task AddHousingAsync(Housing housing);
        Task<InspectionRequestResponseDto> SubmitInspectionRequestAsync(InspectionRequestDto dto);
        Task<bool> SaveHousingAsync(string userId, int housingId);
        Task UpdateAsync(Housing housing);
        Task AddReservationAsync(Reservation reservation);

        Task<List<InspectionSlot>> GetAvailableSlotsByHousingIdAsync(int housingId);
        Task<List<InspectionRequestResponseDto>> GetInspectionRequestsByOwnerAsync(string ownerId);
        Task<IEnumerable<Housing>> GetAllHousesAsync();
        Task<Housing?> GetByIdAsync(int id);

        Task<List<HouseDTO>> GetSavedHousingsAsync(string userId);
        Task<bool> DeleteHousingAsync(int houseId);
        Task<Housing?> GetByIdWithPhotosAsync(int id);
        Task<IEnumerable<Housing>> SearchHousesAsync(string searchKey);
        Task<List<Housing>> GetHousingsForLandlordIdAsync(string landlordId);
        Task<List<Reservation>> GetReservationsForTenantAsync(string userId);

        Task<IEnumerable<Reservation>> GetReservationsByLandlordIdAsync(string landlordId);
        Task<List<Housing>> GetHousingsOrderedByPriceAsync();
        Task<List<Housing>> GetHousingsOrderedByRatingAsync();
        Task<Reservation> GetReservationByIdAsync(int reservationId);
        Task SaveChangesAsync();
    }
}
