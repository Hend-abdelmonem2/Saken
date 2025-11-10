using MediatR;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Core.Features.Houses.Query.Models
{
    public record GetHousingsByTypeQuery(string userId, PropertyType Type) : IRequest<BaseResponse<List<HouseDTO>>>;
}
