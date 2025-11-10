using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.DTO.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Data.DTO.profile
{
    public class TenantProfileDto
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


        public List<TenantReservationDto> RentedHousings { get; set; } = new();
    }
}
