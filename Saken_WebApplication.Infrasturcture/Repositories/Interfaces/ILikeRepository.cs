using Saken_WebApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Interfaces
{
    public  interface ILikeRepository
    {
        Task<Like> GetLikeAsync(string userId, int entityId, string entityType);
        Task AddLikeAsync(Like like);
        Task RemoveLikeAsync(Like like);
        Task SaveChangesAsync();
    }
}
