using MediatR;
using Saken_WebApplication.Data.DTO.UserPreferences;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.AdditionalInformation.Query.Models
{
    public record GetPreferencesQuery(string UserId ):IRequest<BaseResponse<UserPreferencesDto>>;

    
}
