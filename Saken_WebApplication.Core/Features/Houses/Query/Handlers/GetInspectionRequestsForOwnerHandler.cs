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
   public class GetInspectionRequestsForOwnerHandler :
        BaseHousingHandler,IRequestHandler<GetInspectionRequestsForOwnerQuery, List<InspectionRequestResponseDto>>
    {

        public GetInspectionRequestsForOwnerHandler(IHousingService service) :base(service) { }

        public async Task<List<InspectionRequestResponseDto>> Handle(GetInspectionRequestsForOwnerQuery request, CancellationToken ct)
            => await _service.GetInspectionRequestsForOwnerAsync(request.OwnerId);

    }
}
