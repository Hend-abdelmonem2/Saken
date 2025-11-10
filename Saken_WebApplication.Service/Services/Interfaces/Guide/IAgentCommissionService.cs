using Saken_WebApplication.Data.Models.Guid;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Service.Services.Interfaces.Guide
{
    public interface IAgentCommissionService
    {
        Task<BaseResponse<List<CommissionTracking>>> GetCommissionStepsAsync(string agentId);
        Task<BaseResponse<bool>> UpdateCommissionStatusAsync(int commissionId, CommissionStatus newStatus);
        Task<BaseResponse<CommissionTracking>> GetByHousingIdAsync(int housingId);
    }
}
