using MediatR;
using Saken_WebApplication.Core.Features.Contacts.Command.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Contacts.Command.Handlers
{
    public class BlockContactHandler : IRequestHandler<BlockContactCommand, BaseResponse<string>>
    {
        private readonly IContactService _contactService;

        public BlockContactHandler(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<BaseResponse<string>> Handle(BlockContactCommand request, CancellationToken cancellationToken)
        {
            return await _contactService.BlockContactAsync(request.ContactId, request.Block);
        }
    }

}
