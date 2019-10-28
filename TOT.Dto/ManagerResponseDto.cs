using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Dto
{
    public class ManagerResponseDto
    {
        public int Id { get; set; }
        public string Notes { get; set; }
        public DateTime DateResponse { get; set; }

        public int VacationRequestId { get; set; }
        public VacationRequestDto VacationRequest { get; set; }

        public int ManagerId { get; set; }
        public ApplicationUserDto Manager { get; set; }

    }
}
