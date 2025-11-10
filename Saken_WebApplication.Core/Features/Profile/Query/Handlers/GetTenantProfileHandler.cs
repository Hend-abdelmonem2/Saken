using MediatR;
using Saken_WebApplication.Core.Features.Profile.Query.Models;
using Saken_WebApplication.Data.DTO.profile;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Profile.Query.Handlers
{
   public class GetTenantProfileHandler:IRequestHandler <GetTenantProfileQuery, BaseResponse<TenantProfileDto>>
    {
        private readonly IprofileService _profileservice;

        public GetTenantProfileHandler(IprofileService profileservice)
        {
            _profileservice = profileservice;
        }

        public async Task <BaseResponse<TenantProfileDto>> Handle (GetTenantProfileQuery query,CancellationToken cancellationToken)
        {
            return await _profileservice.GetTenantProfileAsync(query.TenantId);
        }
    }
}
