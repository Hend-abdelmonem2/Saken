using AutoMapper;
using Humanizer;
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
    public class HousingProfile :Profile 

    {
        public HousingProfile()
        {
            CreateMap<HousingDto, Housing>()
                .ForMember(dest => dest.HousingType, opt => opt.MapFrom(src => Enum.Parse<PropertyType>(src.HousingType, true)))
                .ForMember(dest => dest.FurnishingStatus, opt => opt.MapFrom(src => Enum.Parse<FurnishingStatus>(src.FurnishingStatus, true)))
                .ForMember(dest => dest.TargetTenantType, opt => opt.MapFrom(src => Enum.Parse<TargetCustomerType>(src.TargetTenantType, true)))
                .ForMember(dest => dest.RentdurationUnit, opt => opt.MapFrom(src => Enum.Parse<RentDurationUnit>(src.RentDurationUnit, true)))
                .ForMember(dest => dest.RentalType, opt => opt.MapFrom(src => Enum.Parse<RentalType>(src.RentalType, true)))
                .ForMember(dest => dest.PhotoUrl, opt => opt.Ignore()) // لأنك بتعالجي الصورة يدويًا
                                                                       // .ForMember(dest => dest.rating, opt => opt.Ignore()) // بنحطها 0 يدوي
    .ForMember(dest => dest.IsFrozen, opt => opt.Ignore()) // لأنك مش بتستلميها من الـ DTO
    .ForMember(dest => dest.Owner, opt => opt.Ignore())
    .ForMember(dest => dest.Reservations, opt => opt.Ignore())
    .ForMember(dest => dest.Reviews, opt => opt.Ignore());// سيتم تعيينه من البراميتر
        }

    }
}
