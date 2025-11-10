using MediatR;
using Saken_WebApplication.Core.Features.Reservation.Query.Models;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Reservation.Query.Handlers
{
    public class GetHousingCostsHandler : IRequestHandler<GetHousingCostsQuery,BaseResponse<HousingCostsDto>>
    {
        private readonly IReservationService _service;

        public GetHousingCostsHandler(IReservationService service)
        {
            _service = service;
        }

        public async Task<BaseResponse<HousingCostsDto>> Handle(GetHousingCostsQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetHousingCostsAsync(request.HousingId, request.DurationInMonths);
        }
    }
}
