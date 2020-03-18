using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Entities
{
    public class Location
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<UserInformation> UserInformations { get; set; }
    }
}
