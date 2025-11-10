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
    public class GetReservationByIdHandler : IRequestHandler<GetReservationByIdQuery,BaseResponse<GetReservationDto?>>
    {
        private readonly IReservationService _reservationService;

        public GetReservationByIdHandler(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task<BaseResponse<GetReservationDto?>> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
        {
            return await _reservationService.GetReservationById(request.Id);
        }
    }
}
