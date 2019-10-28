using System;
using System.Collections.Generic;


namespace TOT.Entities
{
    public class VacationRequest
    {
        public int VacationRequestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreationDate { get; set; }
        public TimeOffType VacationType { get; set; }
        public string Notes { get; set; }

        public ICollection<ManagerResponse> ManagersResponses { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }

    public enum TimeOffType
    {
        SickLeave = 1,
        StudyLeave = 2,
        UnpaidVacation = 3,
        Vacation = 4
    }
}
