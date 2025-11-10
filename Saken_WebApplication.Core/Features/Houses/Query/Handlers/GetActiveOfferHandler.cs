using MediatR;
using Saken_WebApplication.Core.Features.Houses.Query.Models;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Houses.Query.Handlers
{
    public class GetActiveOffersHandler : IRequestHandler<GetActiveOffersQuery, BaseResponse<IEnumerable<OfferDto>>>
    {
        private readonly IHousingOfferService _offerService;

        public GetActiveOffersHandler(IHousingOfferService offerService)
        {
            _offerService = offerService;
        }

        public async Task<BaseResponse<IEnumerable<OfferDto>>> Handle(GetActiveOffersQuery request, CancellationToken cancellationToken)
        {
            return await _offerService.GetActiveOffersAsync();
        }
    }
}
