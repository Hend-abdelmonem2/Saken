using MediatR;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;

namespace Saken_WebApplication.Core.Features.Houses.Query.Models
{
    public record GetHousingsByHighestRatingQuery(string userId) : IRequest<BaseResponse<List<HouseDTO>>>;

}
