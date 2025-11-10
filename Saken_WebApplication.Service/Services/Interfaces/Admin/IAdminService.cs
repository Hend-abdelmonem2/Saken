using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces.Admin
{
    public interface IAdminService
    {
        Task<BaseResponse<List<UserDto>>> GetAllUsersAsync();
        Task<BaseResponse<UserDto>> GetUserByIdAsync(string id);
        Task<BaseResponse<List<UserDto>>> GetUsersByRoleAsync(string roleName);
        Task<BaseResponse<bool>> UpdateUserAsync(string userId, UpdateUserDto model);
        Task<BaseResponse<bool>> DeleteUserAsync(string userId);
        Task<BaseResponse<bool>> FreezeUserAsync(string userId);
        Task<BaseResponse<bool>> UnfreezeUserAsync(string userId);
      
        Task<BaseResponse<int>> GetUserCountAsync();
        
        
        Task<BaseResponse<int>> GetNotificationsCountAsync();
        Task<BaseResponse<int>> GetReservationCountAsync();
    }
}
