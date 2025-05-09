using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces
{
   public interface IHousingFilterService
    {
        Task<List<HousingDto>> GetRecommendedHousingsAsync(string userId , string? location = null);
    }
}
