using Saken_WebApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Interfaces
{
    public interface IHousingOfferRepository
    {
        Task<IEnumerable<HousingOffer>> GetActiveOffersAsync();
        Task<IEnumerable<HousingOffer>> GetOffersByHousingIdAsync(int housingId);
        Task AddOfferAsync(HousingOffer offer);
        Task<HousingOffer?> GetByIdAsync(int id);
        Task SaveChangesAsync();
    }
}
