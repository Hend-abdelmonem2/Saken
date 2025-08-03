using MediatR;
using Saken_WebApplication.Data.DTO.HousingDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Houses.Command.Models
{
    public  class UpdateHousingCommand :IRequest<Unit>
    {
        public int Id { get; set; }
        public HousingDto Dto { get; set; }

        public UpdateHousingCommand(int id, HousingDto dto)
        {
            Id = id;
            Dto = dto;
        }

    }
}
