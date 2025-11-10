using MediatR;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Contacts.Command.Models
{
    public record BlockContactCommand(int ContactId, bool Block)
    : IRequest<BaseResponse<string>>;
}
