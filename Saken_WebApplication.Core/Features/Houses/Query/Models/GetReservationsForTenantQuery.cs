using MediatR;
using Saken_WebApplication.Data.DTO.HousingDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Houses.Query.Models
{
    public record GetReservationsForTenantQuery(string UserId) : IRequest<List<TenantReservationDto>>;
}
