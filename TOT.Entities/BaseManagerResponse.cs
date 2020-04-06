using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TOT.Entities
{
    public abstract class BaseManagerResponse
    {
        public int Id { get; set; }
        public string Notes { get; set; }
        public DateTime DateResponse { get; set; }
        public bool? Approval { get; set; }
        [Range(1, 4)]
        public int ForStageOfApproving { get; set; }
        public int VacationRequestId { get; set; }
        public VacationRequest VacationRequest { get; set; }

        public int ManagerId { get; set; }
        public UserInformation Manager { get; set; }
    }
}
