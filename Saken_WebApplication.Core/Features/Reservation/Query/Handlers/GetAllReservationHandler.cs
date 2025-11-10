using MediatR;
using Saken_WebApplication.Core.Features.Reservation.Query.Models;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Reservation.Query.Handlers
{
   public class GetAllReservationHandler :IRequestHandler<GetAllReservationQuery, BaseResponse<List<AllReservationDto>>>
    {
        private readonly IReservationService _reservationService;

        public GetAllReservationHandler(IReservationService reservationService)
        {
             _reservationService = reservationService;
        }

        public async Task<BaseResponse<List<AllReservationDto>>> Handle(GetAllReservationQuery request, CancellationToken cancellationToken)
        {
            return await _reservationService.GetAllReservationsAsync();
        }
    }
}
