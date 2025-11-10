using MediatR;
using Saken_WebApplication.Data.DTO.profile;
using Saken_WebApplication.Data.Response;

namespace Saken_WebApplication.Core.Features.Profile.Query.Models
{
    // public record GetLandlordProfileQuery(string LandlordId) : IRequest<BaseResponse<LandlordProfileDto>>;
    public record GetUserProfileQuery(string UserId) : IRequest<BaseResponse<UserProfileDto>>;

}
