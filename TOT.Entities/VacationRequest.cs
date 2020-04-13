using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TOT.Entities
{
    public class VacationRequest: BaseVacationRequest
    {

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
