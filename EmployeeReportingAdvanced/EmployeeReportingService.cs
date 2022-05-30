using System.Collections.Generic;
using System.Linq;

namespace EmployeeReportingAdvanced;

public class EmployeeReportingService
{
    private readonly IEmployeeRepository employeeRepository;
    private IEmployeeNotificationService employeeNotificationService;

    private List<Employee> FullAgedEmployees { get; set; }

    private List<Employee> AllAvailableEmployees { get; set; }

    public EmployeeReportingService(IEmployeeRepository employeeRepository, IEmployeeNotificationService employeeNotificationService)
    {
        this.employeeRepository = employeeRepository;
        this.employeeNotificationService = employeeNotificationService;
    }

    public void RetrieveAvailableEmployees()
    {
        SetAvailableEmployees();
        SetFullAgeEmployees();

        if (AllAvailableEmployees.Count < 3)
        {
            SendWarningAboutTooFewAvailableEmployees();
        }
    }

    private void SendWarningAboutTooFewAvailableEmployees()
    {
        var message = $"WARNING: only {AllAvailableEmployees.Count} employee(s) available. Please reach out to +32-6666-6666 immediately upon receiption";
        employeeNotificationService.SendWarningMessage(message);
    }

    private void SetAvailableEmployees()
    {
        AllAvailableEmployees = new List<Employee>();
        var employees = employeeRepository.GetAvailableEmployees();
        foreach (var employee in employees)
        {
            AllAvailableEmployees.Add(
                new Employee(employee.Name, employee.Age, employee.JobType, employee.ExperienceLevel)
                );
        }
    }

    private void SetFullAgeEmployees()
    {
        FullAgedEmployees = AllAvailableEmployees
            .Where(e => e.Age >= 18).ToList();
    }

    public List<Employee> ListEmployees()
    {
        return AllAvailableEmployees.ToList();
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