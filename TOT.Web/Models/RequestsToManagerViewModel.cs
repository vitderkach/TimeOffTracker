﻿using System;
using System.ComponentModel.DataAnnotations;
using TOT.Entities;

namespace TOT.Web.Models
{
    public class RequestsToManagerViewModel
    {
        public int VacationRequestId { get; set; }

        public string Employee { get; set; }

        [Display(Name = "Time off reason")]
        public TimeOffType VacationType { get; set; }

        [Display(Name = "From")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}