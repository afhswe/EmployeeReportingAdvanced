using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using FluentAssertions;

namespace EmployeeReportingAdvanced.Tests;

public class EmployeeReportingServiceTests
{
    [Fact]
    public void RetrieveAvailableEmployees_StoresEmployeeList()
    {
        var repositoryEmployees = new List<Employee>()
        {
            new Employee("Emily Bache", 17, JobType.Salesperson, ExperienceLevel.Novice),
            new Employee("Martin Fowler", 16, JobType.Storeman, ExperienceLevel.Novice),
            new Employee("Kent Beck", 23, JobType.Manager, ExperienceLevel.Intermediate),
        };
        var employeeRepository = new Mock<IEmployeeRepository>();
        employeeRepository.Setup(x => x.GetAvailableEmployees()).Returns(repositoryEmployees);

        var sut = new EmployeeReportingService(employeeRepository.Object);
        sut.RetrieveAvailableEmployees();

        sut.AllAvailableEmployees.Count.Should().Be(3);
        sut.AllAvailableEmployees[0].Should().BeSameAs(repositoryEmployees[0]);
        sut.AllAvailableEmployees[1].Should().BeSameAs(repositoryEmployees[1]);
        sut.AllAvailableEmployees[2].Should().BeSameAs(repositoryEmployees[2]);

        employeeRepository.Verify(x => x.GetAvailableEmployees(), Times.Exactly(2));
    }

    [Fact]
    public void ListEmployees_ReturnsCorrectEmployees()
    {
        var repositoryEmployees = new List<Employee>()
        {
            new Employee("Andrea Huber", 17, JobType.Storeman, ExperienceLevel.Novice),
            new Employee("Charles Miller", 28, JobType.Salesperson, ExperienceLevel.Intermediate),
        };
        var employeeRepository = new Mock<IEmployeeRepository>();
        employeeRepository.Setup(x => x.GetAvailableEmployees()).Returns(repositoryEmployees);

        var sut = new EmployeeReportingService(employeeRepository.Object);
        sut.RetrieveAvailableEmployees();
        var availableEmployeesResult = sut.ListEmployees();
        availableEmployeesResult.Count.Should().Be(2);
        availableEmployeesResult[0].Should().BeSameAs(repositoryEmployees[0]);
        availableEmployeesResult[1].Should().BeSameAs(repositoryEmployees[1]);

        employeeRepository.Verify(x => x.GetAvailableEmployees(), Times.Exactly(2));
    }

    [Fact]
    public void ListEmployeesWeekendWork_ReturnsOnlyEmployees_AllowedToWorkOnWeekends()
    {
        var repositoryEmployees = new List<Employee>()
        {
            new Employee("Andrea Huber", 17, JobType.Storeman, ExperienceLevel.Novice),
            new Employee("Charles Miller", 28, JobType.Salesperson, ExperienceLevel.Intermediate),
            new Employee("Marie Blanchet", 39, JobType.Salesperson, ExperienceLevel.Novice),
            new Employee("Henry Majors", 43, JobType.Salesperson, ExperienceLevel.Senior),
        };
        var employeeRepository = new Mock<IEmployeeRepository>();
        employeeRepository.Setup(x => x.GetAvailableEmployees()).Returns(repositoryEmployees);

        var sut = new EmployeeReportingService(employeeRepository.Object);
        sut.RetrieveAvailableEmployees();
        var allowedEmployeesResult = sut.ListEmployeesAllowedToWorkOnWeekends();
        allowedEmployeesResult[0].Should().BeSameAs(repositoryEmployees[1]);
        allowedEmployeesResult[1].Should().BeSameAs(repositoryEmployees[3]);

        employeeRepository.Verify(x => x.GetAvailableEmployees(), Times.Exactly(2));
    }

    [Fact]
    public void ListEmployeesCashCollecting_ReturnsOnlyEmployees_AllowedToCollectCash()
    {
        var repositoryEmployees = new List<Employee>()
        {
            new Employee("Andrea Huber", 24, JobType.Manager, ExperienceLevel.Novice),
            new Employee("Charles Miller", 28, JobType.Salesperson, ExperienceLevel.Intermediate),
            new Employee("Marie Blanchet", 39, JobType.Storeman, ExperienceLevel.Senior),
            new Employee("Henry Majors", 44, JobType.Salesperson, ExperienceLevel.Senior),
        };
        var employeeRepository = new Mock<IEmployeeRepository>();
        employeeRepository.Setup(x => x.GetAvailableEmployees()).Returns(repositoryEmployees);

        var sut = new EmployeeReportingService(employeeRepository.Object);
        sut.RetrieveAvailableEmployees();
        var allowedEmployeesResult = sut.ListEmployeesCashCollecting();
        allowedEmployeesResult[0].Should().BeSameAs(repositoryEmployees[0]);
        allowedEmployeesResult[1].Should().BeSameAs(repositoryEmployees[3]);

        employeeRepository.Verify(x => x.GetAvailableEmployees(), Times.Exactly(2));
    }
}