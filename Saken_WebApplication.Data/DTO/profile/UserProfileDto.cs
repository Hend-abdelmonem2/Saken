using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.DTO.Review;
using System.Text.Json.Serialization;

namespace Saken_WebApplication.Data.DTO.profile
{
    public class UserProfileDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string address { get; set; }
        public string Role { get; set; }
        public string ProfilePicture { get; set; }

        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public List<ReviewDisplayDTO> Reviews { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<HouseDTO> AvailableHousings { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<HouseDTO> RentedHousings { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<TenantReservationDto> TenantReservations { get; set; }
    }
}
