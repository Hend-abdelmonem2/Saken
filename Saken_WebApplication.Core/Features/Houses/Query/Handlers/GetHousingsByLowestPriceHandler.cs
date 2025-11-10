using MediatR;
using Saken_WebApplication.Core.Features.Houses.Base;
using Saken_WebApplication.Core.Features.Houses.Query.Models;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.housing;

namespace Saken_WebApplication.Core.Features.Houses.Query.Handlers
{
    public class GetHousingsByLowestPriceHandler : BaseHousingHandler, IRequestHandler<GetHousingsByLowestPriceQuery, BaseResponse<List<HouseDTO>>>
    {

        public GetHousingsByLowestPriceHandler(IHousingService service) : base(service) { }

        public async Task<BaseResponse<List<HouseDTO>>> Handle(GetHousingsByLowestPriceQuery request, CancellationToken ct)
            => await _service.GetHousingsByLowestPriceAsync(request.userId);

    }
}
