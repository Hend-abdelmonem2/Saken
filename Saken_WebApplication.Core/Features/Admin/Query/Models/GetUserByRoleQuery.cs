using MediatR;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Admin.Query.Models
{
    public record GetUsersByRoleQuery(string RoleName) : IRequest<BaseResponse<List<UserDto>>>;
}
