using MediatR;
using Saken_WebApplication.Core.Features.Contacts.Query.Models;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Contacts.Query.Handlers
{
    public class GetAllowedContactsHandler : IRequestHandler<GetAllowedContactsQuery, BaseResponse<IEnumerable<Contact>>>
    {
        private readonly IContactService _contactService;

        public GetAllowedContactsHandler(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<BaseResponse<IEnumerable<Contact>>> Handle(GetAllowedContactsQuery request, CancellationToken cancellationToken)
        {
            return await _contactService.GetAllowedContactsAsync(request.OwnerUserId);
        }
    }

}
