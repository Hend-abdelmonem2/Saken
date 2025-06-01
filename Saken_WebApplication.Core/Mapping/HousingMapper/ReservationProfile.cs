using AutoMapper;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Core.Mapping.HousingMapper
{
   public class ReservationProfile:Profile
    {
        public ReservationProfile()
        {
            CreateMap<ReservationDto, Reservation>()
                .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => ReservationStatus.Pending))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.LandlordId, opt => opt.Ignore());
        }
    }
}
