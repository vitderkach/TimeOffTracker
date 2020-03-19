using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TOT.Dto
{
    public class ManagerResponseDto
    {
        public int Id { get; set; }
        [Display(Name = "Manager notes")]
        public string Notes { get; set; }
        [Display(Name = "Date response")]
        public DateTime DateResponse { get; set; }
        [Display(Name = "Status")]
        public bool? Approval { get; set; }
        public bool isRequested { get; set; }

        public int VacationRequestDtoId { get; set; }
        public VacationRequestDto VacationRequest { get; set; }

        public int ManagerId { get; set; }
        public UserInformationDto Manager { get; set; }
    }
}
