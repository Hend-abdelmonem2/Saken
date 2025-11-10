using MediatR;
using Saken_WebApplication.Core.Features.Houses.Base;
using Saken_WebApplication.Core.Features.Houses.Query.Models;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.housing;

namespace Saken_WebApplication.Core.Features.Houses.Query.Handlers
{
    public class GetHousingsByHighestRatingHandler : BaseHousingHandler, IRequestHandler<GetHousingsByHighestRatingQuery, BaseResponse<List<HouseDTO>>>
    {

        public GetHousingsByHighestRatingHandler(IHousingService service) : base(service) { }

        public async Task<BaseResponse<List<HouseDTO>>> Handle(GetHousingsByHighestRatingQuery request, CancellationToken ct)
            => await _service.GetHousingsByHighestRatingAsync(request.userId);


    }
}
