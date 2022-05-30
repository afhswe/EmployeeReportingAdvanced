namespace EmployeeReportingAdvanced;

public class InMemoryEmployeeRepository : IEmployeeRepository
{
    public List<Employee> Employees { get; }

    public InMemoryEmployeeRepository(List<Employee> employees)
    {
        Employees = employees;
    }

    public IEnumerable<Employee> GetAvailableEmployees()
    {
        return Employees;
    }
}