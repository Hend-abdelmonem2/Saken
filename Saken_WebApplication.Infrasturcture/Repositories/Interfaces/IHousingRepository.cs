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
        Task AddReservationAsync(Reservation reservation);
        Task<IEnumerable<Housing>> GetAllHousesAsync();
        Task<Housing?> GetByIdAsync(int id);
        Task UpdateAsync(Housing housing);
    }
}
