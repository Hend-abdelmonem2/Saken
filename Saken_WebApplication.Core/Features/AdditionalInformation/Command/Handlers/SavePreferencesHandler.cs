using MediatR;
using NuGet.Protocol.Plugins;
using Saken_WebApplication.Core.Features.AdditionalInformation.Command.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.UserPreferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.AdditionalInformation.Command.Handlers
{
    public class SavePreferencesHandler:IRequestHandler<SavePreferencesCommand,BaseResponse<string>>
    {

        private readonly IUserPreferencesService _userPreferencesService;

        public SavePreferencesHandler(IUserPreferencesService userPreferencesService)
        {
             _userPreferencesService = userPreferencesService;
        }
        public async Task<BaseResponse<string>> Handle (SavePreferencesCommand command, CancellationToken cancellationToken)
        {
            return  await _userPreferencesService.SavePreferencesAsync(command.Dto);


        }

       
    }
}
