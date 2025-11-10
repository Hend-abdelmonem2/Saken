using MediatR;
using Saken_WebApplication.Data.DTO.Review;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Review.Command.Models
{
    public record AddReviewCommand(ReviewDto Dto, string ReviewerId) : IRequest<BaseResponse<string>>;
}
