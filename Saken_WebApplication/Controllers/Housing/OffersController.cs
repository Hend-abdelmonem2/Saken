using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Core.Features.Houses.Command.Models;
using Saken_WebApplication.Core.Features.Houses.Query.Models;
using Saken_WebApplication.Data.DTO.HousingDTO;

namespace Saken_WebApplication.Controllers.Housing
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OffersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("ActiveOffer")]
        
        public async Task<IActionResult> GetActiveOffers()
        {
            var result = await _mediator.Send(new GetActiveOffersQuery());
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }


        [HttpGet("GetOfferForHouse/{housingId}")]
        [Authorize]
        public async Task<IActionResult> GetOffersByHousingId(int housingId)
        {
            var result = await _mediator.Send(new GetOffersByHousingIdQuery(housingId));
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPost("CreateOffer")]
        [Authorize]
        public async Task<IActionResult> CreateOffer([FromBody] HousingOfferDto dto)

        { 

            var result = await _mediator.Send(new CreateOfferCommand(dto));
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        } 
    }
}

