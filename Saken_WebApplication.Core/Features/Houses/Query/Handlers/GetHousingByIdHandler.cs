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
    public class GetHousingByIdHandler : BaseHousingHandler,IRequestHandler<GetHousingByIdQuery, HousingDetailsDto?>
    {

        public GetHousingByIdHandler(IHousingService service) :base(service) { }

        public async Task<HousingDetailsDto?> Handle(GetHousingByIdQuery request, CancellationToken ct)
            => await _service.GetHousingByIdAsync(request.Id);

    }
}
