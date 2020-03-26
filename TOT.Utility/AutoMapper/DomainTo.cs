﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TOT.Dto;
using TOT.Entities;
using AutoMapper.EquivalencyExpression;
namespace TOT.Utility.AutoMapper
{
    public class DomainTo : Profile
    {
        public DomainTo()
        {
            CreateMap<VacationRequest, VacationRequestDto>()
                .ForMember(dest => dest.ApplicationDto, opt => opt.MapFrom(source => source))
                .ForMember(dest => dest.Stage, opt => opt.MapFrom(source => source.StageOfApproving))
                .ForMember(dest => dest.User, opt => opt.MapFrom(source => source.UserInformation))
                .ForMember(dest => dest.AllManagerResponses, opt => opt.MapFrom(source => source.ManagersResponses));





            CreateMap<VacationRequest, ApplicationDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserInformationId))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.VacationRequestId));
            CreateMap<VacationRequest, VacationRequestListDto>();

            CreateMap<ManagerResponse, ManagerResponseDto>();
            CreateMap<ManagerResponse, ManagerResponseListDto>()
                .ForMember(dest => dest.ResponseDate, opt => opt.MapFrom(src => src.DateResponse))
                .ForMember(dest => dest.FullName, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore());


            CreateMap<VacationRequest, VacationRequestsListForAdminsDTO>();
            CreateMap<VacationRequest, VacationRequestListForManagersDTO>();
            //CreateMap<ManagerResponseDto, ManagerResponseListDto>()
            //    .ForMember(dest => dest.VacationRequestId, opt => opt.MapFrom(src => src.VacationRequest.VacationRequestId))
            //    .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.VacationRequest.User.FullName))
            //    .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.VacationRequest.StartDate))
            //    .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.VacationRequest.EndDate))
            //    .ForMember(dest => dest.VacationType, opt => opt.MapFrom(src => src.VacationRequest.VacationType.GetDescription()))
            //    .ForMember(dest => dest.Approval, opt => opt.MapFrom(src => src.Approval));

            //CreateMap<ManagerResponseDto, VacationRequestApprovalDto>()
            //    .ForMember(dest => dest.VacationRequestId, opt => opt.MapFrom(src => src.VacationRequest.VacationRequestId))
            //    .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.VacationRequest.User.FullName))
            //    .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.VacationRequest.StartDate))
            //    .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.VacationRequest.EndDate))
            //    .ForMember(dest => dest.VacationType, opt => opt.MapFrom(src => src.VacationRequest.VacationType.GetDescription()))
            //    .ForMember(dest => dest.EmployeeNotes, opt => opt.MapFrom(src => src.VacationRequest.Notes))
            //    .ForMember(dest => dest.ManagerResponseId, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.VacationRequest.UserId))
            //    .ForMember(dest => dest.isApproval, opt => opt.MapFrom(src => src.Approval));

            CreateMap<ApplicationUser, UserInformationDto>()
                .ForMember(dest => dest.FirstName, opt => opt.Ignore())
                .ForMember(dest => dest.LastName, opt => opt.Ignore())
                .ForMember(dest => dest.RecruitmentDate, opt => opt.Ignore())
                .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<UserInformation, UserInformationDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApplicationUserId))
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .EqualityComparison((odto, o) => odto.ApplicationUserId == o.Id);


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
