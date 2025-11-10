using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Implement
{
    public class HousingOfferRepository: IHousingOfferRepository
    {
        private readonly ApplicationDBContext _context;

        public HousingOfferRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HousingOffer>> GetActiveOffersAsync()
        {
            return await _context.housingOffer
                .Where(o => DateTime.Now >= o.StartDate && DateTime.Now <= o.EndDate)
                .Include(o => o.Housing)
                .ToListAsync();
        }

        public async Task<IEnumerable<HousingOffer>> GetOffersByHousingIdAsync(int housingId)
        {
            return await _context.housingOffer
                .Where(o => o.HousingId == housingId)
                .ToListAsync();
        }

        public async Task AddOfferAsync(HousingOffer offer)
        {
            await _context.housingOffer.AddAsync(offer);
        }

        public async Task<HousingOffer?> GetByIdAsync(int id)
        {
            return await _context.housingOffer.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

