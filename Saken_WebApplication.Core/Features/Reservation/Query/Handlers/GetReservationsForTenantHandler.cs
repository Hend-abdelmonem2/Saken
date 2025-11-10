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
    public class GetReservationsForTenantHandler : IRequestHandler<GetReservationsForTenantQuery,BaseResponse<List<TenantReservationDto>>>
    {
        private readonly IReservationService _service;

        public GetReservationsForTenantHandler(IReservationService service)
        {
            _service = service;
        }

        public async Task<BaseResponse<List<TenantReservationDto>>> Handle(GetReservationsForTenantQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetReservationsForTenantAsync(request.TenantId);
        }
    }
}
