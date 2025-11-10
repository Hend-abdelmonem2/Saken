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
    public class SearchByAddressQuery : IRequest<BaseResponse<IEnumerable<HouseDTO>>>
    {
        public string? Address { get; }
        public double? Lat { get; }
        public double? Lng { get; }
        public double RadiusKm { get; }
        public SearchByAddressQuery(string? address, double? lat, double? lng, double radiusKm = 10)
        {
            Address = address;
            Lat = lat;
            Lng = lng;
            RadiusKm = radiusKm;
        }
    }
}
