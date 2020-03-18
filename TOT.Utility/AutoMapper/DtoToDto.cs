using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TOT.Dto;
namespace TOT.Utility.AutoMapper
{
    public class DtoToDto: Profile
    {
        public DtoToDto()
        {
            CreateMap<UserInformationDto, UserInformationListDto>();
        }
        
    }
}
