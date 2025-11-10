using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Core.Features.Recommendation.Query.Models;
using Saken_WebApplication.Service.Services.Implement.housing;
using Saken_WebApplication.Service.Services.Implement.Recommand;
using Saken_WebApplication.Service.Services.Interfaces.recommend;

namespace Saken_WebApplication.Controllers.Recommend
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IRecommendationService _recommendationService;

        public RecommendationController(IMediator mediator, IRecommendationService recommendationService)
        {
            _mediator = mediator;
            _recommendationService = recommendationService;
        }

        [HttpGet("RecommendedHouse")]
        [Authorize]
        public async Task<IActionResult> GetRecommendedHouses()
        {
            var result = await _mediator.Send(new GetRecommendedHousesQuery());
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }



       
        [HttpGet("RecommendedUser")]
        [Authorize]
        public async Task<IActionResult> GetRecommendations()
        {
            var result=await _mediator.Send(new GetRecommendedUserQuery());
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

    }
}
