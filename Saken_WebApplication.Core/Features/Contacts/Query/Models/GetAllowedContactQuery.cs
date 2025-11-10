using MediatR;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Contacts.Query.Models
{
    public record GetAllowedContactsQuery(string OwnerUserId)
     : IRequest<BaseResponse<IEnumerable<Contact>>>;
}
