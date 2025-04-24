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

        public HousingService(IHousingRepository housingRepository, ICloudinaryService cloudinaryService)
        {
            _housingRepository = housingRepository;
            _cloudinaryService = cloudinaryService;

        }

        public async Task AddHousingAsync(HousingDto dto, string landlordId)
        {
            var housing = new Data.Models.Housing
            {
                type = Enum.Parse<PropertyType>(dto.Type, ignoreCase: true),
                price = dto.Price,
                address = dto.Address,
                // Photo = dto.PhotoUrl,
                status = Enum.Parse<HousingStatus>(dto.Status, ignoreCase: true),
                furnishingStatus = Enum.Parse<FurnishingStatus>(dto.FurnishingStatus, ignoreCase: true),
                targetCustomers = Enum.Parse<TargetCustomerType>(dto.TargetCustomers, ignoreCase: true),
                rentalPeriod = Enum.Parse<RentalDuration>(dto.RentalPeriod, ignoreCase: true),
                deposit = dto.Deposit,
                rent = dto.Rent,
                insurance = dto.Insurance,
                commission = dto.Commission,
                participationLink = dto.ParticipationLink,
                RentalType = Enum.Parse<RentalType>(dto.RentalType, ignoreCase: true),
                InspectionDate = dto.InspectionDate,
                LandlordId = landlordId,
                rating = 0
            };

            if (dto.Photo != null)
            {
                var uploadResult = await _cloudinaryService.UploadImageAsync(dto.Photo);

                if (uploadResult.Error != null)
                {

                    throw new ApplicationException($"Image upload failed: {uploadResult.Error.Message}");
                }

                housing.Photo = uploadResult.SecureUrl.ToString();
            }
            await _housingRepository.AddHousingAsync(housing);
        }
        public async Task AddReservationAsync(ReservationDto dto, string userId)
        {
            var reservation = new Reservation
            {
                HousingId = dto.HousingId,
                UserId = userId,
                ReservationDate = DateTime.UtcNow,          // الوقت الحالي
                AmountPaid = dto.AmountPaid,
                Status = ReservationStatus.Pending          // الحالة الابتدائية
            };

            await _housingRepository.AddReservationAsync(reservation);
        }
        public async Task<IEnumerable<HousingDto>> GetAllHousesAsync()
        {
            var houses = await _housingRepository.GetAllHousesAsync();

            return houses.Select(h => new HousingDto
            {
                Type = h.type.ToString(),
                Price = h.price,
                Address = h.address,
                Status = h.status.ToString(),
                FurnishingStatus = h.furnishingStatus.ToString(),
                TargetCustomers = h.targetCustomers.ToString(),
                RentalPeriod = h.rentalPeriod.ToString(),
                Deposit = h.deposit,
                Rent = h.rent,
                Insurance = h.insurance,
                Commission = h.commission,
                ParticipationLink = h.participationLink,
                RentalType = h.RentalType.ToString(),
                InspectionDate = h.InspectionDate,
                photoUrl = h.Photo,
                LandlordId = h.LandlordId,
            }).ToList();
        }
        public async Task UpdateHousingAsync(int id, HousingDto dto)
        {
            var housing = await _housingRepository.GetByIdAsync(id);
            if (housing == null)
                throw new Exception("Housing not found");

            // رفع صورة جديدة لو في صورة
            if (dto.Photo != null && dto.Photo.Length > 0)
            {
                var uploadResult = await _cloudinaryService.UploadImageAsync(dto.Photo);

                if (uploadResult.Error != null)
                    throw new ApplicationException("Image upload failed: " + uploadResult.Error.Message);

                housing.Photo = uploadResult.SecureUrl.ToString();
            }

            // تحديث باقي البيانات
            housing.type = Enum.Parse<PropertyType>(dto.Type, ignoreCase: true);
            housing.price = dto.Price;
            housing.address = dto.Address;
            housing.status = Enum.Parse<HousingStatus>(dto.Status, ignoreCase: true);
            housing.furnishingStatus = Enum.Parse<FurnishingStatus>(dto.FurnishingStatus, ignoreCase: true);
            housing.targetCustomers = Enum.Parse<TargetCustomerType>(dto.TargetCustomers, ignoreCase: true);
            housing.rentalPeriod = Enum.Parse<RentalDuration>(dto.RentalPeriod, ignoreCase: true);
            housing.deposit = dto.Deposit;
            housing.rent = dto.Rent;
            housing.insurance = dto.Insurance;
            housing.commission = dto.Commission;
            housing.participationLink = dto.ParticipationLink;
            housing.RentalType = Enum.Parse<RentalType>(dto.RentalType, ignoreCase: true);
            housing.InspectionDate = dto.InspectionDate;

            await _housingRepository.UpdateAsync(housing);
        }
    }
}
