using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces.recommend
{
    public  interface IRecommendationService
    {
        Task<BaseResponse<IEnumerable<HousingRecommendationDto>>> GetRecommendedHousesAsync();
        Task<BaseResponse<IEnumerable<UserRecommendationDto>>> GetRecommendedUsersAsync();
    }
}
