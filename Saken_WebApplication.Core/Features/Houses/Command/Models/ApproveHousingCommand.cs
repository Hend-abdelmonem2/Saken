using MediatR;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Houses.Command.Models
{
    public class ApproveHousingCommand : IRequest<BaseResponse<bool>>
    {
        public int HousingId { get; }
        public ApproveHousingCommand(int housingId) => HousingId = housingId;
    }
}
