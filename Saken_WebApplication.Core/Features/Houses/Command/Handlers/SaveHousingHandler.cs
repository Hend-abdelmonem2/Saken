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
   public class SaveHousingHandler : BaseHousingHandler, IRequestHandler<SaveHousingCommand,bool>
    {

        public SaveHousingHandler(IHousingService service): base(service){}

        public async Task<bool> Handle(SaveHousingCommand request, CancellationToken ct)
            => await _service.SaveHousingAsync(request.UserId, request.HousingId);
    }
}

