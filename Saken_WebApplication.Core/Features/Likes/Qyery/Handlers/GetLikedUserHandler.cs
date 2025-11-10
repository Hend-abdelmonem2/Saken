using MediatR;
using Microsoft.AspNetCore.Http;
using Saken_WebApplication.Core.Features.Likes.Qyery.Models;
using Saken_WebApplication.Data.DTO.Favorite;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Like;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Likes.Qyery.Handlers
{
    public class GetLikedUsersHandler : IRequestHandler<GetLikedUsersQuery, BaseResponse<List<UserLikeDto>>>
    {
        private readonly ILikeService _likeService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetLikedUsersHandler(ILikeService likeService, IHttpContextAccessor httpContextAccessor)
        {
            _likeService = likeService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<List<UserLikeDto>>> Handle(GetLikedUsersQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?
                .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return BaseResponse<List<UserLikeDto>>.Failure("المستخدم غير مسجل الدخول");

            return await _likeService.GetLikedUsersAsync(userId);
        }
    }
    }
