using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Houses.Command.Models
{
    public record SaveHousingCommand(string UserId, int HousingId) : IRequest<bool>;
}
