using MediatR;
using Saken_WebApplication.Core.Features.Houses.Base;
using Saken_WebApplication.Core.Features.Houses.Query.Models;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Houses.Query.Handlers
{
    public class GetHousingsByHighestRatingHandler : BaseHousingHandler,IRequestHandler<GetHousingsByHighestRatingQuery, List<HouseDTO>>
    {
       
        public GetHousingsByHighestRatingHandler(IHousingService service):base(service) { }

        public async Task<List<HouseDTO>> Handle(GetHousingsByHighestRatingQuery request, CancellationToken ct)
            => await _service.GetHousingsByHighestRatingAsync();


    }
}
