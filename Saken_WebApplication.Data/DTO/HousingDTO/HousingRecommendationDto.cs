namespace Saken_WebApplication.Data.DTO.HousingDTO
{
    public class HousingRecommendationDto
    {
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string Photo { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsSaved { get; set; }
        public int MatchScore { get; set; }
    }
}
