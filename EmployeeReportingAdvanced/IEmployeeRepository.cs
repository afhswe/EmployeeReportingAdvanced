using System.Collections.Generic;

namespace EmployeeReportingAdvanced;

public interface IEmployeeRepository
{
    IEnumerable<Employee> GetAvailableEmployees();
}