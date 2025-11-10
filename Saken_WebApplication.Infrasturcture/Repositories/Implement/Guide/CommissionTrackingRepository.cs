using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.Models.Guid;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Guide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Infrasturcture.Repositories.Implement.Guide
{
    public class CommissionTrackingRepository : ICommissionTrackingRepository
    {
        private readonly ApplicationDBContext _context;

        public CommissionTrackingRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<CommissionTracking>> GetByAgentIdAsync(string agentId)
        {
            return await _context.commissionTrackings
                .Include(c => c.Housing)
                .Where(c => c.AgentId == agentId)
                .ToListAsync();
        }

        public async Task<CommissionTracking> GetByHousingIdAsync(int housingId)
        {
            return await _context.commissionTrackings
                .FirstOrDefaultAsync(c => c.HousingId == housingId);
        }

        public async Task<CommissionTracking> AddAsync(CommissionTracking commissionTracking)
        {
            _context.commissionTrackings.Add(commissionTracking);
            await _context.SaveChangesAsync();
            return commissionTracking;
        }

        public async Task<bool> UpdateStatusAsync(int commissionId, CommissionStatus newStatus)
        {
            var commission = await _context.commissionTrackings.FindAsync(commissionId);
            if (commission != null)
            {
                commission.CurrentStatus = newStatus;
                commission.LastUpdated = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
