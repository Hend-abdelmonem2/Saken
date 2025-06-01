using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces.Like;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saken_WebApplication.Data.Models;
using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.DTO.HousingDTO;

namespace Saken_WebApplication.Service.Services.Implement.Like
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likeRepository;

        private readonly List<string> _validEntityTypes = new() { "Housing", "User" };

        public LikeService(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }
        public async Task<string> ToggleLikeAsync(string userId, int entityId, string entityType)
        {
            if (!_validEntityTypes.Contains(entityType))
                return "Invalid entity type";

            var existingLike = await _likeRepository.GetLikeAsync(userId, entityId, entityType);

            if (existingLike != null)
            {
                await _likeRepository.RemoveLikeAsync(existingLike);
                await _likeRepository.SaveChangesAsync();
                return "Like removed";
            }
            else
            {
                var newLike = new Data.Models.Like
                {
                    UserId = userId,
                    EntityId = entityId,
                    EntityType = entityType
                };

                await _likeRepository.AddLikeAsync(newLike);
                await _likeRepository.SaveChangesAsync();
                return "Like added";
            }
        }
        public async Task<List<HousingLikeDto>> GetLikedHousesAsync(string userId)
        {
            return await _likeRepository.GetLikedHousesAsync(userId);
        }
    }
}
