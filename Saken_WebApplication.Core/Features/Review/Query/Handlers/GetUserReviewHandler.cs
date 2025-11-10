using MediatR;
using Saken_WebApplication.Core.Features.Review.Query.Models;
using Saken_WebApplication.Data.DTO.Review;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Review.Query.Handlers
{
    public class GetUserReviewsHandler : IRequestHandler<GetUserReviewsQuery, BaseResponse<ReviewsResultDto>>
    {
        private readonly IReviewService _service;

        public GetUserReviewsHandler(IReviewService service)
        {
            _service = service;
        }

        public async Task<BaseResponse<ReviewsResultDto>> Handle(GetUserReviewsQuery request, CancellationToken cancellationToken)
            => await _service.GetUserReviewsWithAverageAsync(request.UserId);
    }
}
