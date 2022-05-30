using EmployeeReportingAdvanced;

public class ConsoleEmployeeNotificationService : IEmployeeNotificationService
{
    public void SendWarningMessage(string message)
    {
        Console.WriteLine("*********************************************");
        Console.WriteLine(message);
        Console.WriteLine("*********************************************");
    }
}