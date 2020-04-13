using System;
using System.Collections.Generic;
using System.Text;
using TOT.DataImport.Interfaces;
using TOT.Entities;
using static TOT.DataImport.StorageProviders.InMemoryEmployeeStorageProvider;

namespace TOT.DataImport.StorageProviders
{
    class InMemoryStorageProvider : IStorageProvider
    {
        private List<Employee> employees = new List<Employee>();

        public IEmployeeStorageProvider AddEmployeeIfNotExists(
            string name, DateTime? employmentDate = null, string teamName = null, string workPlace = null)
        {
            Employee employee;
            int index = employees.FindIndex(e => e.Name == name);
            if (index != -1)
            {
                employee = employees[index];
                if (employmentDate != null) employee.EmploymentDate = employmentDate;
                if (teamName != null) employee.TeamName = teamName;
                if (workPlace != null) employee.WorkPlace = workPlace;
            } else
            {
                employee = new Employee
                {
                    Name = name,
                    EmploymentDate = employmentDate,
                    TeamName = teamName,
                    WorkPlace = workPlace
                };

                employees.Add(employee);
            }

            return new InMemoryEmployeeStorageProvider { SourceEmployee = employee };
        }
    }
}
