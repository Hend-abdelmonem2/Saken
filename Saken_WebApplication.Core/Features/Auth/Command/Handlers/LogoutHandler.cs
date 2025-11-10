using MediatR;
using Saken_WebApplication.Core.Features.Auth.Command.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Auth.Command.Handlers
{
    public class LogoutHandler : IRequestHandler<LogoutCommand, BaseResponse>
    {
        private readonly IAuthService _authService;

        public LogoutHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<BaseResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            return await _authService.LogoutAsync(request.AccessToken, request.UserId);
        }
    }
}
