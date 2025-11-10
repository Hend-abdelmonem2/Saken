using MediatR;
using Saken_WebApplication.Data.DTO.profile;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Profile.Query.Models
{
    public record GetTenantProfileQuery(string TenantId) : IRequest<BaseResponse<TenantProfileDto>>;
}
