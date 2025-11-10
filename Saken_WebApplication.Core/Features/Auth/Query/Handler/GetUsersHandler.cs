using MediatR;
using Saken_WebApplication.Core.Features.Auth.Query.Models;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces;

namespace Saken_WebApplication.Core.Features.Auth.Query.Handler
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, BaseResponse<IEnumerable<UserDto>>>
    {
        private readonly IAuthService _authService;
        public GetUsersHandler(IAuthService authService) => _authService = authService;

        public async Task<BaseResponse<IEnumerable<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            => await _authService.GetUsersAsync(request.userId);
    }

}
