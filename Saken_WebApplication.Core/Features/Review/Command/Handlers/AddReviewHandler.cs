using MediatR;
using Saken_WebApplication.Core.Features.Review.Command.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Review.Command.Handlers
{
    public class AddReviewHandler : IRequestHandler<AddReviewCommand, BaseResponse<string>>
    {
        private readonly IReviewService _service;

        public AddReviewHandler(IReviewService service)
        {
            _service = service;
        }

        public async Task<BaseResponse<string>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
            => await _service.AddReviewAsync(request.Dto, request.ReviewerId);
    }
}
