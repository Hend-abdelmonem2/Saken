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
    public class SearchByAddressHandler :BaseHousingHandler, IRequestHandler<SearchByAddressQuery, BaseResponse<IEnumerable<HouseDTO>>>
    {
       
        public SearchByAddressHandler(IHousingService service) :base(service) { }

        public async Task<BaseResponse<IEnumerable<HouseDTO>>> Handle(SearchByAddressQuery request, CancellationToken ct)
            => await _service.SearchByAddressAsync(request.Address, request.Lat, request.Lng, request.RadiusKm);
    }
}
