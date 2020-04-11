using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Dto
{
    public class EditVacationDaysDto
    {
        public VacationTypeDto PaidLeave { get; set; }

        public VacationTypeDto GiftLeave { get; set; }

        public VacationTypeDto SickLeave { get; set; }

        public VacationTypeDto StudyLeave { get; set; }

    }
}
