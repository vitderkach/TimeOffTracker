using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Utility.AutoMapper
{
    public class DTOTo: Profile
    {
        public DTOTo()
        {
            CreateMap<VacationRequestDto, VacationRequest>();
            CreateMap<ApplicationUserDto, ApplicationUser>();
            CreateMap<UserInformationDto, UserInformation>();
            CreateMap<ManagerResponseDto, ManagerResponse>();
            CreateMap<ApplyForRequestGetDto, VacationRequestDto>()
                .ForMember(dest => dest.VacationType, opt => opt.MapFrom(
                    src => src.SelectedTimeOffType.ToEnum<TimeOffType>()));
        }
    }
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
