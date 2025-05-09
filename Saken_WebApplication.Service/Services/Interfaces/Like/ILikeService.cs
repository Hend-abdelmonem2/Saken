using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces.Like
{
    public  interface ILikeService
    {
        Task<string> ToggleLikeAsync(string userId, int entityId, string entityType);
    }
}
