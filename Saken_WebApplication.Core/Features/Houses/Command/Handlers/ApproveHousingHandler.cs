using MediatR;
using Saken_WebApplication.Core.Features.Houses.Command.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saken_WebApplication.Core.Features.Houses.Base;

namespace Saken_WebApplication.Core.Features.Houses.Command.Handlers
{
    public class ApproveHousingHandler :BaseHousingHandler, IRequestHandler<ApproveHousingCommand, BaseResponse<bool>>
    {

        public ApproveHousingHandler(IHousingService service) : base(service) { }

        public async Task<BaseResponse<bool>> Handle(ApproveHousingCommand request, CancellationToken ct)
            => await _service.ApproveHousingAsync(request.HousingId);
    }
}
