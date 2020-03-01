using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Entities
{
    public class UserInformationVacationDay
    {
        public int UserInformationId { get; set; }
        public UserInformation UserInformation { get; set; }

        public int VacationDayId { get; set; }
        public VacationDay VacationDay { get; set; }
    }
}
