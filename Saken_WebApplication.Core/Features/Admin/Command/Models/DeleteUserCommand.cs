using MediatR;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Admin.Command.Models
{
    public record DeleteUserCommand(string UserId) : IRequest<BaseResponse<bool>>;
}
