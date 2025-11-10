using Microsoft.AspNetCore.Identity;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.DTO.profile;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Infrasturcture.Repositories;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Reservation;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Review;
using Saken_WebApplication.Service.Services.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using Saken_WebApplication.Service.Services.Interfaces.Profile;

namespace Saken_WebApplication.Service.Services.Implement.Profile
{
    public class ProfileService : IprofileService
    {
        private readonly IGenericRepository<User> _genericRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IHousingService _housingService;
        private readonly IReservationRepository _reservationRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;

        public ProfileService(IGenericRepository<User> genericRepository, IReviewRepository reviewRepository, IHousingService housingService, IReservationRepository reservationRepository, ICloudinaryService cloudinaryService, UserManager<User> userManager)
        {
            _genericRepository = genericRepository;
            _reviewRepository = reviewRepository;
            _housingService = housingService;
            _reservationRepository = reservationRepository;
            _cloudinaryService = cloudinaryService;
            _userManager = userManager;

        }
        /* public async Task<BaseResponse<LandlordProfileDto>> GetLandlordProfileAsync(string landlordId)
         {
             var user = await _genericRepository.GetByIdAsync(landlordId);
             if (user == null)
                 return BaseResponse<LandlordProfileDto>.Failure("User not found");


             var (average, reviews) = await _reviewRepository.GetUserReviewsWithAverageAsync(landlordId);
             var groupedHousings = await _housingService.GetGroupedHousingsForLandlordAsync(landlordId);


             var profile = new LandlordProfileDto
             {
                 UserId = user.Id,
                 FullName = user.FullName,
                 Email = user.Email,
                 PhoneNumber = user.PhoneNumber,
                 Role = user.Role,
                 ProfilePicture = user.profilePicture,
                 address = user.address,
                 AverageRating = Math.Round(average, 2),
                 TotalReviews = reviews.Count,
                 Reviews = reviews,
                 AvailableHousings = groupedHousings.Data.availableHousings,
                 RentedHousings = groupedHousings.Data.RentedHousings
             };

             return BaseResponse<LandlordProfileDto>.SuccessResponse(profile);
         }*/

        public async Task<BaseResponse<TenantProfileDto>> GetTenantProfileAsync(string tenantId)
        {
            var user = await _genericRepository.GetByIdAsync(tenantId);
            if (user == null)
                return BaseResponse<TenantProfileDto>.Failure("Tenant not found");

            var (average, reviews) = await _reviewRepository.GetUserReviewsWithAverageAsync(tenantId);
            var reservations = await _reservationRepository.GetConfirmedReservationsForTenantAsync(tenantId);

            var rentedHousings = reservations.Select(r => new TenantReservationDto
            {
                HousingTitle = r.Housing.Title,
                Address = r.Housing.Address,
                ReservationDate = r.ReservationDate,
                OwnerName = r.Housing.Owner.FullName,
                PricePerMonth = r.Housing.PricePerMeter,
                ImageUrl = r.Housing.PhotoUrl
            }).ToList();

            var profile = new TenantProfileDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                ProfilePicture = user.profilePicture,
                address = user.address,
                AverageRating = Math.Round(average, 2),
                TotalReviews = reviews.Count,
                Reviews = reviews,
                RentedHousings = rentedHousings
            };

            return BaseResponse<TenantProfileDto>.SuccessResponse(profile);
        }


        public async Task<BaseResponse<UserProfileDto>> GetUserProfileAsync(string userId)
        {
            var user = await _genericRepository.GetByIdAsync(userId);
            if (user == null)
                return BaseResponse<UserProfileDto>.Failure("User not found");


            var (average, reviews) = await _reviewRepository.GetUserReviewsWithAverageAsync(userId);


            var profile = new UserProfileDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                ProfilePicture = user.profilePicture,
                address = user.address,
                AverageRating = Math.Round(average, 2),
                TotalReviews = reviews.Count,
                Reviews = reviews
            };


            if (user.Role == "Owner")
            {
                var groupedHousings = await _housingService.GetGroupedHousingsForLandlordAsync(userId);
                profile.AvailableHousings = groupedHousings.Data.availableHousings ?? new List<HouseDTO>();
                profile.RentedHousings = groupedHousings.Data.RentedHousings ?? new List<HouseDTO>();
            }


            else if (user.Role == "Tenant")
            {
                var reservations = await _reservationRepository.GetConfirmedReservationsForTenantAsync(userId);
                profile.TenantReservations = reservations.Select(r => new TenantReservationDto
                {
                    HousingTitle = r.Housing.Title,
                    Address = r.Housing.Address,
                    ReservationDate = r.ReservationDate,
                    OwnerName = r.Housing.Owner.FullName,
                    PricePerMonth = r.Housing.PricePerMeter,
                    ImageUrl = r.Housing.PhotoUrl
                }).ToList();

            }


            return BaseResponse<UserProfileDto>.SuccessResponse(profile);
        }
        public async Task<BaseResponse<string>> UpdateProfileAsync(string userId, UpdateUserDto model)
        {
            var user = await _genericRepository.GetByIdAsync(userId);
            if (user == null)
                return BaseResponse<string>.Failure("المستخدم غير موجود");

            if (!string.IsNullOrWhiteSpace(model.FullName))
                user.FullName = model.FullName;


            if (!string.IsNullOrWhiteSpace(model.Email) && model.Email != user.Email)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                    return BaseResponse<string>.Failure("Email is already in use by another user.");

                user.Email = model.Email;

            }


            if (!string.IsNullOrWhiteSpace(model.PhoneNumber) && model.PhoneNumber != user.PhoneNumber)
            {
                var existingPhoneUser = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == model.PhoneNumber);
                if (existingPhoneUser != null)
                    return BaseResponse<string>.Failure("Phone number is already in use by another user.");

                user.PhoneNumber = model.PhoneNumber;
            }

            if (model.photo != null)
            {
                var uploadResult = await _cloudinaryService.UploadImageAsync(model.photo);
                if (uploadResult.Error != null)
                    return BaseResponse<string>.Failure(uploadResult.Error.Message);

                user.profilePicture = uploadResult.SecureUrl.ToString();
            }


            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                var removeResult = await _userManager.RemovePasswordAsync(user);
                if (!removeResult.Succeeded)
                    return BaseResponse<string>.Failure("فشل في إزالة كلمة المرور القديمة");

                var addResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (!addResult.Succeeded)
                    return BaseResponse<string>.Failure("كلمة المرور الجديدة غير صالحة");
            }


            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return BaseResponse<string>.Failure("فشل في تحديث البيانات");

            return BaseResponse<string>.SuccessResponse("تم تحديث الملف الشخصي بنجاح");
        }

    }
}

