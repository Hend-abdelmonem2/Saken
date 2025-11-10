using MediatR;
using Saken_WebApplication.Core.Features.Contacts.Base;
using Saken_WebApplication.Core.Features.Contacts.Command.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Implement.Contact;
using Saken_WebApplication.Service.Services.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Contacts.Command.Handlers
{
    public class AddContactHandler :IRequestHandler<AddContactCommand, BaseResponse<string>>
    {
       
        private readonly IContactService _contactService;
        public AddContactHandler(IContactService service)
        {
            _contactService = service;
        }
       

        public async Task<BaseResponse<string>> Handle(AddContactCommand request, CancellationToken cancellationToken)
        {
            return await _contactService.AddContactAsync(request.OwnerUserId, request.TargetUserId);
        }
    }
}
