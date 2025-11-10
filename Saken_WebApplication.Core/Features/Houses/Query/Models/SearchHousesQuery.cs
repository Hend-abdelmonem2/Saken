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
    public class SearchHousesQuery : IRequest<BaseResponse<IEnumerable<HouseDTO>>>
    {
        public string SearchKey { get; }
        public SearchHousesQuery(string searchKey) => SearchKey = searchKey;
    }
}
