using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.DTO.profile;
using Saken_WebApplication.Data.Response;

namespace Saken_WebApplication.Service.Services.Interfaces.Profile
{
    public interface IprofileService
    {
        //  Task <BaseResponse<LandlordProfileDto>> GetLandlordProfileAsync(string landlordId);

        Task<BaseResponse<TenantProfileDto>> GetTenantProfileAsync(string tenantId);
        Task<BaseResponse<UserProfileDto>> GetUserProfileAsync(string userId);
        Task<BaseResponse<string>> UpdateProfileAsync(string userId, UpdateUserDto model);
    }
}
