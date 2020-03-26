using System;
using System.ComponentModel.DataAnnotations;

namespace TOT.Entities
{
    public class ManagerResponse
    {
        public ManagerResponse()
        {
            isRequested = true;
        }
        public int Id { get; set; }
        public string Notes { get; set; }
        public DateTime DateResponse { get; set; }
        public bool? Approval { get; set; }
        public bool isRequested { get; set; }
        [Range(1, 4)]
        public int ForStageOfApproving { get; set; }
        public int VacationRequestId { get; set; }
        public VacationRequest VacationRequest { get; set; }

        public int ManagerId { get; set; }
        public UserInformation Manager { get; set; }
    }
}
