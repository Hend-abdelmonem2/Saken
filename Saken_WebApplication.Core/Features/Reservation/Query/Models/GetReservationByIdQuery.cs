using MediatR;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Reservation.Query.Models
{
    public record GetReservationByIdQuery(int Id) : IRequest<BaseResponse<GetReservationDto?>>;
}
