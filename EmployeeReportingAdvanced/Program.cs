// See https://aka.ms/new-console-template for more information

using EmployeeReportingAdvanced;

var employees = new List<Employee>()
{
    new Employee("Andrea Huber", 17, JobType.Storeman, ExperienceLevel.Novice),
    new Employee("Charles Miller", 28, JobType.Salesperson, ExperienceLevel.Intermediate),
    new Employee("Marie Blanchet", 39, JobType.Manager, ExperienceLevel.Novice),
    new Employee("Henry Majors", 43, JobType.Salesperson, ExperienceLevel.Senior)
};

var employeeRepository = new InMemoryRepository(employees);
var employeeReportingService = new EmployeeReportingService(employeeRepository);
employeeReportingService.RetrieveAvailableEmployees();

Console.WriteLine("----------------------------------------");
Console.WriteLine("All employees available: ");
employeeReportingService.PrintEmployees(employeeReportingService.ListEmployees());

Console.WriteLine("----------------------------------------");
Console.WriteLine("Employees allowed to work on weekends: ");
employeeReportingService.PrintEmployees(employeeReportingService.ListEmployeesAllowedToWorkOnWeekends());

Console.WriteLine("----------------------------------------");
Console.WriteLine("Employees allowed to collect cash: ");
employeeReportingService.PrintEmployees(employeeReportingService.ListEmployeesCashCollecting());
Console.WriteLine("----------------------------------------");
