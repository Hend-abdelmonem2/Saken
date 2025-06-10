using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.DTO.Favorite;
using Saken_WebApplication.Data.DTO.HousingDTO;
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
        public async Task<Like> GetLikeAsync(string userId, string entityId, string entityType)
        {
            return await _context.Likes.FirstOrDefaultAsync(l =>
                l.UserId == userId && l.EntityId == entityId && l.EntityType == entityType);
        }
        public async Task<List<HousingLikeDto>> GetLikedHousesAsync(string userId)
        {
            var likedHouseIds = await _context.Likes
                .Where(l => l.UserId == userId && l.EntityType == "Housing")
                .Select(l => l.EntityId)
                .ToListAsync();
            var likedHouseIdsInt = likedHouseIds
            .Where(id => int.TryParse(id, out _)) 
             .Select(id => int.Parse(id))
             .ToList();

            var houses = await _context.houses
                .Where(h => likedHouseIdsInt.Contains(h.h_Id))
                .Select(h => new HousingLikeDto
                {
                    Id = h.h_Id,
                    Address = h.address,
                    Price = h.price,
                    PhotoUrl = h.Photo
                }).ToListAsync();

            return houses;
        }
        public async Task<List<UserLikeDto>> GetLikedUsersAsync(string userId)
        {
            var likedUserIds = await _context.Likes
                .Where(l => l.UserId == userId && l.EntityType == "User")
                .Select(l => l.EntityId)
                .ToListAsync();

            var users = await _context.Users
                .Where(u => likedUserIds.Contains(u.Id))
                .Select(u => new UserLikeDto
                {
                    Id = u.Id,
                    Name = u.FullName,
                    Email = u.Email,
                    ProfilePicture = u.profilePicture,
                    UserType=u.Role
                }).ToListAsync();

            return users;
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
