using MediatR;
using Saken_WebApplication.Core.Features.Admin.Query.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Admin.Query.Handlers
{
    public class GetUserCountHandler : IRequestHandler<GetUserCountQuery, BaseResponse<int>>
    {
        private readonly IAdminService _adminService;

        public GetUserCountHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<BaseResponse<int>> Handle(GetUserCountQuery request, CancellationToken cancellationToken)
        {
            return await _adminService.GetUserCountAsync();
        }
    }
}
