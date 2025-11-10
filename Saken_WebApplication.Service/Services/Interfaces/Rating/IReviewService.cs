using Saken_WebApplication.Data.DTO.Review;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces.Rating
{
    public interface IReviewService
    {
        Task<BaseResponse<string>> AddReviewAsync(ReviewDto dto, string reviewerId);

        Task<BaseResponse<ReviewsResultDto>> GetUserReviewsWithAverageAsync(string userId);
        Task<BaseResponse<ReviewsResultDto>> GetHousingReviewsWithAverageAsync(int housingId);

    }
}
