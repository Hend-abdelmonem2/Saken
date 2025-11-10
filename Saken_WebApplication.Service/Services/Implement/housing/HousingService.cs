using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using Saken_WebApplication.Service.Services.Interfaces.Notifications;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Service.Services.Implement.housing
{
    public class HousingService : IHousingService
    {
        private readonly IHousingRepository _housingRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly INotificationService _notificationService;
        private readonly UserManager<User> _userManager;
        private readonly IAdminRepository _adminRepository;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDBContext _context;


        public HousingService(IHousingRepository housingRepository, ICloudinaryService cloudinaryService, INotificationService notificationService, UserManager<User> userManager,
            IAdminRepository adminRepository,
            IHttpContextAccessor httpContextAccessor, HttpClient httpClient,
             ApplicationDBContext context)
        {
            _housingRepository = housingRepository;
            _cloudinaryService = cloudinaryService;
            _notificationService = notificationService;
            _userManager = userManager;
            _adminRepository = adminRepository;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _context = context;


        }

        public async Task<BaseResponse<string>> AddHousingAsync(HousingDto dto, string landlordId)
        {
            var housing = new Saken_WebApplication.Data.Models.Housing
            {
                Title = dto.Title,
                Address = dto.Address,
                PricePerMeter = dto.PricePerMeter,
                AreaInMeters = dto.AreaInMeters,
                Floor = dto.Floor,
                HousingType = Enum.Parse<PropertyType>(dto.HousingType, true),
                FurnishingStatus = Enum.Parse<FurnishingStatus>(dto.FurnishingStatus, true),
                RentalType = Enum.Parse<RentalType>(dto.RentalType, true),
                RentDurationValue = dto.RentDurationValue,
                RentdurationUnit = Enum.Parse<RentDurationUnit>(dto.RentDurationUnit, true),

                NumberOfRooms = dto.NumberOfRooms,
                HasKitchen = dto.HasKitchen,
                HasBathroom = dto.HasBathroom,
                HasLivingRoom = dto.HasLivingRoom,

                HasBed = dto.HasBed,
                HasWardrobe = dto.HasWardrobe,
                HasChair = dto.HasChair,

                HasFridge = dto.HasFridge,
                HasStove = dto.HasStove,
                HasWasher = dto.HasWasher,
                HasFan = dto.HasFan,
                HasTV = dto.HasTV,
                HasInternet = dto.HasInternet,

                HasGas = dto.HasGas,
                HasElectricity = dto.HasElectricity,
                HasWater = dto.HasWater,

                TargetTenantType = Enum.Parse<TargetCustomerType>(dto.TargetTenantType, true),
                TargetTenantDescription = dto.TargetTenantDescription,

                DepositAmount = dto.DepositAmount,
                InsuranceAmount = dto.InsuranceAmount,
                CommissionAmount = dto.CommissionAmount,

                HousingUrl = dto.HousingUrl,
                OwnerId = landlordId
            };

            // الصورة الأساسية
            if (dto.Photo != null)
            {
                var uploadResult = await _cloudinaryService.UploadImageAsync(dto.Photo);
                if (uploadResult.Error != null)
                    throw new ApplicationException($"Image upload failed: {uploadResult.Error.Message}");


                housing.PhotoUrl = uploadResult.SecureUrl.ToString();

                housing.Photos.Add(new HousingPhoto
                {
                    Url = uploadResult.SecureUrl.ToString(),
                    PublicId = uploadResult.PublicId,
                    UploadedAt = DateTime.UtcNow
                });
            }

            if (dto.ExtraPhotos != null && dto.ExtraPhotos.Any())
            {
                foreach (var photo in dto.ExtraPhotos)
                {
                    var upload = await _cloudinaryService.UploadImageAsync(photo);
                    if (upload.Error == null)
                    {
                        housing.Photos.Add(new HousingPhoto
                        {
                            Url = upload.SecureUrl.ToString(),
                            PublicId = upload.PublicId,
                            UploadedAt = DateTime.UtcNow
                        });
                    }
                }
            }
            if (dto.InspectionDates != null && dto.InspectionDates.Any())
            {
                housing.InspectionSlots.AddRange(dto.InspectionDates.Select(date => new InspectionSlot
                {
                    StartDateTime = date

                }));
            }
            var result = await _housingRepository.AddHousingAsync(housing);
            return result;
        }
        public async Task<BaseResponse> UpdateHousingAsync(int id, UpdateHousingDto dto, string userId)
        {
            var housing = await _housingRepository.GetByIdWithPhotosAsync(id);
            if (housing == null)
                return BaseResponse.Failure(" Housing not found");

            if (housing.OwnerId != userId)
                return BaseResponse.Failure(" أنت غير مصرح لك بتعديل هذا السكن.");

            //  رفع صورة رئيسية
            if (dto.Photo != null && dto.Photo.Length > 0)
            {
                var uploadResult = await _cloudinaryService.UploadImageAsync(dto.Photo);
                if (uploadResult.Error != null)
                    throw new ApplicationException("Image upload failed: " + uploadResult.Error.Message);

                housing.PhotoUrl = uploadResult.SecureUrl.ToString();
                housing.Photos.Add(new HousingPhoto
                {
                    Url = uploadResult.SecureUrl.ToString(),
                    PublicId = uploadResult.PublicId,
                    UploadedAt = DateTime.UtcNow
                });
            }

            // ✅ تحديث صور إضافية (اختياري)
            if (dto.ExtraPhotos != null && dto.ExtraPhotos.Any())
            {
                foreach (var photo in dto.ExtraPhotos)
                {
                    var upload = await _cloudinaryService.UploadImageAsync(photo);
                    if (upload.Error == null)
                    {
                        housing.Photos.Add(new HousingPhoto
                        {
                            Url = upload.SecureUrl.ToString(),
                            PublicId = upload.PublicId,
                            UploadedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            // ✅ تحديث الحقول اللي اتبعت بس
            if (!string.IsNullOrEmpty(dto.Title)) housing.Title = dto.Title;
            if (!string.IsNullOrEmpty(dto.Address)) housing.Address = dto.Address;
            if (dto.PricePerMeter.HasValue) housing.PricePerMeter = dto.PricePerMeter.Value;
            if (dto.AreaInMeters.HasValue) housing.AreaInMeters = dto.AreaInMeters.Value;
            if (dto.Floor.HasValue) housing.Floor = dto.Floor.Value;
            if (!string.IsNullOrEmpty(dto.HousingType)) housing.HousingType = Enum.Parse<PropertyType>(dto.HousingType, true);
            if (!string.IsNullOrEmpty(dto.FurnishingStatus)) housing.FurnishingStatus = Enum.Parse<FurnishingStatus>(dto.FurnishingStatus, true);
            if (!string.IsNullOrEmpty(dto.RentalType)) housing.RentalType = Enum.Parse<RentalType>(dto.RentalType, true);

            if (dto.RentDurationValue.HasValue) housing.RentDurationValue = dto.RentDurationValue.Value;
            if (!string.IsNullOrEmpty(dto.RentDurationUnit)) housing.RentdurationUnit = Enum.Parse<RentDurationUnit>(dto.RentDurationUnit, true);

            // ✅ بقية البوليان بنفس الفكرة
            if (dto.HasKitchen.HasValue) housing.HasKitchen = dto.HasKitchen.Value;
            if (dto.HasBathroom.HasValue) housing.HasBathroom = dto.HasBathroom.Value;
            if (dto.HasLivingRoom.HasValue) housing.HasLivingRoom = dto.HasLivingRoom.Value;

            if (dto.HasBed.HasValue) housing.HasBed = dto.HasBed.Value;
            if (dto.HasWardrobe.HasValue) housing.HasWardrobe = dto.HasWardrobe.Value;
            if (dto.HasChair.HasValue) housing.HasChair = dto.HasChair.Value;
            if (dto.HasFridge.HasValue) housing.HasFridge = dto.HasFridge.Value;
            if (dto.HasStove.HasValue) housing.HasStove = dto.HasStove.Value;
            if (dto.HasWasher.HasValue) housing.HasWasher = dto.HasWasher.Value;
            if (dto.HasFan.HasValue) housing.HasFan = dto.HasFan.Value;
            if (dto.HasTV.HasValue) housing.HasTV = dto.HasTV.Value;
            if (dto.HasInternet.HasValue) housing.HasInternet = dto.HasInternet.Value;

            if (dto.HasGas.HasValue) housing.HasGas = dto.HasGas.Value;
            if (dto.HasElectricity.HasValue) housing.HasElectricity = dto.HasElectricity.Value;
            if (dto.HasWater.HasValue) housing.HasWater = dto.HasWater.Value;

            if (!string.IsNullOrEmpty(dto.TargetTenantType)) housing.TargetTenantType = Enum.Parse<TargetCustomerType>(dto.TargetTenantType, true);
            if (!string.IsNullOrEmpty(dto.TargetTenantDescription)) housing.TargetTenantDescription = dto.TargetTenantDescription;

            if (dto.DepositAmount.HasValue) housing.DepositAmount = dto.DepositAmount.Value;
            if (dto.InsuranceAmount.HasValue) housing.InsuranceAmount = dto.InsuranceAmount.Value;
            if (dto.CommissionAmount.HasValue) housing.CommissionAmount = dto.CommissionAmount.Value;

            if (!string.IsNullOrEmpty(dto.HousingUrl)) housing.HousingUrl = dto.HousingUrl;

            // ✅ تحديث المواعيد
            if (dto.InspectionDates != null && dto.InspectionDates.Any())
            {
                housing.InspectionSlots.Clear();
                housing.InspectionSlots.AddRange(dto.InspectionDates.Select(date => new InspectionSlot
                {
                    StartDateTime = date
                }));
            }

            await _housingRepository.UpdateAsync(housing);
            return BaseResponse.SuccessResponse(" Housing updated successfully");
        }

        public async Task<BaseResponse<InspectionRequestResponseDto>> SubmitInspectionRequestAsync(InspectionRequestDto dto)
        {
            return await _housingRepository.SubmitInspectionRequestAsync(dto);
        }

        public async Task<BaseResponse<bool>> DeleteHousingAsync(int housingId, string userId, bool isAdmin)
        {
            var house = await _housingRepository.GetByIdAsync(housingId);

            if (house == null)
                return new BaseResponse<bool>(false, "Housing not found", false);

            if (house.OwnerId != userId && !isAdmin)
                return new BaseResponse<bool>(false, " You are not authorized to delete this housing", false);

            var deleted = await _housingRepository.DeleteHousingAsync(housingId);
            if (!deleted)
                return new BaseResponse<bool>(false, " Failed to delete housing", false);

            return new BaseResponse<bool>(true, " Housing deleted successfully", true);
        }
        public async Task<BaseResponse<bool>> ToggleFreezeAsync(int id)
        {
            var house = await _housingRepository.GetByIdAsync(id);
            if (house == null)
                return new BaseResponse<bool>(false, "السكن غير موجود.", false);

            house.IsFrozen = !house.IsFrozen;
            await _housingRepository.SaveChangesAsync();

            string message = house.IsFrozen ? "تم تجميد السكن." : " تم إلغاء التجميد.";
            return new BaseResponse<bool>(true, message, house.IsFrozen);
        }
        public async Task<BaseResponse<bool>> SaveHousingAsync(string userId, int housingId)
        {
            if (string.IsNullOrEmpty(userId))
                return BaseResponse<bool>.Failure("UserId is required");

            if (housingId <= 0)
                return BaseResponse<bool>.Failure("Invalid housing Id");

            return await _housingRepository.SaveHousingAsync(userId, housingId);
        }
        public async Task<BaseResponse<IEnumerable<HouseDTO>>> GetAllHousesAsync(string currentUserId)
        {
            var houses = await _housingRepository.GetAllHousesAsync();

            var approvedHouses = houses
           .Where(h => h.Status == HouseStatus.Approved && !h.IsFrozen)
           .ToList();

            if (!approvedHouses.Any())
                return new BaseResponse<IEnumerable<HouseDTO>>(false, " No approved houses found");

            var savedIds = await _context.SavedHousing
          .Where(s => s.UserId == currentUserId)
          .Select(s => s.HousingId)
          .ToListAsync();

            var likedIds = await _context.Likes
                .Where(l => l.UserId == currentUserId && l.EntityType == "Housing")
                .Select(l => l.EntityId)
                .ToListAsync();


            var mapped = houses.Select(h => new HouseDTO
            {
                Id = h.Id,
                Title = h.Title,
                HousingType = h.HousingType.ToString(),
                PricePerMeter = h.PricePerMeter,
                Floor = h.Floor,
                AreaInMeters = h.AreaInMeters,
                Address = h.Address,
                photoUrl = h.PhotoUrl,
                ownerId = h.OwnerId,
                FurnishingStatus = h.FurnishingStatus.ToString(),
                RentalType = h.RentalType.ToString(),
                OwnerName = h.Owner?.FullName,
                Status = h.Status.ToString(),
                IsAvailable = h.IsAvailable,
                IsSaved = savedIds.Contains(h.Id),
                IsLiked = likedIds.Contains(h.Id.ToString()),
                rate = h.HousingRatingAverage,
            })
             .ToList();

            return new BaseResponse<IEnumerable<HouseDTO>>(true, " Houses retrieved successfully", mapped);
        }


        public async Task<BaseResponse<List<InspectionSlotDetailsDto>>> GetAvailableSlotsAsync(int housingId)
        {
            var slots = await _housingRepository.GetAvailableSlotsByHousingIdAsync(housingId);

            if (slots == null || !slots.Any())
                return new BaseResponse<List<InspectionSlotDetailsDto>>(false, " No available slots found");

            var mapped = slots.Select(s => new InspectionSlotDetailsDto
            {
                SlotId = s.Id,
                StartDateTime = s.StartDateTime,
            }).ToList();

            return new BaseResponse<List<InspectionSlotDetailsDto>>(true, "Available slots retrieved successfully", mapped);
        }
        public async Task<BaseResponse<OwnerHousingGroupedDto>> GetGroupedHousingsForLandlordAsync(string landlordId)
        {
            var housings = await _housingRepository.GetHousingsForLandlordIdAsync(landlordId);

            if (housings == null || !housings.Any())
            {
                return BaseResponse<OwnerHousingGroupedDto>.SuccessResponse(
                    new OwnerHousingGroupedDto { availableHousings = new(), RentedHousings = new() },
                    "No housings found for this landlord"
                );
            }

            var result = new OwnerHousingGroupedDto
            {
                availableHousings = new List<HouseDTO>(),
                RentedHousings = new List<HouseDTO>()
            };

            foreach (var h in housings)
            {
                var dto = new HouseDTO
                {
                    Id = h.Id,
                    HousingType = h.HousingType.ToString(),
                    PricePerMeter = h.PricePerMeter,
                    Address = h.Address,
                    photoUrl = h.PhotoUrl,
                    FurnishingStatus = h.FurnishingStatus.ToString(),
                    RentalType = h.RentalType.ToString(),
                    Floor = h.Floor,
                    Status = h.Status.ToString(),
                    IsAvailable = h.IsAvailable,
                    rate = h.HousingRatingAverage,
                    OwnerName = h.Owner?.FullName,
                    ownerId = h.OwnerId,
                };

                if (h.Reservations != null && h.Reservations.Any())
                    result.RentedHousings.Add(dto);
                else
                    result.availableHousings.Add(dto);
            }

            return BaseResponse<OwnerHousingGroupedDto>.SuccessResponse(result, "Housings grouped successfully");
        }
        public async Task<BaseResponse<HousingDetailsDto?>> GetHousingByIdAsync(int id)
        {
            var house = await _housingRepository.GetByIdAsync(id);
            if (house == null)
                return BaseResponse<HousingDetailsDto?>.Failure("Housing not found");

            var dto = new HousingDetailsDto
            {
                Title = house.Title,
                Address = house.Address,
                PricePerMeter = house.PricePerMeter,
                AreaInMeters = house.AreaInMeters,
                Floor = house.Floor,
                HousingType = house.HousingType.ToString(),
                NumberOfRooms = house.NumberOfRooms,
                HasKitchen = house.HasKitchen,
                HasBathroom = house.HasBathroom,
                HasLivingRoom = house.HasLivingRoom,

                HasBed = house.HasBed,
                HasWardrobe = house.HasWardrobe,
                HasChair = house.HasChair,

                HasFridge = house.HasFridge,
                HasStove = house.HasStove,
                HasWasher = house.HasWasher,
                HasFan = house.HasFan,
                HasTV = house.HasTV,
                HasInternet = house.HasInternet,

                HasGas = house.HasGas,
                HasElectricity = house.HasElectricity,
                HasWater = house.HasWater,
                DepositAmount = house.DepositAmount,
                InsuranceAmount = house.InsuranceAmount,
                CommissionAmount = house.CommissionAmount,
                HousingUrl = house.HousingUrl,
                PhotoUrls = house.Photos.Select(p => p.Url).ToList(),
                InspectionSlots = house.InspectionSlots.Select(s => new InspectionSlotDetailsDto
                {
                    SlotId = s.Id,
                    StartDateTime = s.StartDateTime,
                }).ToList(),
                RentDurationInMonths = house.RentdurationUnit switch
                {
                    RentDurationUnit.Month => house.RentDurationValue,
                    RentDurationUnit.Year => house.RentDurationValue * 12,
                    _ => 0
                },
                OwnerName = house.Owner?.FullName,
                OwnerPhone = house.Owner?.PhoneNumber
            };
            return BaseResponse<HousingDetailsDto?>.SuccessResponse(dto, "Housing retrive successfuly");


        }
        public async Task<BaseResponse<List<HouseDTO>>> GetHousingsByHighestRatingAsync(string currentUserId)
        {
            var housings = await _housingRepository.GetHousingsOrderedByRatingAsync();

            if (housings == null || !housings.Any())
                return BaseResponse<List<HouseDTO>>.Failure("No housings found");
            var savedIds = await _context.SavedHousing
             .Where(s => s.UserId == currentUserId)
             .Select(s => s.HousingId)
             .ToListAsync();

            var likedIds = await _context.Likes
                .Where(l => l.UserId == currentUserId && l.EntityType == "Housing")
                .Select(l => l.EntityId)
                .ToListAsync();


            var dtos = housings.Select(h =>
            {
                var dto = MapToDto(h);
                dto.IsSaved = savedIds.Contains(h.Id);
                dto.IsLiked = likedIds.Contains(h.Id.ToString());
                return dto;
            }).ToList();

            return BaseResponse<List<HouseDTO>>.SuccessResponse(dtos, "Housings retrieved and ordered by rating successfully");
        }

        private HouseDTO MapToDto(Saken_WebApplication.Data.Models.Housing h) => new HouseDTO
        {
            Id = h.Id,
            Title = h.Title,
            Address = h.Address,
            PricePerMeter = h.PricePerMeter,
            rate = h.HousingRatingAverage,
            HousingType = h.HousingType.ToString(),
            FurnishingStatus = h.FurnishingStatus.ToString(),
            photoUrl = h.PhotoUrl,
            Status = h.Status.ToString(),
            AreaInMeters = h.AreaInMeters,
            IsAvailable = h.IsAvailable,
            RentalType = h.RentalType.ToString(),
            Floor = h.Floor,
            ownerId = h.OwnerId,
            OwnerName = h.Owner?.FullName,


        };

        public async Task<BaseResponse<List<HouseDTO>>> GetHousingsByLowestPriceAsync(string currentUserId)
        {
            var housings = await _housingRepository.GetHousingsOrderedByPriceAsync();

            if (housings == null || !housings.Any())
                return BaseResponse<List<HouseDTO>>.Failure("No housings found");
            var savedIds = await _context.SavedHousing
            .Where(s => s.UserId == currentUserId)
            .Select(s => s.HousingId)
            .ToListAsync();

            var likedIds = await _context.Likes
                .Where(l => l.UserId == currentUserId && l.EntityType == "Housing")
                .Select(l => l.EntityId)
                .ToListAsync();


            var dtos = housings.Select(h =>
            {
                var dto = MapToDto(h);
                dto.IsSaved = savedIds.Contains(h.Id);
                dto.IsLiked = likedIds.Contains(h.Id.ToString());
                return dto;
            }).ToList();


            return BaseResponse<List<HouseDTO>>.SuccessResponse(dtos, "Housings retrieved and ordered by price successfully");
        }

        public async Task<BaseResponse<List<HouseDTO>>> GetHousingsByTypeAsync(string currentUserId, PropertyType type)
        {
            var housings = await _housingRepository.GetHousingsByTypeAsync(type);
            if (housings == null)
                return BaseResponse<List<HouseDTO>>.Failure("Housings not found");
            var savedIds = await _context.SavedHousing
       .Where(s => s.UserId == currentUserId)
       .Select(s => s.HousingId)
       .ToListAsync();

            var likedIds = await _context.Likes
                .Where(l => l.UserId == currentUserId && l.EntityType == "Housing")
                .Select(l => l.EntityId)
                .ToListAsync();


            var dtos = housings.Select(h =>
            {
                var dto = MapToDto(h);
                dto.IsSaved = savedIds.Contains(h.Id);
                dto.IsLiked = likedIds.Contains(h.Id.ToString());
                return dto;
            }).ToList();

            return BaseResponse<List<HouseDTO>>.SuccessResponse(dtos, "Houses By Type  retrive successfully");
        }


        public async Task<BaseResponse<List<InspectionRequestResponseDto>>> GetInspectionRequestsForOwnerAsync(string ownerId)
        {
            if (string.IsNullOrEmpty(ownerId))
                return new BaseResponse<List<InspectionRequestResponseDto>>(false, " OwnerId is required");

            var requests = await _housingRepository.GetInspectionRequestsByOwnerAsync(ownerId);

            if (requests == null || !requests.Any())
                return new BaseResponse<List<InspectionRequestResponseDto>>(false, " No inspection requests found for this owner");

            return new BaseResponse<List<InspectionRequestResponseDto>>(true, " Inspection requests retrieved successfully", requests);
        }

        public async Task<BaseResponse<List<HouseDTO>>> GetSavedHousingsAsync(string userId)
        {

            var houses = await _housingRepository.GetSavedHousingsAsync(userId);
            if (houses == null || !houses.Any())
                return BaseResponse<List<HouseDTO>>.Failure("No Saved Housing");

            return BaseResponse<List<HouseDTO>>.SuccessResponse(houses, "retrieved successfully");
        }

        public async Task<BaseResponse<IEnumerable<HouseDTO>>> GetPendingHousesAsync()
        {
            var houses = await _housingRepository.GetPendingHousesAsync();
            if (houses == null || !houses.Any())
                return BaseResponse<IEnumerable<HouseDTO>>.Failure("No Houses Founded ");

            var response = houses.Select(MapToDto).ToList();

            return BaseResponse<IEnumerable<HouseDTO>>.SuccessResponse(response, "retrive Houses panding ");
        }


        public async Task<BaseResponse<int>> GetHousesCountAsync()
        {
            var houses = await _housingRepository.GetAllHousesAsync();
            if (houses == null)
                return BaseResponse<int>.Failure("No Houses Found");
            return BaseResponse<int>.SuccessResponse(houses.Count(), "Return Number Of House");
        }
        public async Task<BaseResponse<IEnumerable<HouseDTO>>> SearchHousesAsync(string searchKey)
        {
            var houses = await _housingRepository.SearchHousesAsync(searchKey);
            if (houses == null || !houses.Any())
                return BaseResponse<IEnumerable<HouseDTO>>.Failure("No match house with searchKey");
            var response = houses.Select(MapToDto).ToList();
            return BaseResponse<IEnumerable<HouseDTO>>.SuccessResponse(response, "Retrived Houses successfully");
        }


        public async Task<BaseResponse<IEnumerable<HouseDTO>>> SearchByAddressAsync(string? address, double? lat, double? lng, double radiusKm = 10)
        {
            var houses = await _housingRepository.SearchByAddressAsync(address, lat, lng, radiusKm);
            if (houses == null)
                return BaseResponse<IEnumerable<HouseDTO>>.Failure("No Houses found in this address");

            var response = houses.Select(MapToDto).ToList();
            return BaseResponse<IEnumerable<HouseDTO>>.SuccessResponse(response);
        }

        public async Task<(double lat, double lng)?> GetCoordinatesAsync(string address)
        {
            string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json&limit=1";

            var response = await _httpClient.GetStringAsync(url);
            dynamic results = Newtonsoft.Json.JsonConvert.DeserializeObject(response);

            if (results.Count > 0)
            {
                double lat = double.Parse((string)results[0].lat, System.Globalization.CultureInfo.InvariantCulture);
                double lng = double.Parse((string)results[0].lon, System.Globalization.CultureInfo.InvariantCulture);
                return (lat, lng);
            }

            return null;
        }

        public async Task<BaseResponse<IEnumerable<HousingDto>>> GetFilteredHousesAsync(string? address, string? housingType, string? furnishingStatus, decimal? minPrice, decimal? maxPrice)
        {
            var houses = await _housingRepository.FilterHousesAsync(address, housingType, furnishingStatus, minPrice, maxPrice);
            if (houses == null)
                return BaseResponse<IEnumerable<HousingDto>>.Failure("No Houses Found");
            var response = houses.Select(h => new HousingDto
            {
                Title = h.Title,
                Address = h.Address,
                PricePerMeter = h.PricePerMeter,
                HousingType = h.HousingType.ToString(),
                FurnishingStatus = h.FurnishingStatus.ToString(),
                photoUrl = h.PhotoUrl,
                ownerId = h.OwnerId
            });
            return BaseResponse<IEnumerable<HousingDto>>.SuccessResponse(response, "Retrived result ");
        }
        public async Task<BaseResponse<bool>> ApproveHousingAsync(int housingId)
        {
            var house = await _housingRepository.GetByIdAsync(housingId);
            if (house == null)
                return BaseResponse<bool>.Failure("No House founded ");

            await _housingRepository.ApproveHousingAsync(housingId);
            await _notificationService.SendNotificationAsync(
             house.OwnerId,
            " تمت الموافقة",
             $"تمت الموافقة على عرضك للوحدة {house.Title}."
   );
            return BaseResponse<bool>.SuccessResponse(true, "Approved House");
        }

        public async Task<BaseResponse<string>> RejectHouseAsync(int houseId, string reason)
        {
            var housing = await _housingRepository.GetByIdAsync(houseId);
            if (housing == null)
                return BaseResponse<string>.Failure("No house founded ");
            await _housingRepository.RejectHouseAsync(houseId, reason);
            await _notificationService.SendNotificationAsync(
            housing.OwnerId,
            " تم الرفض",
            $"تم رفض عرضك للوحدة {housing.Title}. السبب: {reason}"
    );
            return BaseResponse<string>.SuccessResponse("house rejected..");

        }















        /*public async Task AddReservationAsync(ReservationDto dto, string userId)
        {
            var housing = await _housingRepository.GetByIdAsync(dto.HousingId);

            if (housing == null)
                throw new Exception("Housing not found");

            var reservation = new Saken_WebApplication.Data.Models.Reservation
            {
                HousingId = dto.HousingId,
                UserId = userId,
                LandlordId = housing.OwnerId,
                ReservationDate = DateTime.UtcNow,          // الوقت الحالي
                AmountPaid = dto.AmountPaid,
                Status = ReservationStatus.Pending          // الحالة الابتدائية
            };

            await _housingRepository.AddReservationAsync(reservation);
        }*/















        /*   public async Task<List<TenantReservationDto>> GetReservationsForTenantAsync(string userId)
           {
               var reservations = await _housingRepository.GetReservationsForTenantAsync(userId);

               return reservations.Select(r => new TenantReservationDto
               {
                   HousingTitle = r.Housing.Title,
                   Address = r.Housing.Address,
                   ReservationDate = r.ReservationDate,
                   OwnerName = r.Housing.Owner.FullName,
                   PricePerMonth = r.Housing.PricePerMeter,
                   ImageUrl = r.Housing.PhotoUrl
               }).ToList();
           }*/











        // HousingService.cs






    }
}
