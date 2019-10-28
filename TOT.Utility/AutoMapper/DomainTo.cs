using AutoMapper;
using System;
using System.Collections.Generic;
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
            CreateMap<ApplicationUser, ApplicationUserDto>();
            CreateMap<UserInformation, UserInformationDto>();
            CreateMap<ManagerResponse, ManagerResponseDto>();

        }
    }
}
