using MediatR;
using Saken_WebApplication.Core.Features.Houses.Query.Models;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.housing;

namespace Saken_WebApplication.Core.Features.Houses.Query.Handlers
{
    public class GetHousingsByTypeHandler : IRequestHandler<GetHousingsByTypeQuery, BaseResponse<List<HouseDTO>>>
    {
        private readonly IHousingService _service;
        public GetHousingsByTypeHandler(IHousingService service) => _service = service;

        public async Task<BaseResponse<List<HouseDTO>>> Handle(GetHousingsByTypeQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetHousingsByTypeAsync(request.userId, request.Type);
        }
    }
}
