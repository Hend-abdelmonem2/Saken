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
    public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, BaseResponse<string>>
    {
        private readonly IAuthService _authService;
        public UpdateRoleHandler(IAuthService authService) => _authService = authService;

        public async Task<BaseResponse<string>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
            => await _authService.UpdateRoleAsync(request.Model);
    }
}
