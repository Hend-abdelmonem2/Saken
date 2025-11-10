using MediatR;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Likes.Command.Models
{
    public record ToggleLikeCommand(string EntityId, string EntityType)
     : IRequest<BaseResponse<string>>;
}
