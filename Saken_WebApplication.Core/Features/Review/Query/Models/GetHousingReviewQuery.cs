using MediatR;
using Saken_WebApplication.Data.DTO.Review;
using Saken_WebApplication.Data.Response;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Review.Query.Models
{
    public record GetHousingReviewsQuery(int HousingId) : IRequest<BaseResponse<ReviewsResultDto>>;
}
