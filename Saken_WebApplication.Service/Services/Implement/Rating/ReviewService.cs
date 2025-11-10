using Microsoft.AspNetCore.Identity;
using Saken_WebApplication.Data.DTO.Review;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Review;
using Saken_WebApplication.Service.Services.Interfaces.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Service.Services.Implement.Rating
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHousingRepository _housingRepository;

        public ReviewService(IReviewRepository reviewRepository, UserManager<User> userManager,IHousingRepository housingRepository)
        {
            _reviewRepository = reviewRepository;
            _userManager = userManager;
            _housingRepository = housingRepository;
        }

        public async Task<BaseResponse<string>> AddReviewAsync(ReviewDto dto, string reviewerId)
        {
            var review = new Review
            {
                ReviewerId = reviewerId,
                ReviewedUserId = dto.ReviewedUserId,
                HousingId = dto.HousingId,
                ReviewType = dto.ReviewType,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepository.AddReviewAsync(review);
            await _reviewRepository.SaveChangesAsync();

           
            if (dto.ReviewType == ReviewType.User && dto.ReviewedUserId != null)
            {
                var reviews = await _reviewRepository.GetReviewsByUserIdAsync(dto.ReviewedUserId);
                var user = await _userManager.FindByIdAsync(dto.ReviewedUserId);

                user.UserRatingAverage = reviews.Average(r => r.Rating);
                user.UserRatingCount = reviews.Count;

                await _userManager.UpdateAsync(user);
            }
            else if (dto.ReviewType == ReviewType.Housing && dto.HousingId != null)
            {
                var reviews = await _reviewRepository.GetReviewsByHousingIdAsync(dto.HousingId.Value);
                var housing = await _housingRepository.GetByIdAsync(dto.HousingId.Value);

                if (housing != null)
                {
                    housing.HousingRatingAverage = reviews.Average(r => r.Rating);
                    housing.HousingRatingCount = reviews.Count;

                    await _housingRepository.UpdateAsync(housing);
                    await _housingRepository.SaveChangesAsync();
                }
            }

            return BaseResponse<string>.SuccessResponse(" تم إضافة التقييم وتحديث المتوسط بنجاح");
        }

        public async Task<BaseResponse<ReviewsResultDto>> GetUserReviewsWithAverageAsync(string userId)
        {
            var result = await _reviewRepository.GetUserReviewsWithAverageAsync(userId);

            if (result.Reviews == null || !result.Reviews.Any())
                return BaseResponse<ReviewsResultDto>.Failure("لا توجد تقييمات لهذا المستخدم");


            var dto = new ReviewsResultDto
            {
                Average = result.Average,
                Reviews = result.Reviews
            };

            return BaseResponse<ReviewsResultDto>.SuccessResponse(dto, "تم استرجاع تقييمات المستخدم بنجاح");
        }
        

        public async Task <BaseResponse<ReviewsResultDto>> GetHousingReviewsWithAverageAsync(int housingId)
        {
            var result = await _reviewRepository.GetHousingReviewsWithAverageAsync(housingId);

            if (result.Reviews == null || !result.Reviews.Any())
                return BaseResponse<ReviewsResultDto>.Failure("لا توجد تقييمات لهذا السكن");
            var dto = new ReviewsResultDto
            {
                Average = result.Average,
                Reviews = result.Reviews
            };

            return  BaseResponse<ReviewsResultDto>.SuccessResponse(dto, "تم استرجاع تقييمات السكن بنجاح");
        }
    }
}
