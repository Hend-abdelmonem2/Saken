using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.DTO.HousingDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces.housing
{
    public interface IHousingService
    {
        Task AddHousingAsync(HousingDto dto , string landlordId);
        Task AddReservationAsync(ReservationDto dto, string userId);
        Task<IEnumerable<HousingDto>> GetAllHousesAsync();
        Task UpdateHousingAsync(int id, HousingDto dto);
        Task<IEnumerable<HousingDto>> SearchHousesAsync(string searchKey);

        Task<bool> DeleteHousingAsync(int houseId);
        Task<IEnumerable<HousingDto>> GetHousingsForLandlordIdAsync(string landlordId);
        Task<HousingDto?> GetHousingByIdAsync(int id);


        Task<(bool success, string message, bool isFrozen)> ToggleFreezeAsync(int id);

    }
}

