using System;
using System.Collections.Generic;
using System.Text;
using TOT.DataImport.Interfaces;
using TOT.Entities;

namespace TOT.DataImport.Excel
{
    class AttendanceTableConfiguration
    {
        public Dictionary<string, TimeOffType> cellValueTimeOffTypePairs { get; protected set; } = new Dictionary<string, TimeOffType>
        {
            { "о", TimeOffType.Vacation },
            { "б", TimeOffType.SickLeave },
            { "у", TimeOffType.StudyLeave },
            { "н", TimeOffType.UnpaidVacation },
            // TODO: { "а", TimeOffType.Administrative },
        };
    }
}
