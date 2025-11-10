using Saken_WebApplication.Data.DTO.UserPreferences;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces.UserPreferences
{
    public interface IUserPreferencesService
    {
        Task <BaseResponse<string>>SavePreferencesAsync( UserPreferencesDto model);
        Task<BaseResponse<UserPreferencesDto>> GetPreferencesAsync(string userId);
    }
}
