using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;

namespace Saken_WebApplication.Service.Services.Interfaces
{
    public interface IAuthService
    {
        Task<BaseResponse<AuthModel>> RegisterAsync(RegisterModelDTO model);
        Task<BaseResponse<AuthResponseDto>> RefreshTokenAsync(string token);
        Task<BaseResponse<string>> ForgetPasswordAsync(string email);
        Task<BaseResponse<AuthModel>> LoginAsync(RequestLoginDto request);
        Task<BaseResponse> LogoutAsync(string? accessToken, string userId);
        Task<BaseResponse<bool>> RevokeTokenAsync(string token);
        Task<BaseResponse<string>> ResetPasswordAsync(ResetPasswordDto model);
        Task<BaseResponse<string>> Send2FACodeAsync(string email);
        Task<BaseResponse<string>> Resend2FACodeAsync(string email);
        Task<BaseResponse<string>> Verify2FACodeAsync(Verify2FACodeDto model);

        Task<BaseResponse<string>> UpdateProfileAsync(string userId, UpdateUserDto model);
        Task<BaseResponse<IEnumerable<UserDto>>> GetUsersAsync(string userId);
        Task<BaseResponse<string>> UpdateRoleAsync(UpdateRoleDto model);

        Task<BaseResponse<IEnumerable<UserDto>>> GetUsersByRoleAsync(string userId, string role);
        Task<BaseResponse<UserDto>> GetUserByIdAsync(string Id);
    }
}
