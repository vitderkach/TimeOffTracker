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
        public EmployeeNotFoundException() { }
        
        public EmployeeNotFoundException(string employeeName, DateTime employmentDate) 
            : base($"The employee {employeeName} with recruitment date {employmentDate}.") 
        {
            EmployeeName = employeeName;
            EmploymentDate = employmentDate;
        }
        public EmployeeNotFoundException(string message) : base(message) { }

        public EmployeeNotFoundException(string message, System.Exception inner)
            : base(message, inner) { }

        protected EmployeeNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
