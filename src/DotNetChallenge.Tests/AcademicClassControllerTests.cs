using DotNetChallenge.Crosscutting.Dtos;
using DotNetChallenge.Services.Contracts.Services;
using DotNetChallenge.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class AcademicClassControllerTests
{
    private readonly AcademicClassController _controller;
    private readonly Mock<IAcademicClassService> _mockService;

    public AcademicClassControllerTests()
    {
        _mockService = new Mock<IAcademicClassService>();
        _controller = new AcademicClassController(_mockService.Object);
    }

    [Fact]
    public async Task Index_ReturnsViewResult_WithListOfClasses()
    {
        var mockClasses = new AcademicClassDto[] { new AcademicClassDto(), new AcademicClassDto() };
        _mockService.Setup(service => service.GetAllActiveClassesAsync())
                    .ReturnsAsync(mockClasses);

        var result = await _controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(mockClasses, viewResult.Model);
    }

    [Fact]
    public async Task GetById_ReturnsViewResult_WithClassDetails()
    {
        var classId = Guid.NewGuid();
        var mockClass = new AcademicClassDto();
        _mockService.Setup(service => service.GetByIdWithJoinsAsync(classId))
                    .ReturnsAsync(mockClass);

        var result = await _controller.GetById(classId);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(mockClass, viewResult.Model);
        Assert.Equal("~/Views/AcademicClass/AcademicClassDetails.cshtml", viewResult.ViewName);
    }

    [Fact]
    public async Task CreateAcademicClass_ReturnsOkResult_WhenClassCreated()
    {
        var newClass = new AcademicClassDto();
        _mockService.Setup(service => service.CreateClassAsync(newClass)).ReturnsAsync(true);

        var result = await _controller.CreateAcademicClass(newClass);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)okResult.Value);
    }

    [Fact]
    public async Task UpdateAcademicClass_ReturnsOkResult_WhenClassUpdated()
    {
        var existingClass = new AcademicClassDto();
        _mockService.Setup(service => service.UpdateClassAsync(existingClass)).ReturnsAsync(true);

        var result = await _controller.UpdateAcademicClass(existingClass);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)okResult.Value);
    }

    [Fact]
    public async Task ExcludeAcademicClass_ReturnsOkResult_WhenClassExcluded()
    {
        var classId = Guid.NewGuid();
        _mockService.Setup(service => service.ExcludeClassAsync(classId)).ReturnsAsync(true);

        var result = await _controller.ExcludeAcademicClass(classId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)okResult.Value);
    }

    [Fact]
    public async Task AddStudentToClass_ReturnsOkResult_WhenStudentAdded()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        _mockService.Setup(service => service.AddStudentToClassAsync(classId, studentId)).ReturnsAsync(true);

        // Act
        var result = await _controller.AddStudentToClass(classId, studentId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)okResult.Value);
    }

    [Fact]
    public async Task RemoveStudentFromClass_ReturnsOkResult_WhenStudentRemoved()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        _mockService.Setup(service => service.RemoveStudentFromClassAsync(classId, studentId)).ReturnsAsync(true);

        // Act
        var result = await _controller.RemoveStudentFromClass(classId, studentId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)okResult.Value);
    }
}
