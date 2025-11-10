using MediatR;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.recommend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Recommendation.Query.Models
{
    public class GetRecommendedHousesQuery : IRequest<BaseResponse<IEnumerable<HousingRecommendationDto>>>
    {
    }
}
