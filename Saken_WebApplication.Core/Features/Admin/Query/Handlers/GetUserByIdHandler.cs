using MediatR;
using Saken_WebApplication.Core.Features.Admin.Query.Models;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Admin.Query.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, BaseResponse<UserDto>>
    {
        private readonly IAdminService _adminService;

        public GetUserByIdHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<BaseResponse<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _adminService.GetUserByIdAsync(request.UserId);
        }
    }
}
