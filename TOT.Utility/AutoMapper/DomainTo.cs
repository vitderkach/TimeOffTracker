using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TOT.Dto;
using TOT.Entities;

namespace TOT.Utility.AutoMapper
{
    public class DomainTo: Profile
    {
        public DomainTo()
        {
            CreateMap<VacationRequest, VacationRequestDto>();
            CreateMap<VacationRequest, ApplyForRequestGetDto>()
                .ForMember(dest => dest.VacationTypes, opt => opt.MapFrom(src => src.VacationType.GetDescription()));

            CreateMap<ApplicationUser, ApplicationUserDto>();
            CreateMap<UserInformation, UserInformationDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.LastName} {src.FirstName}"))
                ;
            CreateMap<ManagerResponse, ManagerResponseDto>();
            CreateMap<VacationRequestListDto, VacationRequest>();

        }
    }
    public static class ExtensionMethods
    {
        static public string GetDescription(this TimeOffType This)
        {
            var type = typeof(TimeOffType);
            var memInfo = type.GetMember(This.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}
