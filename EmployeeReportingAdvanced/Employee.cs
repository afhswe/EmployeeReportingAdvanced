namespace EmployeeReportingAdvanced;

public class Employee
{
    public string Name { get; }
    public int Age { get; }
    public JobType JobType { get; }
    public ExperienceLevel ExperienceLevel { get; }

    public Employee(string name, int age, JobType jobType, ExperienceLevel experienceLevel)
    {
        Name = name;
        Age = age;
        JobType = jobType;
        ExperienceLevel = experienceLevel;
    }
}