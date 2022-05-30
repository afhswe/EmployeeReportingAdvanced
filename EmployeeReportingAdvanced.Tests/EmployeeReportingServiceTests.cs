using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using FluentAssertions;

namespace EmployeeReportingAdvanced.Tests;

public class EmployeeReportingServiceTests
{
    private Mock<IEmployeeRepository> employeeRepositoryStub;
    private Mock<IEmployeeNotificationService> employeeNotificationServiceMock;

    public EmployeeReportingServiceTests()
    {
        employeeRepositoryStub = new Mock<IEmployeeRepository>();
        employeeNotificationServiceMock = new Mock<IEmployeeNotificationService>();
    }

    [Fact]
    public void ListEmployees_ReturnsCorrectEmployees()
    {
        var repositoryEmployees = new List<Employee>()
        {
            new Employee("Andrea Huber", 17, JobType.Storeman, ExperienceLevel.Novice),
            new Employee("Charles Miller", 28, JobType.Salesperson, ExperienceLevel.Intermediate),
        };
        var sut = CreateEmployeeServiceWithEmployees(repositoryEmployees);

        sut.RetrieveAvailableEmployees();
        var availableEmployeesResult = sut.ListEmployees();
        availableEmployeesResult.Count.Should().Be(2);
        availableEmployeesResult.Should().ContainEquivalentOf(repositoryEmployees[0]);
        availableEmployeesResult.Should().ContainEquivalentOf(repositoryEmployees[1]);
    }

    private EmployeeReportingService CreateEmployeeServiceWithEmployees(List<Employee> repositoryEmployees)
    {
        employeeRepositoryStub.Setup(x => x.GetAvailableEmployees()).Returns(repositoryEmployees);
        var sut = new EmployeeReportingService(employeeRepositoryStub.Object, employeeNotificationServiceMock.Object);
        return sut;
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
        var sut = CreateEmployeeServiceWithEmployees(repositoryEmployees);

        sut.RetrieveAvailableEmployees();
        var allowedEmployeesResult = sut.ListEmployeesAllowedToWorkOnWeekends();
        allowedEmployeesResult.Should().ContainEquivalentOf(repositoryEmployees[1]);
        allowedEmployeesResult.Should().ContainEquivalentOf(repositoryEmployees[3]);
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
        var sut = CreateEmployeeServiceWithEmployees(repositoryEmployees);

        sut.RetrieveAvailableEmployees();
        var allowedEmployeesResult = sut.ListEmployeesCashCollecting();
        allowedEmployeesResult.Should().ContainEquivalentOf(repositoryEmployees[0]);
        allowedEmployeesResult.Should().ContainEquivalentOf(repositoryEmployees[3]);
    }

    [Fact]
    public void RetrieveAvailableEmployees_SendsWarningIfTooFewEmployeesAvailable()
    {
        var repositoryEmployees = new List<Employee>()
        {
            new Employee("Andrea Huber", 24, JobType.Manager, ExperienceLevel.Novice),
            new Employee("Charles Miller", 28, JobType.Salesperson, ExperienceLevel.Intermediate)
        };
        var sut = CreateEmployeeServiceWithEmployees(repositoryEmployees);
        sut.RetrieveAvailableEmployees();

        employeeNotificationServiceMock.Verify(x => x.SendWarningMessage($"WARNING: only 2 employee(s) available"), Times.Once);
    }
}