using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TOT.Entities
{
    public class VacationRequest
    {
        public VacationRequest()
        {
            ManagersResponses = new Collection<ManagerResponse>();
        }
        public int VacationRequestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreationDate { get; set; }
        public TimeOffType VacationType { get; set; }
        public string Notes { get; set; }
        public bool? Approval { get; set; }

        public ICollection<ManagerResponse> ManagersResponses { get; set; }

        public int? UserId { get; set; }
        public ApplicationUser User { get; set; }
    }

    public enum TimeOffType
    {
        [Description("Sick Leave")]
        SickLeave = 1,
        [Description("Study Leave")]
        StudyLeave = 2,
        [Description("Unpaid vacation")]
        UnpaidVacation = 3,
        [Description("Vacation")]
        Vacation = 4
    }
}
