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
        public async Task<IEnumerable<Saken_WebApplication.Data.Models.Housing>> GetAllHousesAsync()
        {
            return await _context.houses
                .Include(h => h.Landlord)         // لو حابة تجيبي بيانات صاحب السكن كمان
                .ToListAsync();
        }
        public async Task<Saken_WebApplication.Data.Models.Housing?> GetByIdAsync(int id)
        {
            return await _context.houses.FindAsync(id);
        }

        public async Task UpdateAsync(Saken_WebApplication.Data.Models.Housing housing)
        {
            _context.houses.Update(housing);
            await _context.SaveChangesAsync();
        }
    }
}
