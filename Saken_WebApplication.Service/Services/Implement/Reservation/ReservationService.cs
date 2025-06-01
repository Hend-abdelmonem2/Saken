using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Implement.Reservation
{
    public class ReservationService: IReservationService
    {
        private  readonly IHousingRepository _housingRepository;
        public ReservationService(IHousingRepository housingRepository)
        {
            _housingRepository = housingRepository;
        }
        public async Task<IEnumerable<GetReservationDto>> GetReservationsForLandlordAsync(string landlordId)
        {
            var reservations = await _housingRepository.GetReservationsByLandlordIdAsync(landlordId);

            return reservations.Select(r => new GetReservationDto
            {
                Id = r.res_Id,
                HousingId = r.HousingId,
                AmountPaid = r.AmountPaid,
                ReservationDate = r.ReservationDate,
                Status = r.Status,
                TenantName = r.Tenant?.FullName,       // اسم المستأجر لو حابه ترجعيه
                HousingAddress = r.Housing?.address    // عنوان السكن مثلاً
            }).ToList();
        }
        public async Task<ReservationContractDto> GetReservationContractAsync(int  reservationId)
        {
            var reservation = await _housingRepository.GetReservationByIdAsync(reservationId);

            if (reservation == null)
                return null;

            return new ReservationContractDto
            {
                LandlordName = reservation.Housing?.Landlord?.FullName,
                LandlordPhone = reservation.Housing?.Landlord?.PhoneNumber,

                TenantName = reservation.Tenant?.FullName,
                TenantPhone = reservation.Tenant?.PhoneNumber,

                HousingAddress = reservation.Housing?.address,
                ReservationDate = reservation.ReservationDate,
                AmountPaid = reservation.AmountPaid,
                Status = reservation.Status.ToString(),

            };
        }
        public async Task<GetReservationDto> GetReservationById(int id)
        {
            var reservation = await _housingRepository.GetReservationByIdAsync(id);

            if (reservation == null)
                return null;

            return new GetReservationDto
            {
                Id = reservation.res_Id,
                HousingId = reservation.HousingId,
                AmountPaid = reservation.AmountPaid,
                ReservationDate = reservation.ReservationDate,
                Status = reservation.Status,
                TenantName = reservation.Tenant?.FullName,
                HousingAddress = reservation.Housing?.address
            };
        }
    }
}
