using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Core.Features.Review.Command.Models;
using Saken_WebApplication.Core.Features.Review.Query.Models;
using Saken_WebApplication.Data.DTO.Review;

namespace Saken_WebApplication.Controllers.Review
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview([FromBody] ReviewDto dto)
        {
            var reviewerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var command = new AddReviewCommand(dto, reviewerId!);

            var result = await _mediator.Send(command);
            if(!result.Success)
                return NotFound(result);
            return Ok(result);
        }


        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserReviews(string userId)
        {
            var result = await _mediator.Send(new GetUserReviewsQuery(userId));
            if(!result.Success)
                return NotFound(result);
            return Ok(result);
        }

     
        [HttpGet("housing/{housingId:int}")]
        [Authorize]
        public async Task<IActionResult> GetHousingReviews(int housingId)
        {
            var result = await _mediator.Send(new GetHousingReviewsQuery(housingId));
            if(!result.Success)
                 return BadRequest(result);
            return Ok(result);
        }
    }

}
