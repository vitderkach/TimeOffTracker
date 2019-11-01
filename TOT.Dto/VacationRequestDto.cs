﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TOT.Entities;

namespace TOT.Dto
{
    public class VacationRequestDto
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public TimeOffType VacationType { get; set; }
        public string Notes { get; set; }
        [ScaffoldColumn(false)]
        public int SelectedManager { get; set; }
        public ICollection<ManagerResponseDto> ManagersResponses { get; set; }

        public int UserId { get; set; }
    }
}
