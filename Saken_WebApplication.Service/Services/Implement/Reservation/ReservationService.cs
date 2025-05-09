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
                HousingId = r.HousingId,
                AmountPaid = r.AmountPaid,
                ReservationDate = r.ReservationDate,
                Status = r.Status,
                TenantName = r.Tenant?.FullName,       // اسم المستأجر لو حابه ترجعيه
                HousingAddress = r.Housing?.address    // عنوان السكن مثلاً
            }).ToList();
        }
    }
}
