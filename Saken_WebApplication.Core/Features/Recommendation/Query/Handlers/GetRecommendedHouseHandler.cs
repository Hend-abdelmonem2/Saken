using MediatR;
using Saken_WebApplication.Core.Features.Recommendation.Query.Models;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.recommend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Recommendation.Query.Handlers
{
    public class GetRecommendedHousesHandler : IRequestHandler<GetRecommendedHousesQuery, BaseResponse<IEnumerable<HousingRecommendationDto>>>
    {
        private readonly IRecommendationService _recommendationService;

        public GetRecommendedHousesHandler(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        public async Task<BaseResponse<IEnumerable<HousingRecommendationDto>>> Handle(GetRecommendedHousesQuery request, CancellationToken cancellationToken)
        {
            return await _recommendationService.GetRecommendedHousesAsync();
        }
    }
}
