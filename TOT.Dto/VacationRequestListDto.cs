using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TOT.Entities;

namespace TOT.Dto {

    public class VacationRequestListDto {
        public int VacationRequestId { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public TimeOffType VacationType { get; set; }

        public ApplicationUserDto User { get; set; }

        public ICollection<ManagerResponseDto> ManagersResponses { get; set; }
        public string Notes { get; set; }
    }
}
