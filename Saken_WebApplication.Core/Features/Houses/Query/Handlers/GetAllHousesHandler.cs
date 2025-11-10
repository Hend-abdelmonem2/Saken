using MediatR;
using Saken_WebApplication.Core.Features.Houses.Base;
using Saken_WebApplication.Core.Features.Houses.Query.Models;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.housing;

namespace Saken_WebApplication.Core.Features.Houses.Query.Handlers
{
    public class GetAllHousesHandler
     : BaseHousingHandler, IRequestHandler<GetAllHousesQuery, BaseResponse<IEnumerable<HouseDTO>>>
    {
        public GetAllHousesHandler(IHousingService service) : base(service) { }

        public async Task<BaseResponse<IEnumerable<HouseDTO>>> Handle(GetAllHousesQuery request, CancellationToken ct)
            => await _service.GetAllHousesAsync(request.userId);
    }

}
