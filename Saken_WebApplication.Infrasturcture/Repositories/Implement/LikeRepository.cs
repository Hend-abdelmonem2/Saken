using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Implement
{
    public class LikeRepository: ILikeRepository
    {
         private readonly ApplicationDBContext _context;

        public LikeRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Like> GetLikeAsync(string userId, int entityId, string entityType)
        {
            return await _context.Likes.FirstOrDefaultAsync(l =>
                l.UserId == userId && l.EntityId == entityId && l.EntityType == entityType);
        }

        public async Task AddLikeAsync(Like like)
        {
            await _context.Likes.AddAsync(like);
        }

        public async Task RemoveLikeAsync(Like like)
        {
            _context.Likes.Remove(like);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
