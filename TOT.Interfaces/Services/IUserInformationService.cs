﻿using System;
using System.Collections.Generic;
using System.Text;
using TOT.Dto;

namespace TOT.Interfaces.Services
{
    public interface IUserInformationService
    {
        UserInformationDTO getUserInformation(int id);
    }
}
