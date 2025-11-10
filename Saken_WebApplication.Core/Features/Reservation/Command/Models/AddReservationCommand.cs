using MediatR;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Reservation.Command.Models
{
    public record AddReservationCommand(ReservationDto Dto, string UserId) : IRequest<BaseResponse<ReservationResponseDto>>;
}
