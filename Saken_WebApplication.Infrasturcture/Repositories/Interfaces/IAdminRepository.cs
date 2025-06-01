using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Interfaces
{
   public interface IAdminRepository
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(string id);
        Task<List<UserDto>> GetUsersByRoleAsync(string roleName);

        Task<(bool IsSuccess, string Message)> UpdateUserAsync(string userId,UpdateUserDto model);
        Task<string> DeleteUserAsync(string userId);
        Task<string> FreezeUserAsync(string userId);
        Task<string> UnfreezeUserAsync(string userId);
    }
}
