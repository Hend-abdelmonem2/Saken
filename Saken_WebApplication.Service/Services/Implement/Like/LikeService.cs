using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.DTO.Favorite;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces.Like;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<BaseResponse<string>> ToggleLikeAsync(string userId, string entityId, string entityType)
        {
            if (!_validEntityTypes.Contains(entityType))
                return BaseResponse<string>.Failure("Invalid entity type");

            var existingLike = await _likeRepository.GetLikeAsync(userId, entityId, entityType);

            if (existingLike != null)
            {
                await _likeRepository.RemoveLikeAsync(existingLike);
                await _likeRepository.SaveChangesAsync();
                return BaseResponse<string>.SuccessResponse("Like removed", " تمت إزالة الإعجاب");
            }

            var newLike = new Saken_WebApplication.Data.Models.Like
            {
                UserId = userId,
                EntityId = entityId,
                EntityType = entityType
            };

            await _likeRepository.AddLikeAsync(newLike);
            await _likeRepository.SaveChangesAsync();

            return BaseResponse<string>.SuccessResponse("Like added", " تم إضافة إعجاب");
        }

        public async Task<BaseResponse<List<HousingLikeDto>>> GetLikedHousesAsync(string userId)
        {
            var data = await _likeRepository.GetLikedHousesAsync(userId);
            return BaseResponse<List<HousingLikeDto>>.SuccessResponse(data, " المساكن التي تم الإعجاب بها");
        }

        public async Task<BaseResponse<List<UserLikeDto>>> GetLikedUsersAsync(string userId)
        {
            var data = await _likeRepository.GetLikedUsersAsync(userId);
            return BaseResponse<List<UserLikeDto>>.SuccessResponse(data, " المستخدمين الذين تم الإعجاب بهم");
        }
    }

}
