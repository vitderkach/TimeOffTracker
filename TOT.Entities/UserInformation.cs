using System;

namespace TOT.Entities
{
    public class UserInformation
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
