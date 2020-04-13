using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TOT.Entities
{
    public abstract class BaseVacationRequest
    {
        public BaseVacationRequest()
        {
            ManagersResponses = new List<ManagerResponse>();
        }
        public int VacationRequestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeOffType VacationType { get; set; }
        public string Notes { get; set; }
        public bool? Approval { get; set; }
        [Range(1, 4)]
        public int StageOfApproving { get; set; }
        public int? TakenDays { get; set; }

        public bool? SelfCancelled { get; set; }

        public IEnumerable<ManagerResponse> ManagersResponses { get; set; }

        public int UserInformationId { get; set; }
        public UserInformation UserInformation { get; set; }
    }
}
