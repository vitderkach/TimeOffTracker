using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TOT.Entities;

namespace TOT.Dto
{
    public class ApplyForRequestPostDto
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public string SelectedTimeOffType { get; set; }
        public string Notes { get; set; }
        [Required]
        [Display(Name = "Manager")]
        public int SelectedManager { get; set; }
        public IEnumerable<SelectListItem> MyManagers { get; set; }
        public ICollection<ManagerResponseDto> ManagersResponses { get; set; }

        public int UserId { get; set; }
    }
}
