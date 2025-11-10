using MediatR;
using Saken_WebApplication.Core.Features.Houses.Base;
using Saken_WebApplication.Core.Features.Houses.Command.Handlers;
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
    public class UpdateHousingHandler : BaseHousingHandler ,IRequestHandler<UpdateHousingCommand, BaseResponse>
    {

        public UpdateHousingHandler(IHousingService service) : base(service) { }

        public async Task<BaseResponse> Handle(UpdateHousingCommand request, CancellationToken cancellationToken)
        {
            await _service.UpdateHousingAsync(request.Id, request.Dto, request.UserId);

            return new BaseResponse(true, "تم تحديث السكن");
        }

    }
}




