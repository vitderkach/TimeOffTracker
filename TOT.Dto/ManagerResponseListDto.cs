using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Dto
{
    public class ManagerResponseListDto
    {
        public int Id { get; set; }
        public string Notes { get; set; }
        public DateTime DateResponse { get; set; }
        public bool? Approval { get; set; }
        public bool isRequested { get; set; }

        public VacationRequestDto VacationRequest { get; set; }
        public ApplicationUserDto Manager { get; set; }
    }
}
