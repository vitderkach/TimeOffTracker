using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TOT.Entities
{
    public class ManagerResponse
    { 
        public int Id { get; set; }
        public string Notes { get; set; }
        public DateTime DateResponse { get; set; }

        public int? VacationRequestId { get; set; }
        [ForeignKey("VacationRequestId")]
        public virtual VacationRequest VacationRequest { get; set; }
                                                              
        public int? ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public virtual ApplicationUser Manager { get; set; }
    }
}
