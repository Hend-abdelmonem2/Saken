using MediatR;
using Saken_WebApplication.Core.Features.Reservation.Command.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Reservation.Command.Handlers
{
    public class ConfirmReservationHandler : IRequestHandler<ConfirmReservationCommand, BaseResponse<string>>
    {
        private readonly IReservationService _reservationService;

        public ConfirmReservationHandler(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task<BaseResponse<string>> Handle(ConfirmReservationCommand request, CancellationToken cancellationToken)
        {

            return await _reservationService.ConfirmReservationAsync(request.ReservationId, request.LandlordId);
        }
            
    }
}
