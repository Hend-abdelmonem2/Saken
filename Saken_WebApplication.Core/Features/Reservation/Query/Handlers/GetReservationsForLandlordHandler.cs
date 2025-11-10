using MediatR;
using Saken_WebApplication.Core.Features.Reservation.Query.Models;
using Saken_WebApplication.Data.DTO;
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
    public class GetReservationsForLandlordHandler : IRequestHandler<GetReservationsForLandlordQuery,BaseResponse<IEnumerable<GetReservationDto>>>
    {
        private readonly IReservationService _service;

        public GetReservationsForLandlordHandler(IReservationService service)
        {
            _service = service;
        }

        public async Task<BaseResponse<IEnumerable<GetReservationDto>>> Handle(GetReservationsForLandlordQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetReservationsForLandlordAsync(request.LandlordId);
        }
    }
}
