using System;

namespace TOT.Entities
{
    public class ManagerResponse
    { 
        public int Id { get; set; }
        public string Notes { get; set; }
        public DateTime DateResponse { get; set; }

        public int VacationRequestId { get; set; }
        public  VacationRequest VacationRequest { get; set; }
                                                              
        public int ManagerId { get; set; }
        public ApplicationUser Manager { get; set; }
    }
}
