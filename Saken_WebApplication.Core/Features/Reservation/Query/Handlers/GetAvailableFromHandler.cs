using MediatR;
using Saken_WebApplication.Core.Features.Reservation.Query.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Reservation.Query.Handlers
{
    public class GetAvailableFromHandler : IRequestHandler<GetAvailableFromQuery,BaseResponse<DateTime?>>
    {
        private readonly IReservationService _service;

        public GetAvailableFromHandler(IReservationService service)
        {
            _service = service;
        }

        public async Task<BaseResponse<DateTime?>> Handle(GetAvailableFromQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetAvailableFromAsync(request.HousingId);
        }
    }
}
