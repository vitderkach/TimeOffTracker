using AutoMapper;
using AutoMapper.EquivalencyExpression;
using System;
using System.Collections.Generic;
using System.Text;
using TOT.Dto;
namespace TOT.Utility.AutoMapper
{
    public class DtoToDto : Profile
    {
        public DtoToDto()
        {
            CreateMap<UserInformationDto, UserInformationListDto>();

            CreateMap<ManagerResponseDto, VacationDetailsDTO>()
                .ForMember(dest => dest.ManagerResponseDto, options => options.MapFrom(source => source));

            CreateMap<VacationRequestDto, VacationDetailsDTO>()
                .ForMember(dest => dest.VacationRequestDto, options => options.MapFrom(source => source));

            CreateMap<UserInformationDto, VacationRequestDto>()
                .ForMember(dest => dest.User, options => options.MapFrom(source => source));

            CreateMap<UserInformationDto, ManagerResponseListDto>()
                .ForMember(dest => dest.Approval, options => options.Ignore())
                .ForMember(dest => dest.ManagerId, options => options.Ignore())
                .ForMember(dest => dest.Notes, options => options.Ignore())
                .ForMember(dest => dest.ResponseDate, options => options.Ignore())
                .EqualityComparison((odto, o) => odto.Id == o.ManagerId);

            CreateMap<UserInformationDto, VacationRequestsListForAdminsDTO>()
                .EqualityComparison((odto, o) => odto.Id == o.UserInformationId);

            CreateMap<UserInformationDto, VacationRequestListForManagersDTO>()
    .EqualityComparison((odto, o) => odto.Id == o.UserInformationId);
        }

    }
}
