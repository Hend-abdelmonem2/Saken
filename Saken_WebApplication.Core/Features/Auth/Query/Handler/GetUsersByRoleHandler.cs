using MediatR;
using Saken_WebApplication.Core.Features.Auth.Query.Models;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces;

namespace Saken_WebApplication.Core.Features.Auth.Query.Handler
{
    public class GetUsersByRoleHandler : IRequestHandler<GetUsersByRoleQuery, BaseResponse<IEnumerable<UserDto>>>
    {
        private readonly IAuthService _authService;
        public GetUsersByRoleHandler(IAuthService authService) => _authService = authService;

        public async Task<BaseResponse<IEnumerable<UserDto>>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
            => await _authService.GetUsersByRoleAsync(request.userId, request.Role);
    }
}
