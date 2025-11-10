using MediatR;
using Saken_WebApplication.Core.Features.Houses.Command.Models;
using Saken_WebApplication.Core.Features.Reservation.Command.Models;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Reservation.Command.Handlers
{
    public class AddReservationHandler : IRequestHandler<AddReservationCommand,BaseResponse< ReservationResponseDto>>
    {
        private readonly IReservationService _reservationService;

        public AddReservationHandler(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task<BaseResponse<ReservationResponseDto>> Handle(AddReservationCommand request, CancellationToken cancellationToken)
        {
            return await _reservationService.AddReservationAsync(request.Dto, request.UserId);
        }
    }
}
