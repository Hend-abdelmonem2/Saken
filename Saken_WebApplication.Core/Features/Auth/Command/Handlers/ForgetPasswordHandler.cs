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
    public class ForgetPasswordHandler : IRequestHandler<ForgetPasswordCommand, BaseResponse<string>>
    {
        private readonly IAuthService _authService;
        public ForgetPasswordHandler(IAuthService authService) => _authService = authService;

        public async Task<BaseResponse<string>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
            => await _authService.ForgetPasswordAsync(request.Email);
    }
}
