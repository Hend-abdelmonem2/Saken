using MediatR;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Reservation.Command.Models
{
    public record CancelReservationCommand(int ReservationId, string UserId) : IRequest<BaseResponse<string>>;
}
