using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.Models.Guid;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Guide;
using Saken_WebApplication.Service.Services.Interfaces.Guide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Service.Services.Implement.Guide
{
    public class AgentCommissionService : IAgentCommissionService
    {
        private readonly ICommissionTrackingRepository _repository;
        private readonly ApplicationDBContext _context;
        public AgentCommissionService(ICommissionTrackingRepository repository, ApplicationDBContext context)
        {
            _repository = repository;
            _context = context;

        }

        public async Task<BaseResponse<List<CommissionTracking>>> GetCommissionStepsAsync(string agentId)
        {
            var agent= await _repository.GetByAgentIdAsync(agentId);
            if (agent == null)
                return BaseResponse<List<CommissionTracking>>.Failure("Agent Not Found");
            return BaseResponse<List<CommissionTracking>>.SuccessResponse(agent, "agent return success");

        }
        public async Task<BaseResponse<CommissionTracking>> GetByHousingIdAsync(int housingId)
        {
            var tracking = await _context.commissionTrackings
                .FirstOrDefaultAsync(x => x.HousingId == housingId);

            if (tracking == null)
                return BaseResponse<CommissionTracking>.Failure("Tracking not found");

            return BaseResponse<CommissionTracking>.SuccessResponse(tracking,"Tracking retrive successfully");
        }

        public async Task<BaseResponse<bool>> UpdateCommissionStatusAsync(int commissionId, CommissionStatus newStatus)
        {
            var tracking = await _repository.UpdateStatusAsync(commissionId, newStatus);

            if (!tracking) 
                return BaseResponse<bool>.Failure("فشل في تحديث الحالة");

            return BaseResponse<bool>.SuccessResponse(true, "تم تحديث الحالة بنجاح");
        }
    }
}
