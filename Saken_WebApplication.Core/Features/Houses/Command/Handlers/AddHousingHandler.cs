using MediatR;
using Saken_WebApplication.Core.Features.Houses.Base;
using Saken_WebApplication.Core.Features.Houses.Command.Models;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Houses.Command.Handlers
{
    public class AddHousingHandler : BaseHousingHandler, IRequestHandler<AddHousingCommand>
    {
        public AddHousingHandler(IHousingService service) : base(service)
        {
        }

        #region methods
        public async Task<Unit> Handle(AddHousingCommand request, CancellationToken cancellationToken)
        {
            await _service.AddHousingAsync(request.Dto, request.LandlordId);
            return Unit.Value;
        }
        #endregion

    }
}
