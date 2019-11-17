using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TOT.Dto;

namespace TOT.Web.Models {
    public class VacationListViewModel {
        public IEnumerable<VacationRequestListDto> Vacations { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
