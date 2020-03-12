using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TOT.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public override int Id { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public int UserInformationId { get; set; }
        public UserInformation UserInformation { get; set; }
        public ICollection<VacationRequest> VacationRequests { get; set; }

        //public ICollection<ManagerResponse> ManagerResponses { get; set; }
    }
}
