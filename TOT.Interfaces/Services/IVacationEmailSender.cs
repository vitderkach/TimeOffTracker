using System;
using System.Collections.Generic;
using System.Text;
using TOT.Dto;

namespace TOT.Interfaces.Services {
    public interface IVacationEmailSender {
        void ExecuteToManager(EmailModel model);
        void ExecuteToEmployeeAccept(EmailModel model);
        void ExecuteToEmployeeDecline(EmailModel model);
    }
}
