using AutoMapper;
using NuGet.Packaging;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Service.Services.Implement.housing
{
    public class HousingService : IHousingService
    {
        private readonly IHousingRepository _housingRepository;
        private readonly ICloudinaryService _cloudinaryService;
       // private readonly IMapper _mapper;

        public HousingService(IHousingRepository housingRepository , ICloudinaryService cloudinaryService)
        {
            _housingRepository = housingRepository;
            _cloudinaryService = cloudinaryService;
            //_mapper = mapper;
        }

        public async Task AddHousingAsync(HousingDto dto, string landlordId)
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
            await _housingRepository.AddHousingAsync(housing);
        }

        public async Task<InspectionRequestResponseDto> SubmitInspectionRequestAsync(InspectionRequestDto dto)
        {
            return await _housingRepository.SubmitInspectionRequestAsync(dto);
        }

        public async Task<List<InspectionSlotDetailsDto>> GetAvailableSlotsAsync(int housingId)
        {
            var slots = await _housingRepository.GetAvailableSlotsByHousingIdAsync(housingId);

            return slots
                .Where(s => !s.IsBooked)
                .Select(s => new InspectionSlotDetailsDto
                {
                    SlotId = s.Id,
                    StartDateTime = s.StartDateTime,
                })
                .ToList();
        }

        public async Task<List<InspectionRequestResponseDto>> GetInspectionRequestsForOwnerAsync(string ownerId)
        {
            return await _housingRepository.GetInspectionRequestsByOwnerAsync(ownerId);
        }




        public async Task AddReservationAsync(ReservationDto dto, string userId)
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
        }
        public async Task<IEnumerable<HouseDTO>> GetAllHousesAsync()
        {
            var houses = await _housingRepository.GetAllHousesAsync();

            return houses.Select(h => new HouseDTO

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
                OwnerName = h.Owner?.FullName
            }).ToList();
        }



        public async Task UpdateHousingAsync(int id, HousingDto dto)
        {
            var housing = await _housingRepository.GetByIdWithPhotosAsync(id);
            if (housing == null)
                throw new Exception("Housing not found");

            // رفع صورة رئيسية
            if (dto.Photo != null && dto.Photo.Length > 0)
            {
                var uploadResult = await _cloudinaryService.UploadImageAsync(dto.Photo);
                if (uploadResult.Error != null)
                    throw new ApplicationException("Image upload failed: " + uploadResult.Error.Message);

                housing.PhotoUrl = uploadResult.SecureUrl.ToString(); // تحديث الصورة الرئيسية

                housing.Photos.Add(new HousingPhoto
                {
                    Url = uploadResult.SecureUrl.ToString(),
                    PublicId = uploadResult.PublicId,
                    UploadedAt = DateTime.UtcNow
                });
            }

            // صور إضافية
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

            // تحديث البيانات العامة
            housing.Title = dto.Title;
            housing.Address = dto.Address;
            housing.PricePerMeter = dto.PricePerMeter;
            housing.AreaInMeters = dto.AreaInMeters;
            housing.Floor = dto.Floor;
            housing.HousingType = Enum.Parse<PropertyType>(dto.HousingType, true);
            housing.FurnishingStatus = Enum.Parse<FurnishingStatus>(dto.FurnishingStatus, true);
            housing.RentalType = Enum.Parse<RentalType>(dto.RentalType, true);
            housing.RentDurationValue = dto.RentDurationValue;
            housing.RentdurationUnit = Enum.Parse<RentDurationUnit>(dto.RentDurationUnit, true);

            // تاريخ آخر إيجار (إن أردت تحديثه من dto لاحقًا)
            // housing.LastRentedDate = dto.LastRentedDate;

            // وسائل الراحة
            housing.HasKitchen = dto.HasKitchen;
            housing.HasBathroom = dto.HasBathroom;
            housing.HasLivingRoom = dto.HasLivingRoom;

            // الأثاث
            housing.HasBed = dto.HasBed;
            housing.HasWardrobe = dto.HasWardrobe;
            housing.HasChair = dto.HasChair;

            // الأجهزة
            housing.HasFridge = dto.HasFridge;
            housing.HasStove = dto.HasStove;
            housing.HasWasher = dto.HasWasher;
            housing.HasFan = dto.HasFan;
            housing.HasTV = dto.HasTV;
            housing.HasInternet = dto.HasInternet;

            // المرافق
            housing.HasGas = dto.HasGas;
            housing.HasElectricity = dto.HasElectricity;
            housing.HasWater = dto.HasWater;

            // المستأجر المستهدف
            housing.TargetTenantType = Enum.Parse<TargetCustomerType>(dto.TargetTenantType, true);
            housing.TargetTenantDescription = dto.TargetTenantDescription;

            // المدفوعات
            housing.DepositAmount = dto.DepositAmount;
            housing.InsuranceAmount = dto.InsuranceAmount;
            housing.CommissionAmount = dto.CommissionAmount;

            housing.HousingUrl = dto.HousingUrl;

            // تحديث المالك (اختياري حسب صلاحيات التعديل)
            if (!string.IsNullOrEmpty(dto.ownerId))
                housing.OwnerId = dto.ownerId;
            housing.InspectionSlots.Clear();

            if (dto.InspectionDates != null && dto.InspectionDates.Any())
            {
                housing.InspectionSlots.AddRange(dto.InspectionDates.Select(date => new InspectionSlot
                {
                    StartDateTime = date
                }));
            }

            await _housingRepository.UpdateAsync(housing);
        }



        public async Task<IEnumerable<HouseDTO>> SearchHousesAsync(string searchKey)
        {
            var houses = await _housingRepository.SearchHousesAsync(searchKey);

            return houses.Select(h => new HouseDTO
            {
                Id = h.Id,
                Title = h.Title,
                Address = h.Address,
                photoUrl = h.PhotoUrl,
                PricePerMeter = h.PricePerMeter,
                AreaInMeters = h.AreaInMeters,
                Floor = h.Floor,
                HousingType = h.HousingType.ToString(),
                FurnishingStatus = h.FurnishingStatus.ToString(),
                RentalType = h.RentalType.ToString(),
                OwnerName = h.Owner?.FullName
            }).ToList();
        }

        public async Task<bool> DeleteHousingAsync(int houseId)
        {

            var housing = await _housingRepository.GetByIdAsync(houseId);
            if (housing == null)
                throw new Exception("Housing not found");
            return await _housingRepository.DeleteHousingAsync(houseId);
        }

        public async Task<OwnerHousingGroupedDto> GetGroupedHousingsForLandlordAsync(string landlordId)
        {
            var housings = await _housingRepository.GetHousingsForLandlordIdAsync(landlordId);

            var result = new OwnerHousingGroupedDto();

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
                    OwnerName = h.Owner?.FullName
                };

                if (h.Reservations != null && h.Reservations.Any())
                    result.RentedHousings.Add(dto);
                else
                    result.AvailableHousings.Add(dto);
            }

            return result;
        }

        public async Task<List<TenantReservationDto>> GetReservationsForTenantAsync(string userId)
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
        }

        public async Task<List<HouseDTO>> GetHousingsByLowestPriceAsync()
        {
            var housings = await _housingRepository.GetHousingsOrderedByPriceAsync();
            return housings.Select(MapToDto).ToList();
        }

        public async Task<List<HouseDTO>> GetHousingsByHighestRatingAsync()
        {
            var housings = await _housingRepository.GetHousingsOrderedByRatingAsync();
            return housings.Select(MapToDto).ToList();
        }

        private HouseDTO MapToDto(Saken_WebApplication.Data.Models.Housing h) => new HouseDTO
        {
            Id = h.Id,
            Address = h.Address,
            PricePerMeter = h.PricePerMeter,
            rate = h.HousingRatingAverage,
            HousingType = h.HousingType.ToString(),
            FurnishingStatus = h.FurnishingStatus.ToString(),
            OwnerName = h.Owner?.FullName,
            photoUrl = h.PhotoUrl
        };
        public async Task<(bool success, string message, bool isFrozen)> ToggleFreezeAsync(int id)
        {
            var house = await _housingRepository.GetByIdAsync(id);
            if (house == null)
                return (false, "السكن غير موجود.", false);

            house.IsFrozen = !house.IsFrozen;
            await _housingRepository.SaveChangesAsync();

            string message = house.IsFrozen ? "تم تجميد السكن." : "تم إلغاء التجميد.";
            return (true, message, house.IsFrozen);
        }

        public async Task<HousingDetailsDto?> GetHousingByIdAsync(int id)
        {
            var house = await _housingRepository.GetByIdAsync(id);
            if (house == null)
                return null;

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
            return dto;


        }

        public async Task<bool> SaveHousingAsync(string userId, int housingId)
        {
            return await _housingRepository.SaveHousingAsync(userId, housingId);
        }

        // HousingService.cs
        public async Task<List<HouseDTO>> GetSavedHousingsAsync(string userId)
        {
            return await _housingRepository.GetSavedHousingsAsync(userId);
        }



    }
}
