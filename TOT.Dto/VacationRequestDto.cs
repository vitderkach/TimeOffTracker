using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TOT.Entities;

namespace TOT.Dto
{
    public class VacationRequestDto
    {
        public int VacationRequestId { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public TimeOffType VacationType { get; set; }
        public string Notes { get; set; }
        [ScaffoldColumn(false)]
        public IList<int> SelectedManager { get; set; }
        public ICollection<ManagerResponseDto> ManagersResponses { get; set; }
        public bool Approval { get; set; }
        public ApplicationUser User { get; set; }
        public int UserId { get; set; }
        public ApplicationUserDto User { get; set; }
    }
}
