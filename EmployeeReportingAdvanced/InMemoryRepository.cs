using EmployeeReportingAdvanced;

public class InMemoryRepository : IEmployeeRepository
{
    public List<Employee> Employees { get; }

    public InMemoryRepository(List<Employee> employees)
    {
        Employees = employees;
    }

    public IEnumerable<Employee> GetAvailableEmployees()
    {
        return Employees;
    }
}