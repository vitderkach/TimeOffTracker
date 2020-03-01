using System;
using System.Collections.Generic;

namespace TOT.Entities
{
    public class UserInformation
    {
        public int UserInformationId { get; set; }
        public ApplicationUser User { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime RecruitmentDate { get; set; }

        public bool IsFired { get; set; }

        public VacationPolicyInfo VacationPolicyInfo { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public ICollection<UserInformationVacationDay> UserInformationVacationDays { get; set; }
    }
}
