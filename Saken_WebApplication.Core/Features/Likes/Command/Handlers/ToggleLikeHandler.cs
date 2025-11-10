using MediatR;
using Microsoft.AspNetCore.Http;
using Saken_WebApplication.Core.Features.Likes.Command.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Like;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Likes.Command.Handlers
{
    public class ToggleLikeHandler : IRequestHandler<ToggleLikeCommand, BaseResponse<string>>
    {
        private readonly ILikeService _likeService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ToggleLikeHandler(ILikeService likeService, IHttpContextAccessor httpContextAccessor)
        {
            _likeService = likeService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> Handle(ToggleLikeCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?
                .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return BaseResponse<string>.Failure("المستخدم غير مسجل الدخول");

            return await _likeService.ToggleLikeAsync(userId, request.EntityId, request.EntityType);
        }
    }
    }
