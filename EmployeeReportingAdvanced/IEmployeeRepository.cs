using System.Collections.Generic;

namespace EmployeeReportingAdvanced.Tests;

public interface IEmployeeRepository
{
    IEnumerable<Employee> GetAvailableEmployees();
}