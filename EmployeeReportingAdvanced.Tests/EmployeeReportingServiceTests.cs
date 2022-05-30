using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using FluentAssertions;

namespace EmployeeReportingAdvanced.Tests
{
    public class EmployeeReportingServiceTests
    {
        [Fact]
        public void AddAvailableEmployees_UpdatesEmployeeList()
        {
            var fakeEmployees = new List<Employee>()
            {
                new Employee("Mark Rogers", 20)
            };
            var employeeRepository = new Mock<IEmployeeRepository>();
            employeeRepository.Setup(x => x.GetAvailableEmployees()).Returns(fakeEmployees);

            var sut = new EmployeeReportingService(employeeRepository.Object);
            sut.RetrieveAvailableEmployees();
            sut.AvailableEmployees.Count.Should().Be(1);
            sut.AvailableEmployees[0].Should().BeSameAs(fakeEmployees[0]);
        }
    }

    public class EmployeeReportingService
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeReportingService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public void RetrieveAvailableEmployees()
        {
            AvailableEmployees = employeeRepository.GetAvailableEmployees().ToList();
        }

        public List<Employee> AvailableEmployees { get; private set; }
    }

    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAvailableEmployees();
    }

    public class Employee
    {
        public string Name { get; }
        public int Age { get; }

        public Employee(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}