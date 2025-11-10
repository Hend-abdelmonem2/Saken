using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.DTO.Review;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Infrasturcture.Repositories.Implement.Review
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDBContext _context;

        public ReviewRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task AddReviewAsync(Saken_WebApplication.Data.Models.Review review)
        {
            await _context.reviews.AddAsync(review);
        }

        public async Task<List<Saken_WebApplication.Data.Models.Review>> GetReviewsByUserIdAsync(string userId)

        {
            return await _context.reviews
                .Where(r => r.ReviewType == ReviewType.User && r.ReviewedUserId == userId)
                .ToListAsync();
        }

        public async Task<List<Saken_WebApplication.Data.Models.Review>> GetReviewsByHousingIdAsync(int housingId)
        {
            return await _context.reviews
                .Where(r => r.ReviewType == ReviewType.Housing && r.HousingId == housingId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<(double Average, List<ReviewDisplayDTO> Reviews)> GetUserReviewsWithAverageAsync(string userId)
        {
            var query = _context.reviews
                .Where(r => r.ReviewType == ReviewType.User && r.ReviewedUserId == userId)
                .Include(r => r.Reviewer);

            var average = await query.AverageAsync(r => (double?)r.Rating) ?? 0;

            var reviews = await query
                .Select(r => new ReviewDisplayDTO
                {
                    ReviewerName = r.Reviewer.UserName,
                    ReviewerPhotoUrl = r.Reviewer.profilePicture,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            return (average, reviews);
        }
        public async Task<(double Average, List<ReviewDisplayDTO> Reviews)> GetHousingReviewsWithAverageAsync(int housingId)
        {
            var query = _context.reviews
                .Where(r => r.ReviewType == ReviewType.Housing && r.HousingId == housingId)
                .Include(r => r.Reviewer); 
            var average = await query.AverageAsync(r => (double?)r.Rating) ?? 0;

            var reviews = await query
                .Select(r => new ReviewDisplayDTO
                {
                    ReviewerName = r.Reviewer.UserName,
                    ReviewerPhotoUrl = r.Reviewer.profilePicture,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            return (average, reviews);
        }


    }
}
