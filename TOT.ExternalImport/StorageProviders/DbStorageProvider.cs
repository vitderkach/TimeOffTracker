using System;
using TOT.DataImport.Interfaces;
using TOT.Entities;
using TOT.Interfaces;
using System.Text.RegularExpressions;
using TOT.DataImport.Exceptions;
namespace TOT.DataImport.StorageProviders
{
    public class DbStorageProvider : IStorageProvider
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISharedService _sharedService;

        public DbStorageProvider(IUnitOfWork unitOfWork, ISharedService sharedService)
        {
            _unitOfWork = unitOfWork;
            _sharedService = sharedService;
        }

        public IEmployeeStorageProvider AddEmployeeAndRewriteHimTeamAndWorkplace(string name, DateTime? employmentDate, bool? isFired, string teamName, string workplace)
        {
            name = Regex.Replace(name, " +", " ");
            UserInformation user = null;
            if (employmentDate != null)
            {
                if (name.Split(' ').Length == 1)
                {
                    user = _unitOfWork.UserInformationRepository
                        .GetOne(ui => ui.RecruitmentDate.Value == employmentDate.Value && ui.FirstName + ui.LastName == name, ui => ui.Team, ui => ui.Location);
                }
                else
                {
                    user = _unitOfWork.UserInformationRepository
                        .GetOne(ui => ui.RecruitmentDate.Value == employmentDate.Value && ui.FirstName + " " + ui.LastName == name, ui => ui.Team, ui => ui.Location);
                }

            }
            else if (isFired != null)
            {
                if (name.Split(' ').Length == 1)
                {
                    user = _unitOfWork.UserInformationRepository
                    .GetOne(ui => ui.IsFired == isFired.Value && ui.FirstName + ui.LastName == name, ui => ui.Team, ui => ui.Location);
                }
                else
                {
                    user = _unitOfWork.UserInformationRepository
                        .GetOne(ui => ui.IsFired == isFired.Value && ui.FirstName + " " + ui.LastName == name, ui => ui.Team, ui => ui.Location);
                }

            }
            else
            {
                throw new EmployeeNotFoundException(name);
            }

            if (user is UserInformation)
            {
                if (user.Team == null || user.Team.Name != teamName)
                {
                    Team team = _unitOfWork.TeamRepository.GetOne(t => t.Name == teamName);
                    if (!(team is Team))
                    {
                        team = new Team { Name = teamName };
                        _unitOfWork.TeamRepository.Create(team);
                        _unitOfWork.Save();
                        team = _unitOfWork.TeamRepository.GetOne(t => t.Name == teamName);
                    }
                    user.TeamId = team.Id;
                    _unitOfWork.UserInformationRepository.Update(user, u => u.TeamId);
                }
                if (user.Location == null || user.Location.Name != workplace)
                {
                    Location location = _unitOfWork.LocationRepository.GetOne(l => l.Name == workplace);
                    if (!(location is Location))
                    {
                        location = new Location { Name = workplace };
                        _unitOfWork.LocationRepository.Create(location);
                        _unitOfWork.Save();
                        location = _unitOfWork.LocationRepository.GetOne(l => l.Name == workplace);
                    }
                    user.LocationId = location.Id;
                    _unitOfWork.UserInformationRepository.Update(user, u => u.LocationId);
                }
                _unitOfWork.Save();
                return new DbEmployeeStorageProvider(_sharedService, user.ApplicationUserId);
            }
            else
            {
                if (employmentDate != null)
                {
                    throw new EmployeeNotFoundException(name, employmentDate.Value);
                }
                else if (isFired != null)
                {
                    throw new EmployeeNotFoundException(name, isFired.Value);
                }
                else
                {
                    throw new EmployeeNotFoundException(name);
                }

            }
        }

        public IEmployeeStorageProvider AddEmployee(string name, DateTime? employmentDate, bool? isFired)
        {
            name = Regex.Replace(name, " +", " ");
            UserInformation user = null;
            if (employmentDate != null)
            {
                if (name.Split(' ').Length == 1)
                {
                    user = _unitOfWork.UserInformationRepository
                        .GetOne(ui => ui.RecruitmentDate.Value == employmentDate.Value && ui.FirstName + ui.LastName == name);
                }
                else
                {
                    user = _unitOfWork.UserInformationRepository
                        .GetOne(ui => ui.RecruitmentDate.Value == employmentDate.Value && ui.FirstName + " " + ui.LastName == name);
                }

            }
            else if (isFired != null)
            {
                if (name.Split(' ').Length == 1)
                {
                    user = _unitOfWork.UserInformationRepository
                        .GetOne(ui => ui.IsFired == isFired.Value && ui.FirstName + ui.LastName == name);
                }
                else
                {
                    user = _unitOfWork.UserInformationRepository
                        .GetOne(ui => ui.IsFired == isFired.Value && ui.FirstName + " " + ui.LastName == name);
                }
            }
            else
            {
                throw new EmployeeNotFoundException(name);
            }
            if (user is UserInformation)
            {

                return new DbEmployeeStorageProvider(_sharedService, user.ApplicationUserId);
            }
            else
            {
                if (employmentDate != null)
                {
                    throw new EmployeeNotFoundException(name, employmentDate.Value);
                }
                else if (isFired != null)
                {
                    throw new EmployeeNotFoundException(name, isFired.Value);
                }
                else
                {
                    throw new EmployeeNotFoundException(name);
                }
            }
        }
    }
}
