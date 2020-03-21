using System;
using System.Collections.Generic;
using System.Text;
using TOT.DataImport.Interfaces;
using TOT.Entities;

namespace TOT.DataImport.Excel
{
    class AttendanceTableConfiguration
    {
        public Dictionary<string, TimeOffType> CellValueTimeOffTypePairs { get; protected set; } = new Dictionary<string, TimeOffType>
        {
            { "о", TimeOffType.PaidLeave },
            { "б", TimeOffType.ConfirmedSickLeave },
            { "у", TimeOffType.StudyLeave },
            { "н", TimeOffType.UnofficialSickLeave },
            { "а", TimeOffType.AdministrativeLeave },
        };
    }
}
