using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces
{
    public  interface IUserService
    {
        Task<BaseResponse<List<UserDto>>> FilterUsersAsync(string? name, string? roleName);
        Task<BaseResponse<List<UserDto>>> SearchUsersAsync(string keyword);
    }
}
