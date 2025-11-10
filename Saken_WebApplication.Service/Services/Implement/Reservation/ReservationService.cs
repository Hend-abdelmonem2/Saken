using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Reservation;
using Saken_WebApplication.Service.Services.Interfaces.Notifications;
using Saken_WebApplication.Service.Services.Interfaces.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Service.Services.Implement.Reservation
{
    public class ReservationService : IReservationService
    {
        private readonly IHousingRepository _housingRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly INotificationService _notificationService;
        private readonly IUserRepository _userRepository;
        private readonly IAdminRepository _adminRepository;
        public ReservationService(IHousingRepository housingRepository, IReservationRepository reservationRepository, INotificationService notificationService, IUserRepository userRepository, IAdminRepository adminRepository)
        {
            _housingRepository = housingRepository;
            _reservationRepository = reservationRepository;
            _notificationService = notificationService;
            _userRepository = userRepository;
            _adminRepository = adminRepository;
        }
        public async Task<BaseResponse<ReservationResponseDto>> AddReservationAsync(ReservationDto dto, string userId)
        {
            var housing = await _reservationRepository.GetHousingWithDetailsAsync(dto.HousingId);
            if (housing == null)
                return new BaseResponse<ReservationResponseDto>(false, " السكن غير موجود");

            DateTime endDate = dto.StartDateTime.AddMonths(dto.DurationInMonths);

            bool isOverlapping = await _reservationRepository.IsOverlappingReservationAsync(dto.HousingId, dto.StartDateTime, endDate);
            if (isOverlapping)
                return new BaseResponse<ReservationResponseDto>(false, " هذا السكن محجوز في هذه الفترة");

            decimal monthlyPrice = housing.PricePerMeter;
            decimal total = monthlyPrice * dto.DurationInMonths;

            var user = await _userRepository.GetByIdAsync(userId);
            var tenantName = user?.FullName ?? "مستخدم";
            var tenantPhone = user?.PhoneNumber ?? "غير متوفر";

            var reservation = new Saken_WebApplication.Data.Models.Reservation
            {
                HousingId = dto.HousingId,
                UserId = userId,
                LandlordId = housing.OwnerId,
                StartDateTime = dto.StartDateTime,
                EndDateTime = endDate,
                DurationInMonths = dto.DurationInMonths,
                AmountPaid = total,
                Status = ReservationStatus.Pending,
                ReservationDate = DateTime.UtcNow
            };

            housing.LastRentedDate = endDate;

            await _reservationRepository.AddReservationAsync(reservation);
            await _reservationRepository.SaveChangesAsync();

            await _notificationService.SendNotificationAsync(
                housing.OwnerId,
                "طلب حجز جديد",
                $"تم استلام طلب حجز من {tenantName} - {tenantPhone}"
            );

            var responseDto = new ReservationResponseDto
            {
                ReservationNumber = $"#{Guid.NewGuid():N}".Substring(0, 6).ToUpper(),
                StartDateTime = reservation.StartDateTime,
                DurationInMonths = reservation.DurationInMonths,
                LandlordName = housing.Owner?.FullName ?? "غير معروف",
                HousingType = housing.HousingType.ToString(),
                Location = housing.Address,
                RoomCount = housing.NumberOfRooms,
                MonthlyPrice = monthlyPrice,
                Rating = housing.HousingRatingAverage,
                ImageUrl = housing.PhotoUrl
            };

            return new BaseResponse<ReservationResponseDto>(true, " تم إنشاء الحجز بنجاح", responseDto);
        }

     
        public async Task<BaseResponse<DateTime?>> GetAvailableFromAsync(int housingId)
        {
            var availableFrom = await _housingRepository.GetAvailableFromAsync(housingId);
            return new BaseResponse<DateTime?>(true, " تم جلب تاريخ التوفر", availableFrom);
        }

    
        public async Task<BaseResponse<HousingCostsDto>> GetHousingCostsAsync(int housingId, int durationInMonths)
        {
            var housing = await _housingRepository.GetHousingCostsByIdAsync(housingId);
            if (housing == null)
                return new BaseResponse<HousingCostsDto>(false, " السكن غير موجود");

            decimal monthlyPrice = housing.PricePerMeter;
            decimal depositAmount = housing.DepositAmount;
            decimal insuranceAmount = housing.InsuranceAmount;
            decimal commissionAmount = housing.CommissionAmount;

            decimal rentTotal = monthlyPrice * durationInMonths;
            decimal totalAmount = rentTotal + depositAmount + insuranceAmount + commissionAmount;

            var dto = new HousingCostsDto
            {
                MonthlyPrice = monthlyPrice,
                DepositAmount = depositAmount,
                InsuranceAmount = insuranceAmount,
                CommissionAmount = commissionAmount,
                RentTotal = rentTotal,
                TotalAmount = totalAmount
            };

            return new BaseResponse<HousingCostsDto>(true, " تم حساب التكاليف بنجاح", dto);
        }

    
        public async Task<BaseResponse<IEnumerable<GetReservationDto>>> GetReservationsForLandlordAsync(string landlordId)
        {
            var reservations = await _reservationRepository.GetReservationsByLandlordIdAsync(landlordId);

            var result = reservations.Select(r => new GetReservationDto
            {
                Id = r.res_Id,
                HousingId = r.HousingId,
                AmountPaid = r.AmountPaid,
                ReservationDate = r.ReservationDate,
                Status = r.Status,
                TenantName = r.Tenant?.FullName,
                HousingAddress = r.Housing?.Address
            }).ToList();
            if (result.Count == 0)
                return BaseResponse<IEnumerable<GetReservationDto>>.Failure("No Reservations found for user");

            return new BaseResponse<IEnumerable<GetReservationDto>>(true, " تم جلب الحجوزات", result);
        }


        public async Task<BaseResponse<ReservationContractDto>> GetReservationContractAsync(int reservationId)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId);
            if (reservation == null)
                return new BaseResponse<ReservationContractDto>(false, "الحجز غير موجود");

            // 🔒 السماح فقط للحجوزات المؤكدة
            if (reservation.Status != ReservationStatus.Confirmed)
                return new BaseResponse<ReservationContractDto>(false, "العقد متاح فقط للحجوزات المؤكدة");

            var dto = new ReservationContractDto
            {
                LandlordName = reservation.Housing?.Owner?.FullName,
                LandlordPhone = reservation.Housing?.Owner?.PhoneNumber,
                TenantName = reservation.Tenant?.FullName,
                TenantPhone = reservation.Tenant?.PhoneNumber,
                HousingAddress = reservation.Housing?.Address,
                ReservationDate = reservation.ReservationDate,
                AmountPaid = reservation.AmountPaid,
                Status = reservation.Status.ToString(),
            };

            return new BaseResponse<ReservationContractDto>(true, "تم جلب عقد الحجز بنجاح", dto);
        }



        public async Task<BaseResponse<GetReservationDto>> GetReservationById(int id)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
            if (reservation == null)
                return new BaseResponse<GetReservationDto>(false, " الحجز غير موجود");

            var dto = new GetReservationDto
            {
                Id = reservation.res_Id,
                HousingId = reservation.HousingId,
                AmountPaid = reservation.AmountPaid,
                ReservationDate = reservation.ReservationDate,
                Status = reservation.Status,
                TenantName = reservation.Tenant?.FullName,
                HousingAddress = reservation.Housing?.Address
            };

            return new BaseResponse<GetReservationDto>(true, " تم جلب الحجز", dto);
        }

 
        public async Task<BaseResponse<List<TenantReservationDto>>> GetReservationsForTenantAsync(string userId)
        {
            var reservations = await _reservationRepository.GetReservationsForTenantAsync(userId);

            var result = reservations.Select(r => new TenantReservationDto
            {
                HousingTitle = r.Housing.Title,
                Address = r.Housing.Address,
                ReservationDate = r.ReservationDate,
                OwnerName = r.Housing.Owner.FullName,
                PricePerMonth = r.Housing.PricePerMeter,
                ImageUrl = r.Housing.PhotoUrl
            }).ToList();
            if (result.Count == 0)
                return BaseResponse<List<TenantReservationDto>>.Failure("No Reservations found for user");

            return new BaseResponse<List<TenantReservationDto>>(true, " تم جلب حجوزات المستأجر", result);
        }

      
        public async Task<BaseResponse<List<TenantReservationDto>>> GetConfirmedReservationsForTenantAsync(string userId)
        {
            var reservations = await _reservationRepository.GetConfirmedReservationsForTenantAsync(userId);

            var result = reservations.Select(r => new TenantReservationDto
            {
                HousingTitle = r.Housing.Title,
                Address = r.Housing.Address,
                ReservationDate = r.ReservationDate,
                OwnerName = r.Housing.Owner.FullName,
                PricePerMonth = r.Housing.PricePerMeter,
                ImageUrl = r.Housing.PhotoUrl
            }).ToList();
            if (result.Count == 0)
                return BaseResponse<List<TenantReservationDto>>.Failure("No confirmed Reservations found for user");

            return new BaseResponse<List<TenantReservationDto>>(true, " تم جلب الحجوزات المؤكدة", result);
        }

  
        public async Task<BaseResponse<string>> ConfirmReservationAsync(int reservationId, string landlordId)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId);
            if (reservation == null)
                return new BaseResponse<string>(false, " الحجز غير موجود");

            if (reservation.LandlordId != landlordId)
                return new BaseResponse<string>(false, " غير مصرح لك بتأكيد هذا الحجز");

            var housing = await _housingRepository.GetByIdAsync(reservation.HousingId);
            if (housing == null)
                return new BaseResponse<string>(false, " السكن غير موجود");

            await _reservationRepository.UpdateStatusAsync(
                reservationId,
                ReservationStatus.Confirmed,
                reservation.EndDateTime
            );

            housing.IsAvailable = false;
            await _housingRepository.UpdateAsync(housing);

            await _notificationService.SendNotificationAsync(
                reservation.UserId,
                " تم تأكيد الحجز",
                $"تهانينا! تم تأكيد حجزك للوحدة {reservation.Housing.Title}."
            );

            var availableFrom = housing.AvailableFrom;

            if (availableFrom.HasValue)
            {
                Hangfire.BackgroundJob.Schedule(
                    () => SendAvailabilityNotificationToAllUsers(housing.Id, housing.Title),
                    availableFrom.Value
                );
            }

            return new BaseResponse<string>(true, " تم تأكيد الحجز بنجاح");
        }

      
        public async Task<BaseResponse<string>> CancelReservationAsync(int reservationId, string userId)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId);
            if (reservation == null)
                return new BaseResponse<string>(false, " الحجز غير موجود");

            if (!string.Equals(reservation.UserId?.Trim(), userId?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(reservation.LandlordId?.Trim(), userId?.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return new BaseResponse<string>(false, "غير مصرح لك بإلغاء هذا الحجز");
            }

            reservation.Status = ReservationStatus.Cancelled;
            await _reservationRepository.SaveChangesAsync();

            await _notificationService.SendNotificationAsync(
                reservation.UserId,
                " تم إلغاء الحجز",
                $"تم إلغاء حجزك للوحدة {reservation.Housing.Title}."
            );

            return new BaseResponse<string>(true, " تم إلغاء الحجز بنجاح");
        }


        public async Task<BaseResponse<List<AllReservationDto>>> GetAllReservationsAsync()
        {
            var reservations = await _reservationRepository.GetAllReservationsAsync();

            if (reservations == null || !reservations.Any())
                return new BaseResponse<List<AllReservationDto>>(false, " لا توجد حجوزات متاحة");

            var result = reservations.Select(r => new AllReservationDto
            {
                Id = r.res_Id,
                HousingId = r.HousingId,
                Price = r.AmountPaid,
                Status = r.Status.ToString(),
                TenantName = r.Tenant?.FullName,
                HousingTitle = r.Housing?.Title
            }).ToList();

            return new BaseResponse<List<AllReservationDto>>(true, " تم جلب كل الحجوزات", result);
        }


        public async Task<BaseResponse<string>> SendAvailabilityNotificationToAllUsers(int housingId, string housingTitle)
        {
            var allUsers = await _adminRepository.GetAllUsersAsync();

            if (allUsers == null || !allUsers.Any())
            {
                return new BaseResponse<string>(false, " لا يوجد مستخدمين لإرسال الإشعارات");
            }

            foreach (var user in allUsers)
            {
                await _notificationService.SendNotificationAsync(
                    user.Id,
                    " وحدة متاحة",
                    $"الوحدة {housingTitle} أصبحت متاحة الآن للحجز."
                );
            }

            return new BaseResponse<string>(
                true,
                "تم إرسال إشعارات توفر الوحدة لكل المستخدمين بنجاح",
                housingTitle
            );
        }


        public async Task<BaseResponse<int>> GetAllReservationsCountAsync()
        {
            var count = await _reservationRepository.GetReservationsCountAsync();
            return new BaseResponse<int>(true, " تم جلب عدد الحجوزات", count);
        }








    }
}
