using MediatR;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Reservation.Command.Models
{
    public class ConfirmReservationCommand : IRequest<BaseResponse<string>>
    {
        public int ReservationId { get; set; }
        public string LandlordId { get; set; }

        public ConfirmReservationCommand(int reservationId, string landlordId)
        {
            ReservationId = reservationId;
            LandlordId = landlordId;
        }
    }
}
