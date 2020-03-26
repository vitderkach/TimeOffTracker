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
        [Range(1, 4)]
        public int StageOfApproving { get; set; }

        public IEnumerable<ManagerResponse> ManagersResponses { get; set; }

        public int UserInformationId { get; set; }
        public UserInformation UserInformation { get; set; }
    }

    public enum TimeOffType
    {
        [Description("Confirmed Sick Leave")]
        ConfirmedSickLeave,

        [Description("Unofficial Sick Leave")]
        UnofficialSickLeave,

        [Description("Study Leave")]
        StudyLeave,

        [Description("Paid Leave")]
        PaidLeave,

        [Description("Gift Leave")]
        GiftLeave,

        [Description("Administrative Leave")]
        AdministrativeLeave
    }
}
