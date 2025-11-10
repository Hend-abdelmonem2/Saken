namespace Saken_WebApplication.Data.DTO
{
    public class UserRecommendationDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
        public int MatchScore { get; set; }
        public string UserType { get; set; }
        public bool IsFavorite { get; set; }
        public double? DistanceKm { get; set; }
    }
}
