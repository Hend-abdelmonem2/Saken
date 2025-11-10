using MediatR;
using Saken_WebApplication.Core.Features.Profile.Command.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Profile.Command.Handlers
{
    public class UpdateProfileHandler :IRequestHandler<UpdateProfileCommand, BaseResponse<string>>
    {
        private readonly IprofileService _profileservice;


        public UpdateProfileHandler(IprofileService profileservice)
        {
            _profileservice = profileservice;   
        }

        public async Task <BaseResponse<string>> Handle (UpdateProfileCommand command,CancellationToken cancellationToken)
        {
            return await _profileservice.UpdateProfileAsync(command.UserId, command.Model);
        }
    }
}
