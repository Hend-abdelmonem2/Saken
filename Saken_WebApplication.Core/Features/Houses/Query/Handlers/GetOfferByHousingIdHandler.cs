using MediatR;
using Saken_WebApplication.Core.Features.Houses.Query.Models;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Houses.Query.Handlers
{
    public class GetOffersByHousingIdHandler : IRequestHandler<GetOffersByHousingIdQuery, BaseResponse<IEnumerable<HousingOffer>>>
    {
        private readonly IHousingOfferService _offerService;

        public GetOffersByHousingIdHandler(IHousingOfferService offerService)
        {
            _offerService = offerService;
        }

        public async Task<BaseResponse<IEnumerable<HousingOffer>>> Handle(GetOffersByHousingIdQuery request, CancellationToken cancellationToken)
        {
            return await _offerService.GetOffersByHousingIdAsync(request.HousingId);
        }
    }
}
