using MediatR;
using Saken_WebApplication.Core.Features.Reservation.Command.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Reservation.Command.Handlers
{
    public class SendAvailabilityNotificationHandler : IRequestHandler<SendAvailabilityNotificationCommand, BaseResponse<string>>
    {
        private readonly IReservationService _service;

        public SendAvailabilityNotificationHandler(IReservationService service)
        {
            _service = service;
        }

        public async Task<BaseResponse<string>> Handle(SendAvailabilityNotificationCommand request, CancellationToken cancellationToken)
        {
            return await _service.SendAvailabilityNotificationToAllUsers(request.HousingId, request.HousingTitle);
            
        }
    }
}
