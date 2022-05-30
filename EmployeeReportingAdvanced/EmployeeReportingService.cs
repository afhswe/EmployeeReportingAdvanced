using System.Collections.Generic;
using System.Linq;

namespace EmployeeReportingAdvanced.Tests;

public class EmployeeReportingService
{
    private readonly IEmployeeRepository employeeRepository;

    public EmployeeReportingService(IEmployeeRepository employeeRepository)
    {
        this.employeeRepository = employeeRepository;
    }

    public void RetrieveAvailableEmployees()
    {
        AllAvailableEmployees = employeeRepository.GetAvailableEmployees().ToList();
        SetFullAgeEmployees();
    }

    private void SetFullAgeEmployees()
    {
        FullAgedEmployees = employeeRepository
            .GetAvailableEmployees()
            .ToList()
            .Where(e => e.Age >= 18).ToList();
    }

    private List<Employee> FullAgedEmployees { get; set; }

    public List<Employee> AllAvailableEmployees { get; private set; }

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
}