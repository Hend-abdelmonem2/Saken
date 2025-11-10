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
    public class LikeRepository : ILikeRepository
        {
            private readonly ApplicationDBContext _context;

            public LikeRepository(ApplicationDBContext context)
            {
                _context = context;
            }

            public async Task<Like?> GetLikeAsync(string userId, string entityId, string entityType)
            {
                return await _context.Likes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(l =>
                        l.UserId == userId && l.EntityId == entityId && l.EntityType == entityType);
            }

            public async Task<List<HousingLikeDto>> GetLikedHousesAsync(string userId)
            {
                return await (from like in _context.Likes.AsNoTracking()
                              join house in _context.houses.AsNoTracking()
                                  on like.EntityId equals house.Id.ToString()
                              where like.UserId == userId && like.EntityType == "Housing"
                              select new HousingLikeDto
                              {
                                  Id = house.Id,
                                  Address = house.Address,
                                  Price = house.PricePerMeter,
                                  PhotoUrl = house.PhotoUrl
                              }).ToListAsync();
            }

            public async Task<List<UserLikeDto>> GetLikedUsersAsync(string userId)
            {
                return await (from like in _context.Likes.AsNoTracking()
                              join user in _context.Users.AsNoTracking()
                                  on like.EntityId equals user.Id
                              where like.UserId == userId && like.EntityType == "User"
                              select new UserLikeDto
                              {
                                  Id = user.Id,
                                  Name = user.FullName,
                                  Email = user.Email,
                                  ProfilePicture = user.profilePicture,
                                  UserType = user.Role
                              }).ToListAsync();
            }

            public async Task AddLikeAsync(Like like)
            {
                await _context.Likes.AddAsync(like);
            }

            public Task RemoveLikeAsync(Like like)
            {
                _context.Likes.Remove(like);
                return Task.CompletedTask;
            }

            public Task SaveChangesAsync()
            {
                return _context.SaveChangesAsync();
            }
        }

    }
