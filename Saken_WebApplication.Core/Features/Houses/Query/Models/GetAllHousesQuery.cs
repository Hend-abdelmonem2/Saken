using MediatR;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;

namespace Saken_WebApplication.Core.Features.Houses.Query.Models
{
    public record GetAllHousesQuery(string userId) : IRequest<BaseResponse<IEnumerable<HouseDTO>>>;

}
