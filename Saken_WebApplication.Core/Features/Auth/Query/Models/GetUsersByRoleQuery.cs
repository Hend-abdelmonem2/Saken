using MediatR;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Response;

namespace Saken_WebApplication.Core.Features.Auth.Query.Models
{
    public record GetUsersByRoleQuery(string userId, string Role) : IRequest<BaseResponse<IEnumerable<UserDto>>>;

}
