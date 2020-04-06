using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Entities
{
    public class ManagerResponseHistory: BaseManagerResponse
    {
        public DateTime SystemStart { get; set; }
        public DateTime SystemEnd { get; set; }
    }
}
