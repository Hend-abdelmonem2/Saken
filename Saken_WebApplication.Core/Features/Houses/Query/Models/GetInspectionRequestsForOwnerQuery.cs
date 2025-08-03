using MediatR;
using Saken_WebApplication.Data.DTO.HousingDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Houses.Query.Models
{
    public record GetInspectionRequestsForOwnerQuery(string OwnerId) : IRequest<List<InspectionRequestResponseDto>>;

}
