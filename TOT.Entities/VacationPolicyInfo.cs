using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TOT.Entities {
    public class VacationPolicyInfo {
        public VacationPolicyInfo()
        {
            
        }
        public int Id { get; set; }
        
        public ICollection<VacationType> TimeOffTypes { get; set; }
        public int UserInformationId { get; set; }
        public UserInformation UserInformation { get; set; }
    }
}
