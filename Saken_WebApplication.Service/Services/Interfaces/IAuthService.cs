using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Service.Services.Interfaces
{
    public  interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModelDTO model);
        Task<AuthResponseDto> RefreshTokenAsync(string token);
        Task<string> ForgetPasswordAsync(string email);
        Task<AuthModel> LoginAsync(RequestLoginDto request);
        Task<bool> RevokeTokenAsync(string token);
        Task<string> ResetPasswordAsync(ResetPasswordDto model);
        Task<string> Send2FACodeAsync(string email);
        Task<string> Resend2FACodeAsync(string email);
        Task<string> Verify2FACodeAsync(Verify2FACodeDto model);

        Task UpdateUserAsync(string Id, UpdateUserDto model);
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task UpdateRoleAsync(UpdateRoleDto model);

        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role);
        Task<UserDto> GetUserByIdAsync(string Id);
    }
}
