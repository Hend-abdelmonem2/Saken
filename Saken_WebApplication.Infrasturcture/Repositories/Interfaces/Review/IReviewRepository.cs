using Saken_WebApplication.Data.DTO.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Review
{
    public interface IReviewRepository
    {
        Task AddReviewAsync(Saken_WebApplication.Data.Models.Review review);
        Task<List<Saken_WebApplication.Data.Models.Review>> GetReviewsByUserIdAsync(string userId);
        Task<List<Saken_WebApplication.Data.Models.Review>> GetReviewsByHousingIdAsync(int housingId);
        Task<(double Average, List<ReviewDisplayDTO> Reviews)> GetUserReviewsWithAverageAsync(string userId);
        Task<(double Average, List<ReviewDisplayDTO> Reviews)> GetHousingReviewsWithAverageAsync(int housingId);


        Task SaveChangesAsync();
    }
}
