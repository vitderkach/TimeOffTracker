using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TOT.Entities;

namespace TOT.Dto
{
    public class UserInformationDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Registered")]
        [JsonIgnore]
        public DateTime RecruitmentDate { get; set; }

        public string Email { get; set; }

        public Location Location { get; set; }

        public Team Team { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => FirstName + " " + LastName;

    }
}
