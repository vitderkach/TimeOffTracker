using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;
using TOT.Interfaces;

namespace TOT.Business.Services
{
    public class SharedService : ISharedService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SharedService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddVacationDays(int employeeId, int days, int year, TimeOffType type, bool rewritePreviousStatutoryDays)
        {
            var vacation = _unitOfWork.VacationTypeRepository.GetOne(vt => vt.Year == year && vt.TimeOffType == type && vt.UserInformationId == employeeId);
            if (!(vacation is VacationType))
            {
                vacation = new VacationType() { TimeOffType = type, Year = year, UserInformationId = employeeId};
                _unitOfWork.VacationTypeRepository.Create(vacation);
                _unitOfWork.Save();
                vacation = _unitOfWork.VacationTypeRepository.GetOne(vt => vt.Year == year && vt.TimeOffType == type && vt.UserInformationId == employeeId);
            }
            if (rewritePreviousStatutoryDays)
            {
                vacation.StatutoryDays = days;
            }
            else
            {
                vacation.StatutoryDays += days;
            }

            _unitOfWork.VacationTypeRepository.Update(vacation, vt => vt.StatutoryDays);
            _unitOfWork.Save();
        }

        public void AddVacationRequest(int employeeId, DateTime from, DateTime to, TimeOffType type, int stage, bool? approved, int? takenDays, bool? excelFormat)
        {
            if (from.Year == to.Year)
            {
                VacationRequest vacationRequest;
                vacationRequest = _unitOfWork.VacationRequestRepository.GetOne(vr => vr.StartDate == from && vr.EndDate == to && vr.VacationType == type);
                if (!(vacationRequest is VacationRequest))
                {
                    vacationRequest = new VacationRequest() { StartDate = from, EndDate = to, VacationType = type, StageOfApproving = stage, Approval = approved, ExcelFormat = excelFormat,
                    UserInformationId = employeeId, TakenDays = takenDays };
                    _unitOfWork.VacationRequestRepository.Create(vacationRequest);
                    _unitOfWork.Save();
                }
                else
                {
                    vacationRequest.StageOfApproving = stage;
                    vacationRequest.Approval = approved;
                    vacationRequest.TakenDays = takenDays;
                    vacationRequest.ExcelFormat = excelFormat;
                    _unitOfWork.VacationRequestRepository.Update(vacationRequest, vr => vr.Approval, vr => vr.StageOfApproving, vr => vr.ExcelFormat, vr => vr.TakenDays);
                    _unitOfWork.Save();
                }
            }
            else
            {
                AddVacationRequest(employeeId, from, new DateTime(from.Year, 12, 31), type, stage, approved, takenDays, excelFormat);
                AddVacationRequest(employeeId, new DateTime(to.Year, 1, 1), to, type, stage, approved, takenDays, excelFormat);
            }
        }
        public void AddVacation(int employeeId, DateTime from, DateTime to, TimeOffType type, int fromTakenDays, int? toTakenDays)
        {
            if (from.Year == to.Year)
            {
                if (type == TimeOffType.PaidLeave)
                {
                    var giftVacation = _unitOfWork.VacationTypeRepository.GetOne(vt => vt.Year == from.Year && vt.TimeOffType == type && vt.UserInformationId == employeeId);
                    if (giftVacation.StatutoryDays > giftVacation.UsedDays)
                    {
                        int availableGiftDays = giftVacation.StatutoryDays - giftVacation.UsedDays;
                        int daysToTakeFromGift = (availableGiftDays > fromTakenDays) ? fromTakenDays : availableGiftDays;
                        giftVacation.UsedDays += daysToTakeFromGift;
                        fromTakenDays -= daysToTakeFromGift;
                        _unitOfWork.VacationTypeRepository.Update(giftVacation, vt => vt.UsedDays);
                    }
                    if (fromTakenDays > 0)
                    {
                        var paidVacation = _unitOfWork.VacationTypeRepository.GetOne(vt => vt.Year == from.Year && vt.TimeOffType == type && vt.UserInformationId == employeeId);
                        paidVacation.UsedDays += fromTakenDays;
                        _unitOfWork.VacationTypeRepository.Update(paidVacation, vt => vt.UsedDays);
                    }
                    _unitOfWork.Save();
                }
                else
                {
                    var vacation = _unitOfWork.VacationTypeRepository.GetOne(vt => vt.Year == from.Year && vt.TimeOffType == type && vt.UserInformationId == employeeId);
                    vacation.UsedDays += fromTakenDays;
                    _unitOfWork.VacationTypeRepository.Update(vacation, vt => vt.UsedDays);
                    _unitOfWork.Save();
                }
            }
            else
            {
                AddVacation(employeeId, from, new DateTime(from.Year, 12, 31), type, fromTakenDays, null);
                AddVacation(employeeId, new DateTime(to.Year, 1, 1), to, type, toTakenDays.Value, null);
            }

        }

    }
}
