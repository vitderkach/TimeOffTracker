using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TOT.Entities
{
    public class UserInformation
    {
        public int UserInformationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser User { get; set; }
    }
}
