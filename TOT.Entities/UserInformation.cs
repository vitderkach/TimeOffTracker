using System;
using System.Collections.Generic;

namespace TOT.Entities
{
    public class UserInformation
    {
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? RecruitmentDate { get; set; }

        public bool IsFired { get; set; }

        public int? LocationId { get; set; }
        public Location Location { get; set; }

        public int? TeamId { get; set; }
        public Team Team { get; set; }

        public ICollection<VacationPolicy> VacationPolicies { get; set; }
    }
}
