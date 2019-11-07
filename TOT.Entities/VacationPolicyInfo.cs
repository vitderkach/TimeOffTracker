using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TOT.Entities {
    public class VacationPolicyInfo {
        public VacationPolicyInfo()
        {
            TimeOffTypes = new Collection<VacationType>();

            var values = Enum.GetValues(typeof(TimeOffType)).Cast<TimeOffType>();
            for(int i = 0; i < values.Count(); i++)
            {
                TimeOffTypes.Add(new VacationType() { TimeOffType = values.ElementAt(i), WastedDays = 0 });
            }
        }
        public int Id { get; set; }
        
        public ICollection<VacationType> TimeOffTypes { get; set; }
        public int UserInformationId { get; set; }
        public UserInformation UserInformation { get; set; }
    }
}
