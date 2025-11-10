using Saken_WebApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Reservation
{
    public interface IReservationRepository
    {
        Task<Housing> GetHousingWithDetailsAsync(int housingId);
        Task<bool> IsOverlappingReservationAsync(int housingId, DateTime startDate, DateTime endDate);
        Task AddReservationAsync(Saken_WebApplication.Data.Models.Reservation reservation);


        Task<List<Saken_WebApplication.Data.Models.Reservation>> GetReservationsForTenantAsync(string userId);

        Task<IEnumerable<Saken_WebApplication.Data.Models.Reservation>> GetReservationsByLandlordIdAsync(string landlordId);

        Task<Saken_WebApplication.Data.Models.Reservation> GetReservationByIdAsync(int reservationId);
        Task UpdateStatusAsync(int reservationId, ReservationStatus status, DateTime? endDateTime = null);

        Task<List<Saken_WebApplication.Data.Models.Reservation>> GetConfirmedReservationsForTenantAsync(string userId);
        Task<int> GetReservationsCountAsync();
        Task<List<Saken_WebApplication.Data.Models.Reservation>> GetAllReservationsAsync();
        Task SaveChangesAsync();
    }
}
