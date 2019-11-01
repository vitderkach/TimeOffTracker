using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Dto
{
    public class ApplicationUserDto
    {
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int UserInformationId { get; set; }
        public UserInformationDto UserInformation { get; set; }
        public ICollection<VacationRequestDto> VacationRequests { get; set; }
        public ICollection<ManagerResponseDto> ManagerResponses { get; set; }
    }
}
