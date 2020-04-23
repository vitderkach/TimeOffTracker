using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.DataImport.Exceptions
{
    [Serializable]
    public class EmployeeNotFoundException : ApplicationException
    {
        public string EmployeeName { get; private set; }
        public DateTime? EmploymentDate { get; private set; }
        public bool? IsFired { get; private set; }
        public EmployeeNotFoundException() { }
        
        public EmployeeNotFoundException(string employeeName, DateTime employmentDate) 
            : base($"The employee {employeeName} with recruitment date {employmentDate} wasn't found.") 
        {
            EmployeeName = employeeName;
            EmploymentDate = employmentDate;
        }

        public EmployeeNotFoundException(string employeeName, bool isFired)
    : base($"The employee {employeeName}, which {((isFired == true) ? "has been fired from company." : "works now in company")}, wasn't found.")
        {
            EmployeeName = employeeName;
            IsFired = isFired;
        }
        public EmployeeNotFoundException(string employeeName) : base($"The employee {employeeName}, wasn't found.") { }

        public EmployeeNotFoundException(string employeeName, System.Exception inner)
            : base(employeeName, inner) { }

        protected EmployeeNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
