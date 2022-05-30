using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using FluentAssertions;

namespace EmployeeReportingAdvanced.Tests;

public class EmployeeReportingServiceTests
{
    [Fact]
    public void AddAvailableEmployees_UpdatesEmployeeList()
    {
        var repositoryEmployees = new List<Employee>()
        {
            new Employee("Andrea Huber", 17, JobType.Storeman, ExperienceLevel.Novice),
        };
        var employeeRepository = new Mock<IEmployeeRepository>();
        employeeRepository.Setup(x => x.GetAvailableEmployees()).Returns(repositoryEmployees);

        var sut = new EmployeeReportingService(employeeRepository.Object);
        sut.RetrieveAvailableEmployees();
        sut.AllAvailableEmployees.Count.Should().Be(1);
        sut.AllAvailableEmployees[0].Should().BeSameAs(repositoryEmployees[0]);

        employeeRepository.Verify(x => x.GetAvailableEmployees(), Times.Exactly(2));
    }

    [Fact]
    public void ListEmployees_ReturnsAllAvailableEmployees()
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
        var availableEmployees = sut.ListEmployees();
        sut.AllAvailableEmployees.Count.Should().Be(2);
        sut.AllAvailableEmployees[0].Should().BeSameAs(availableEmployees[0]);
        sut.AllAvailableEmployees[1].Should().BeSameAs(availableEmployees[1]);

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
        var employeesAllowedResult = sut.ListEmployeesAllowedToWorkOnWeekends();
        sut.FullAgedEmployees.Should().Contain(repositoryEmployees[1]);
        sut.FullAgedEmployees.Should().Contain(repositoryEmployees[3]);

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
        var employeesAllowedResult = sut.ListEmployeesCashCollecting();
        sut.FullAgedEmployees.Should().Contain(repositoryEmployees[0]);
        sut.FullAgedEmployees.Should().Contain(repositoryEmployees[3]);

        employeeRepository.Verify(x => x.GetAvailableEmployees(), Times.Exactly(2));
    }
}