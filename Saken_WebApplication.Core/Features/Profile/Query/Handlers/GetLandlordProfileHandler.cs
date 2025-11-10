using MediatR;
using Saken_WebApplication.Core.Features.Profile.Query.Models;
using Saken_WebApplication.Data.DTO.profile;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Profile;

namespace Saken_WebApplication.Core.Features.Profile.Query.Handlers
{
    public class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, BaseResponse<UserProfileDto>>
    {
        private readonly IprofileService _profileservice;

        public GetUserProfileHandler(IprofileService profileservice)
        {
            _profileservice = profileservice;
        }


        public async Task<BaseResponse<UserProfileDto>> Handle(GetUserProfileQuery getUserProfileQuery, CancellationToken cancellationToken)
        {
            return await _profileservice.GetUserProfileAsync(getUserProfileQuery.UserId);
        }
    }
}
