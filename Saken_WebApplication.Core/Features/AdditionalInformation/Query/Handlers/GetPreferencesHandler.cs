using MediatR;
using Saken_WebApplication.Core.Features.AdditionalInformation.Query.Models;
using Saken_WebApplication.Data.DTO.UserPreferences;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.UserPreferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.AdditionalInformation.Query.Handlers
{
   public class GetPreferencesHandler:IRequestHandler<GetPreferencesQuery, BaseResponse<UserPreferencesDto>>
    {
        private readonly IUserPreferencesService _userPreferencesService;

        public GetPreferencesHandler(IUserPreferencesService userPreferencesService)
        { 
            _userPreferencesService = userPreferencesService;
            
        }
        public async Task<BaseResponse<UserPreferencesDto>> Handle(GetPreferencesQuery request, CancellationToken cancellationToken)
        {
            return   await _userPreferencesService.GetPreferencesAsync(request.UserId);

           
        }
    }
}
