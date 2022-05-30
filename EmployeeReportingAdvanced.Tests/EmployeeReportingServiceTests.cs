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
        var sut = CreateEmployeeServiceWithEmployees(new List<Employee>()
        {
            ANoviceEmployee(),
            AnIntermediateEmployee(),
        });

        sut.RetrieveAvailableEmployees();
        var availableEmployeesResult = sut.ListEmployees();
        availableEmployeesResult.Count.Should().Be(2);
        availableEmployeesResult.Should().ContainEquivalentOf(ANoviceEmployee());
        availableEmployeesResult.Should().ContainEquivalentOf(AnIntermediateEmployee());
    }

    [Fact]
    public void ListEmployeesWeekendWork_ReturnsOnlyEmployees_AllowedToWorkOnWeekends()
    {
        var sut = CreateEmployeeServiceWithEmployees(new List<Employee>()
        {
            ANoviceEmployee(),
            AnIntermediateEmployee(),
            ANoviceSalesperson(),
            ASeniorSalesperson(),
        });

        sut.RetrieveAvailableEmployees();
        var allowedEmployeesResult = sut.ListEmployeesAllowedToWorkOnWeekends();
        allowedEmployeesResult.Should().ContainEquivalentOf(AnIntermediateEmployee());
        allowedEmployeesResult.Should().ContainEquivalentOf(ASeniorSalesperson());
    }

    [Fact]
    public void ListEmployeesCashCollecting_ReturnsOnlyEmployees_AllowedToCollectCash()
    {
        var sut = CreateEmployeeServiceWithEmployees(new List<Employee>()
        {
            AManager(),
            AnIntermediateSalesperson(),
            ASeniorStoreman(),
            ASeniorSalesperson(),
        });

        sut.RetrieveAvailableEmployees();

        var allowedEmployeesResult = sut.ListEmployeesCashCollecting();
        allowedEmployeesResult.Should().ContainEquivalentOf(AManager());
        allowedEmployeesResult.Should().ContainEquivalentOf(ASeniorSalesperson());
    }

    [Fact]
    public void RetrieveAvailableEmployees_SendsWarningIfTooFewEmployeesAvailable()
    {
        var sut = CreateEmployeeServiceWithEmployees(new List<Employee>()
        {
            AnEmployee(),
            AnEmployee()
        });
        sut.RetrieveAvailableEmployees();

        employeeNotificationServiceMock.Verify(
            x => x.SendWarningMessage(It.Is<string>(s => s.Contains("2 employee(s)"))), Times.Once);
    }

    [Fact]
    public void RetrieveAvailableEmployees_NoWarningIfEnoughEmployeesAvailable()
    {
        var sut = CreateEmployeeServiceWithEmployees(new List<Employee>()
        {
            AnEmployee(),
            AnEmployee(),
            AnEmployee(),
            AnEmployee()
        });
        sut.RetrieveAvailableEmployees();

        employeeNotificationServiceMock.Verify(x => x.SendWarningMessage(It.IsAny<string>()), Times.Never);
    }

    private EmployeeReportingService CreateEmployeeServiceWithEmployees(List<Employee> repositoryEmployees)
    {
        employeeRepositoryStub.Setup(x => x.GetAvailableEmployees()).Returns(repositoryEmployees);
        var sut = new EmployeeReportingService(employeeRepositoryStub.Object, employeeNotificationServiceMock.Object);
        return sut;
    }

    private Employee ASeniorSalesperson()
    {
        return new Employee("Henry Majors", 44, JobType.Salesperson, ExperienceLevel.Senior);
    }

    private Employee ASeniorStoreman()
    {
        return new Employee("Marie Blanchet", 39, JobType.Storeman, ExperienceLevel.Senior);
    }

    private Employee AnIntermediateSalesperson()
    {
        return new Employee("Charles Miller", 28, JobType.Salesperson, ExperienceLevel.Intermediate);
    }

    private Employee AManager()
    {
        return new Employee("Andrea Huber", 24, JobType.Manager, ExperienceLevel.Novice);
    }

    private Employee AnEmployee()
    {
        return new Employee("Andrea Huber", 24, JobType.Manager, ExperienceLevel.Novice);
    }

    private Employee ANoviceSalesperson()
    {
        return new Employee("Marie Blanchet", 39, JobType.Salesperson, ExperienceLevel.Novice);
    }

    private Employee AnIntermediateEmployee()
    {
        return new Employee("Charles Miller", 28, JobType.Salesperson, ExperienceLevel.Intermediate);
    }

    private Employee ANoviceEmployee()
    {
        return new Employee("Andrea Huber", 17, JobType.Storeman, ExperienceLevel.Novice);
    }
}