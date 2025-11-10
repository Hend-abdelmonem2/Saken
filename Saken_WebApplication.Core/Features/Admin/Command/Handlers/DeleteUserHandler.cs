using MediatR;
using Saken_WebApplication.Core.Features.Admin.Command.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Admin.Command.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, BaseResponse<bool>>
    {
        private readonly IAdminService _adminService;

        public DeleteUserHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return await _adminService.DeleteUserAsync(request.UserId);
        }
    }
}
