using MediatR;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Houses.Query.Models
{
    public class GetFilteredHousesQuery : IRequest<BaseResponse<IEnumerable<HousingDto>>>
    {
        public string? Address { get; }
        public string? HousingType { get; }
        public string? FurnishingStatus { get; }
        public decimal? MinPrice { get; }
        public decimal? MaxPrice { get; }

        public GetFilteredHousesQuery(string? address, string? housingType, string? furnishingStatus, decimal? minPrice, decimal? maxPrice)
        {
            Address = address;
            HousingType = housingType;
            FurnishingStatus = furnishingStatus;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
        }
    }
}
