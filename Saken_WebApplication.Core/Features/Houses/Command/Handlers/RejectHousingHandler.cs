using MediatR;
using Saken_WebApplication.Core.Features.Houses.Base;
using Saken_WebApplication.Core.Features.Houses.Command.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Houses.Command.Handlers
{
    public class RejectHousingHandler :BaseHousingHandler, IRequestHandler<RejectHousingCommand, BaseResponse<string>>
    {
        
        public RejectHousingHandler(IHousingService service) : base (service) { }

        public async Task<BaseResponse<string>> Handle(RejectHousingCommand request, CancellationToken ct)
            => await _service.RejectHouseAsync(request.HousingId, request.Reason);
    }
}
