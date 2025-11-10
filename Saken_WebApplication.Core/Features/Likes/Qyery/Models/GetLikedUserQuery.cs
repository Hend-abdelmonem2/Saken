using MediatR;
using Saken_WebApplication.Data.DTO.Favorite;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Likes.Qyery.Models
{
    public record GetLikedUsersQuery : IRequest<BaseResponse<List<UserLikeDto>>>;

}
