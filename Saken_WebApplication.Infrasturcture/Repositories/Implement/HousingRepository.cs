using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Infrasturcture.Data;
using static Saken_WebApplication.Data.Models.Housing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Data.Models;

namespace Saken_WebApplication.Infrasturcture.Repositories.Implement
{
    public class HousingRepository : IHousingRepository
    {
        private readonly ApplicationDBContext _context;

        public HousingRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task AddHousingAsync(Saken_WebApplication.Data.Models.Housing housing)
        {
            await _context.houses.AddAsync(housing);
            await _context.SaveChangesAsync();
        }
        public async Task AddReservationAsync(Reservation reservation)
        {
            await _context.reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();


        }
        public async Task UpdateAsync(Saken_WebApplication.Data.Models.Housing housing)
        {
            _context.houses.Update(housing);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Saken_WebApplication.Data.Models.Housing>> GetAllHousesAsync()
        {         
            var houses = await _context.houses
             .Where(h => !h.IsFrozen)
              .Include(h => h.Landlord)
              .ToListAsync();
            return houses;

        }
        public async Task<Saken_WebApplication.Data.Models.Housing> GetHouseWithLandlordAsync(int housingId)
        {
            return await _context.houses
                .Include(h => h.Landlord)
                .FirstOrDefaultAsync(h => h.h_Id == housingId);
        }
        public async Task<Saken_WebApplication.Data.Models.Housing?> GetByIdAsync(int id)
        {
            return await _context.houses
         .Include(h => h.Landlord) // نجيب بيانات المؤجر كمان
         .FirstOrDefaultAsync(h => h.h_Id == id);
        }

        public async Task<IEnumerable<Saken_WebApplication.Data.Models.Housing>> SearchHousesAsync(string searchKey)
        {
            var houses = await _context.houses.ToListAsync();

            return houses.Where(h =>
                h.address.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
                h.price.ToString().Contains(searchKey) ||
                h.type.ToString().Contains(searchKey) ||
                h.status.ToString().Contains(searchKey) ||
                h.furnishingStatus.ToString().Contains(searchKey)
            );
        }
        public async Task<IEnumerable<Saken_WebApplication.Data.Models.Reservation>> GetReservationsByLandlordIdAsync(string landlordId)
        {
            return await _context.reservations
                .Include(r => r.Housing)
                .Include(r => r.Tenant)
                .Where(r => r.LandlordId == landlordId)
                .ToListAsync();
        }
        public async Task<bool> DeleteHousingAsync(int  houseId)
        {
            var house = await _context.houses.FindAsync(houseId);
            if (house == null)
                return false;

            _context.houses.Remove(house);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Saken_WebApplication.Data.Models.Housing>> GetHousingsForLandlordIdAsync(string landlordId)
        {
            return await _context.houses
                .Where(h => h.LandlordId == landlordId)
           
                .ToListAsync();
        }
        public async Task<Saken_WebApplication.Data.Models.Reservation> GetReservationByIdAsync(int reservationId)
        {
            return await _context.reservations
                .Include(r => r.Housing)
                .ThenInclude(h => h.Landlord)
                .Include(r => r.Tenant)
                .FirstOrDefaultAsync(r => r.res_Id == reservationId);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
