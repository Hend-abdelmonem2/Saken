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
   public class DeleteHousingHandler : BaseHousingHandler, IRequestHandler<DeleteHousingCommand, bool>
    {

        public DeleteHousingHandler(IHousingService service) :base(service) { }

        public async Task<bool> Handle(DeleteHousingCommand request, CancellationToken ct)
            => await _service.DeleteHousingAsync(request.Id);
    }

}
