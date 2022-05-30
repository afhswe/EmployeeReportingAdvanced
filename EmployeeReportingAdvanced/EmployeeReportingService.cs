using System.Collections.Generic;
using System.Linq;

namespace EmployeeReportingAdvanced;

public class EmployeeReportingService
{
    private readonly IEmployeeRepository employeeRepository;

    private List<Employee> FullAgedEmployees { get; set; }

    public List<Employee> AllAvailableEmployees { get; private set; }

    public EmployeeReportingService(IEmployeeRepository employeeRepository)
    {
        this.employeeRepository = employeeRepository;
    }

    public void RetrieveAvailableEmployees()
    {
        SetAvailableEmployees();
        SetFullAgeEmployees();

        if (AllAvailableEmployees.Count < 3)
        {
            Console.WriteLine("*********************************************");
            Console.WriteLine($"WARNING: only {AllAvailableEmployees.Count} employee(s) available");
            Console.WriteLine("*********************************************");
        }
    }

    private void SetAvailableEmployees()
    {
        AllAvailableEmployees = employeeRepository.GetAvailableEmployees().ToList();
    }

    private void SetFullAgeEmployees()
    {
        FullAgedEmployees = employeeRepository
            .GetAvailableEmployees()
            .ToList()
            .Where(e => e.Age >= 18).ToList();
    }

    public List<Employee> ListEmployees()
    {
        return AllAvailableEmployees;
    }

    public List<Employee> ListEmployeesAllowedToWorkOnWeekends()
    {
        return FullAgedEmployees
            .Where(e => e.ExperienceLevel != ExperienceLevel.Novice).ToList();
    }

    public List<Employee> ListEmployeesCashCollecting()
    {
        return FullAgedEmployees
            .Where(e => (e.ExperienceLevel == ExperienceLevel.Senior && e.JobType == JobType.Salesperson) ||
                        e.JobType == JobType.Manager).ToList();
    }

    public void PrintEmployees(List<Employee> listEmployeesAllowedToWorkOnWeekends)
    {
        Console.WriteLine();
        listEmployeesAllowedToWorkOnWeekends.ForEach(
            e =>
            {
                Console.WriteLine($"{e.Name}, {e.Age}, {e.JobType}, {e.ExperienceLevel}");
            });
    }
}