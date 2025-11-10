using MediatR;
using Microsoft.AspNetCore.Http;
using Saken_WebApplication.Core.Features.Likes.Qyery.Models;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Like;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Likes.Qyery.Handlers
{
    public class GetLikedHousesHandler : IRequestHandler<GetLikedHousesQuery, BaseResponse<List<HousingLikeDto>>>
    {
        private readonly ILikeService _likeService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetLikedHousesHandler(ILikeService likeService, IHttpContextAccessor httpContextAccessor)
        {
            _likeService = likeService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<List<HousingLikeDto>>> Handle(GetLikedHousesQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?
                .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return BaseResponse<List<HousingLikeDto>>.Failure("المستخدم غير مسجل الدخول");

            return await _likeService.GetLikedHousesAsync(userId);
        }
    }
}
