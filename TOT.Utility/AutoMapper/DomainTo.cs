using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TOT.Dto;
using TOT.Entities;

namespace TOT.Utility.AutoMapper
{
    public class DomainTo : Profile
    {
        public DomainTo()
        {
            CreateMap<VacationRequest, VacationRequestDto>()
                .ForMember(dest => dest.VacationTypeString, opt => opt.MapFrom(src => src.VacationType.GetDescription()));
            CreateMap<VacationRequest, ApplyForRequestGetDto>()
                .ForMember(dest => dest.VacationTypes, opt => opt.MapFrom(src => src.VacationType.GetDescription()));

            CreateMap<ManagerResponse, ManagerResponseDto>();
            CreateMap<VacationRequestListDto, VacationRequest>();
            CreateMap<VacationRequest, VacationRequestListDto>()
                .ForMember(dest => dest.VacationType, opt => opt.MapFrom(src => src.VacationType.GetDescription()));

            CreateMap<VacationRequestApprovalDto, VacationRequestDto>()
                 .ForMember(dest => dest.VacationType, opt => opt.MapFrom(src => src.VacationType.Replace(" ", "")));

            CreateMap<ManagerResponseDto, ManagerResponseListDto>()
                .ForMember(dest => dest.VacationRequestId, opt => opt.MapFrom(src => src.VacationRequest.VacationRequestId))
                .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.VacationRequest.User.FullName))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.VacationRequest.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.VacationRequest.EndDate))
                .ForMember(dest => dest.VacationType, opt => opt.MapFrom(src => src.VacationRequest.VacationType.GetDescription()))
                .ForMember(dest => dest.Approval, opt => opt.MapFrom(src => src.Approval));

            CreateMap<ManagerResponseDto, VacationRequestApprovalDto>()
                .ForMember(dest => dest.VacationRequestId, opt => opt.MapFrom(src => src.VacationRequest.VacationRequestId))
                .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.VacationRequest.User.FullName))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.VacationRequest.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.VacationRequest.EndDate))
                .ForMember(dest => dest.VacationType, opt => opt.MapFrom(src => src.VacationRequest.VacationType.GetDescription()))
                .ForMember(dest => dest.EmployeeNotes, opt => opt.MapFrom(src => src.VacationRequest.Notes))
                .ForMember(dest => dest.ManagerResponseId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.VacationRequest.UserId))
                .ForMember(dest => dest.isApproval, opt => opt.MapFrom(src => src.Approval));

            CreateMap<ApplicationUser, UserInformationDto>().
                ForSourceMember(src => src.Id, opt => opt.DoNotValidate());

            CreateMap<UserInformation, UserInformationDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApplicationUserId));

            CreateMap<ApplicationUser, UserInformationListDto>().
    ForSourceMember(src => src.Id, opt => opt.DoNotValidate());

            CreateMap<UserInformation, UserInformationListDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApplicationUserId));

            CreateMap<VacationType, VacationTypeDto>();
            CreateMap<ICollection<VacationType>, VacationDaysDto>()
                .ForMember(dest => dest.TimeOffTypes, opt => opt.MapFrom(src => src));
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
