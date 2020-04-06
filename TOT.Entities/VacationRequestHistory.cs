using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Entities
{
    public class VacationRequestHistory: BaseVacationRequest
    {
        public DateTime SystemStart { get; set; }
        public DateTime SystemEnd { get; set; }
    }
}
