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
                .ForMember(dest => dest.type, opt => opt.MapFrom(src => Enum.Parse<PropertyType>(src.Type, true)))
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => Enum.Parse<HousingStatus>(src.Status, true)))
                .ForMember(dest => dest.furnishingStatus, opt => opt.MapFrom(src => Enum.Parse<FurnishingStatus>(src.FurnishingStatus, true)))
                .ForMember(dest => dest.targetCustomers, opt => opt.MapFrom(src => Enum.Parse<TargetCustomerType>(src.TargetCustomers, true)))
                .ForMember(dest => dest.rentalPeriod, opt => opt.MapFrom(src => Enum.Parse<RentalDuration>(src.RentalPeriod, true)))
                .ForMember(dest => dest.RentalType, opt => opt.MapFrom(src => Enum.Parse<RentalType>(src.RentalType, true)))
                .ForMember(dest => dest.Photo, opt => opt.Ignore()) // لأنك بتعالجي الصورة يدويًا
    .ForMember(dest => dest.rating, opt => opt.Ignore()) // بنحطها 0 يدوي
    .ForMember(dest => dest.IsFrozen, opt => opt.Ignore()) // لأنك مش بتستلميها من الـ DTO
    .ForMember(dest => dest.Landlord, opt => opt.Ignore())
    .ForMember(dest => dest.Reservations, opt => opt.Ignore())
    .ForMember(dest => dest.Reviews, opt => opt.Ignore());// سيتم تعيينه من البراميتر
        }
     
    }
}
