using MediatR;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Response;

namespace Saken_WebApplication.Core.Features.Auth.Query.Models
{
    public record GetUsersQuery(string userId) : IRequest<BaseResponse<IEnumerable<UserDto>>>;

}
