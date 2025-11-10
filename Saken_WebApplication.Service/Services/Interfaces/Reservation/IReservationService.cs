using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces.Reservation
{
    public  interface IReservationService
    {
        Task<BaseResponse<ReservationResponseDto>> AddReservationAsync(ReservationDto dto, string userId);
        Task<BaseResponse<DateTime?>> GetAvailableFromAsync(int housingId);
        Task<BaseResponse<IEnumerable<GetReservationDto>>> GetReservationsForLandlordAsync(string landlordId);
        Task<BaseResponse<ReservationContractDto>> GetReservationContractAsync(int reservationId);
        Task<BaseResponse<GetReservationDto>> GetReservationById(int id);

        Task<BaseResponse<HousingCostsDto>> GetHousingCostsAsync(int housingId, int durationInMonths);
        Task<BaseResponse<List<TenantReservationDto>>> GetReservationsForTenantAsync(string userId);

        Task<BaseResponse<List<TenantReservationDto>>> GetConfirmedReservationsForTenantAsync(string userId);
        Task<BaseResponse<List<AllReservationDto>>> GetAllReservationsAsync();
        Task<BaseResponse<int>> GetAllReservationsCountAsync();


        Task<BaseResponse<string>> ConfirmReservationAsync(int reservationId, string landlordId);
        Task<BaseResponse<string>> CancelReservationAsync(int reservationId, string userId);
        Task<BaseResponse<string>> SendAvailabilityNotificationToAllUsers(int housingId, string housingTitle);



    }
}
