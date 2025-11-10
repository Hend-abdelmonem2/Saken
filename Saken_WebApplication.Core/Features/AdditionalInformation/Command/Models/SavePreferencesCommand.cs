using MediatR;
using Saken_WebApplication.Data.DTO.UserPreferences;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.AdditionalInformation.Command.Models
{
    public record SavePreferencesCommand(UserPreferencesDto Dto):IRequest<BaseResponse<string>>;
    
}
