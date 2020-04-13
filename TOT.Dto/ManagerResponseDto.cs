using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TOT.Dto
{
    public class ManagerResponseDto
    {
        public int Id { get; set; }
        public int VacationRequestId { get; set; }
        [Display(Name = "Manager notes")]
        public string Notes { get; set; }
        [Display(Name = "Date response")]
        public DateTime DateResponse { get; set; }
    }
}
