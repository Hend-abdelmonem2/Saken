using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.DTO.Favorite;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces.Like
{
    public  interface ILikeService
    {
        Task<BaseResponse<string>> ToggleLikeAsync(string userId, string entityId, string entityType);
        Task<BaseResponse<List<HousingLikeDto>>> GetLikedHousesAsync(string userId);
        Task<BaseResponse<List<UserLikeDto>>> GetLikedUsersAsync(string userId);

    }
}
