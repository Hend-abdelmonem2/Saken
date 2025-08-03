using MediatR;
using Saken_WebApplication.Core.Features.Houses.Base;
using Saken_WebApplication.Core.Features.Houses.Command.Models;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Houses.Command.Handlers
{
    public class SubmitInspectionRequestHandler : BaseHousingHandler, IRequestHandler<SubmitInspectionRequestCommand, InspectionRequestResponseDto>
    {

        public SubmitInspectionRequestHandler(IHousingService service):base(service) { }

        public async Task<InspectionRequestResponseDto> Handle(SubmitInspectionRequestCommand request, CancellationToken ct)
            => await _service.SubmitInspectionRequestAsync(request.Dto);
    }
}
