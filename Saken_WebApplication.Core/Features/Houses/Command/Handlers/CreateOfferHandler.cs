using MediatR;
using Saken_WebApplication.Core.Features.Houses.Command.Models;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using Saken_WebApplication.Service.Services.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Houses.Command.Handlers
{
    // Handler
    public class CreateOfferHandler : IRequestHandler<CreateOfferCommand, BaseResponse<string>>
    {
        private readonly IHousingOfferService _offerService;

        public CreateOfferHandler(IHousingOfferService offerService)
        {
            _offerService = offerService;
        }

        public async Task<BaseResponse<string>> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
        {
            return await _offerService.CreateOfferAsync(request.OfferDto);
        }
    }

}
