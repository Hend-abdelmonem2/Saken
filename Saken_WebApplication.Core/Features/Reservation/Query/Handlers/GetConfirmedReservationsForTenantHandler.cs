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
    public class GetConfirmedReservationsForTenantHandler : IRequestHandler<GetConfirmedReservationsForTenantQuery, BaseResponse<List<TenantReservationDto>>>
    {
        private readonly IReservationService _service;

        public GetConfirmedReservationsForTenantHandler(IReservationService service)
        {
            _service = service;
        }

        public async Task<BaseResponse<List<TenantReservationDto>>> Handle(GetConfirmedReservationsForTenantQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetConfirmedReservationsForTenantAsync(request.UserId);
        }
    }
}
