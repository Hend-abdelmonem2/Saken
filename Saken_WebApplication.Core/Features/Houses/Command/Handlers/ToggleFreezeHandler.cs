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
    public class ToggleFreezeHandler : BaseHousingHandler, IRequestHandler<ToggleFreezeCommand, (bool, string, bool)>
    {

        public ToggleFreezeHandler(IHousingService service):base(service) { }

        public async Task<(bool, string, bool)> Handle(ToggleFreezeCommand request, CancellationToken ct)
            => await _service.ToggleFreezeAsync(request.Id);

    }
}
