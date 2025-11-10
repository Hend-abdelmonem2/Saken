using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Infrasturcture.Repositories.Implement.Reservation
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDBContext _context;

        public ReservationRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Saken_WebApplication.Data.Models.Housing> GetHousingWithDetailsAsync(int housingId)
        {
            return await _context.houses
                .Include(h => h.Owner)
                .Include(h => h.Photos)
                .FirstOrDefaultAsync(h => h.Id == housingId);
        }

        public async Task<bool> IsOverlappingReservationAsync(int housingId, DateTime startDate, DateTime endDate)
        {
            return await _context.reservations
                .Where(r => r.HousingId == housingId &&
                            r.Status == ReservationStatus.Confirmed &&
                            ((startDate >= r.StartDateTime && startDate < r.EndDateTime) ||
                             (endDate > r.StartDateTime && endDate <= r.EndDateTime)))
                .AnyAsync();
        }

        public async Task AddReservationAsync(Saken_WebApplication.Data.Models.Reservation reservation)
        {
            await _context.reservations.AddAsync(reservation);
        }

        public async Task<List<Saken_WebApplication.Data.Models.Reservation>> GetReservationsForTenantAsync(string userId)
        {
            return await _context.reservations
                .Include(r => r.Housing)
                    .ThenInclude(h => h.Owner)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Saken_WebApplication.Data.Models.Reservation>> GetConfirmedReservationsForTenantAsync(string userId)
        {
            return await _context.reservations
                .Include(r => r.Housing)
                    .ThenInclude(h => h.Owner)
                .Where(r => r.UserId == userId && r.Status == ReservationStatus.Confirmed)
                .ToListAsync();
        }

        public async Task<Saken_WebApplication.Data.Models.Reservation> GetReservationByIdAsync(int reservationId)
        {
            return await _context.reservations
                .Include(r => r.Housing)
                .ThenInclude(h => h.Owner)
                .Include(r => r.Tenant)
                .FirstOrDefaultAsync(r => r.res_Id == reservationId);
        }

        public async Task<IEnumerable<Saken_WebApplication.Data.Models.Reservation>> GetReservationsByLandlordIdAsync(string landlordId)
        {
            return await _context.reservations
                .Include(r => r.Housing)
                .Include(r => r.Tenant)
                .Where(r => r.LandlordId == landlordId)
                .ToListAsync();
        }

        public async Task UpdateStatusAsync(int reservationId, ReservationStatus status, DateTime? endDateTime = null)
        {
            var reservation = await _context.reservations.FindAsync(reservationId);
            if (reservation != null)
            {
                reservation.Status = status;

                if (status == ReservationStatus.Confirmed && endDateTime.HasValue)
                {
                    var housing = await _context.houses.FindAsync(reservation.HousingId);
                    if (housing != null)
                        housing.LastRentedDate = endDateTime.Value;
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Saken_WebApplication.Data.Models.Reservation>> GetAllReservationsAsync()
        {
            return await _context.reservations
                .Include(r => r.Housing)
                 .ThenInclude(h => h.Owner)
                .Include(r => r.Tenant)
                .ToListAsync();
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetReservationsCountAsync()
        {
            return await _context.reservations.CountAsync();
        }
    }
}
