using MediatR;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Houses.Command.Models
{
    public class RejectHousingCommand : IRequest<BaseResponse<string>>
    {
        public int HousingId { get; }
        public string Reason { get; }
        public RejectHousingCommand(int housingId, string reason)
        {
            HousingId = housingId;
            Reason = reason;
        }
    }
    }
