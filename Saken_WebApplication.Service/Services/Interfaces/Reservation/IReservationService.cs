using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.DTO.HousingDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces.Reservation
{
    public  interface IReservationService
    {
        Task<IEnumerable<GetReservationDto>> GetReservationsForLandlordAsync(string landlordId);
        Task<ReservationContractDto> GetReservationContractAsync(int  reservationId);
        Task <GetReservationDto> GetReservationById(int id);
    }
}
