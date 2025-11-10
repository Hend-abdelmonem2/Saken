using MediatR;
using Saken_WebApplication.Core.Features.Houses.Base;
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
    public class GetFilteredHousesHandler :BaseHousingHandler, IRequestHandler<GetFilteredHousesQuery, BaseResponse<IEnumerable<HousingDto>>>
    {
        
        public GetFilteredHousesHandler(IHousingService service) :base(service) { } 

        public async Task<BaseResponse<IEnumerable<HousingDto>>> Handle(GetFilteredHousesQuery request, CancellationToken ct)
            => await _service.GetFilteredHousesAsync(
                request.Address,
                request.HousingType,
                request.FurnishingStatus,
                request.MinPrice,
                request.MaxPrice
            );
    }
}
