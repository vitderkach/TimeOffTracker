using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Entities
{
    public class VacationType
    {
        public int Id { get; set; }

        public int UserInformationId { get; set; }
        public UserInformation UserInformation { get; set; }

        public TimeOffType TimeOffType { get; set; }

        public int Year { get; set; }

        public int StatutoryDays { get; set; }

        public int UsedDays { get; set; }
    }
}
