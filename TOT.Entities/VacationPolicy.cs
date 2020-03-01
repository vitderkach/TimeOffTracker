using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TOT.Entities
{
    public class VacationPolicy
    {
        public int UserInformationId { get; set; }
        public UserInformation UserInformation { get; set; }

        public int Year { get; set; }

        public ICollection<VacationType> VacationTypes { get; set; }

    }
}
