using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Entities
{
    public class VacationDay
    {
        public int Id { get; set; }

        public int DefaultVacationCount { get; set; }

        public int GiftVacationCount { get; set; }

        public ICollection<UserInformationVacationDay> UserInformationVacationDays { get; set; }
    }
}
