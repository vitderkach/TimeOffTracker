using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using TOT.Entities;

namespace TOT.Dto
{
    public class ApplicationDto
    {
        public ApplicationDto()
        {
            RequiredManagersEmails = new List<string>();
        }
        public int Id { get; set; }

        [Display(Name = "Vacation type")]
        [Required(ErrorMessage = "Required")]
        public TimeOffType VacationType { get; set; }

        [Display(Name = "From")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Required")]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Required")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Vacation types")]
        public string SelectedTimeOffType { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "Managers")]
        [Required(ErrorMessage = "Required")]
        public IEnumerable<string> RequiredManagersEmails { get; set; }
        public int UserId { get; set; }
    }
}
