using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;

namespace TOT.Dto
{
    public class VacationRequestDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreationDate { get; set; }
        public TimeOffType VacationType { get; set; }
        public string Notes { get; set; }

        public ICollection<ManagerResponseDto> ManagersResponses { get; set; }

        public int UserId { get; set; }
    }
}
