using AutoMapper;
using AutoMapper.EquivalencyExpression;
using System;
using System.Collections.Generic;
using System.Text;
using TOT.Dto;
using TOT.Entities;
namespace TOT.Utility.AutoMapper
{
    public class DTOTo : Profile
    {
        public DTOTo()
        {
            CreateMap<VacationRequestDto, VacationRequest>();


            CreateMap<UserInformationDto, ApplicationUser>();

            CreateMap<UserInformationDto, UserInformation>()
                .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ManagerResponseDto, ManagerResponse>();
            CreateMap<ApplicationDto, VacationRequest>()
            .ForMember(dest => dest.UserInformationId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Approval, opt => opt.Ignore())
            .ForMember(dest => dest.StageOfApproving, opt => opt.Ignore())
            .ForMember(dest => dest.UserInformationId, opt => opt.MapFrom(src => src.UserId))
            .EqualityComparison((odto, o) => odto.Id == o.VacationRequestId);
            CreateMap<VacationRequestListDto, VacationRequest>();

            CreateMap<UserInformationListDto, ApplicationUser>();
            CreateMap<UserInformationListDto, UserInformation>()
                .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.Id));

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
