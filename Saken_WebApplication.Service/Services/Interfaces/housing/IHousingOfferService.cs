using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces.housing
{
    public interface IHousingOfferService
    {
        Task<BaseResponse<IEnumerable<OfferDto>>> GetActiveOffersAsync();
        Task<BaseResponse<IEnumerable<HousingOffer>>> GetOffersByHousingIdAsync(int housingId);
        Task<BaseResponse<string>> CreateOfferAsync(HousingOfferDto dto);
    }
}
