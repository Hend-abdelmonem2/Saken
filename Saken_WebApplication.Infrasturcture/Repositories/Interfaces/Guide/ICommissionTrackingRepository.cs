using Saken_WebApplication.Data.Models.Guid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Guide
{
    public interface ICommissionTrackingRepository
    {
        Task<List<CommissionTracking>> GetByAgentIdAsync(string agentId);
        Task<CommissionTracking> GetByHousingIdAsync(int housingId);
        Task<CommissionTracking> AddAsync(CommissionTracking commissionTracking);
        Task<bool> UpdateStatusAsync(int commissionId, CommissionStatus newStatus);
    }
}
