using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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

        public int ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser User { get; set; }
    }
    public enum TimeOffType
    {
        SickLeave = 1,
        StudyLeave = 2,
        UnpaidVacation = 3,
        Vacation = 4
    }
}
