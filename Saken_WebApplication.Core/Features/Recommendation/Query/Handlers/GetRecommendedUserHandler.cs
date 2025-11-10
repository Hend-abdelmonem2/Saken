using MediatR;
using Saken_WebApplication.Core.Features.Recommendation.Query.Models;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.recommend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Recommendation.Query.Handlers
{
    public class GetRecommendedUserHandler:IRequestHandler<GetRecommendedUserQuery, BaseResponse<IEnumerable<UserRecommendationDto>>>
    {
        private readonly IRecommendationService _recommendationService;

        public GetRecommendedUserHandler(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        public async Task <BaseResponse<IEnumerable<UserRecommendationDto>>> Handle (GetRecommendedUserQuery query,CancellationToken cancellationToken)
        {
            return await _recommendationService.GetRecommendedUsersAsync();
        }
    }
}
