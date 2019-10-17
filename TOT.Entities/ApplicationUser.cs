using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TOT.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public DateTime RegistrationDate { get; set; }
        public int UserInformationId { get; set; }
        [ForeignKey("UserInformationId")]
        public UserInformation UserInformation { get; set; }
        public ICollection<VacationRequest> VacationRequests { get; set; }
    }
}
