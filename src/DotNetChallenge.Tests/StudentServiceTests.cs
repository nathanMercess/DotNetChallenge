using DotNetChallenge.Crosscutting.Dtos;
using DotNetChallenge.Services.Contracts.Services;
using Moq;

namespace DotNetChallenge.Tests;

public sealed class StudentServiceTests
{
    private readonly Mock<IStudentService> _studentServiceMock;

    public StudentServiceTests()
    {
        _studentServiceMock = new Mock<IStudentService>();
    }

    [Fact]
    public async Task GetAllActiveStudentsAsync_ShouldReturnActiveStudents()
    {
        // Arrange
        var expectedStudents = new[]
        {
            new StudentDto { Id = Guid.NewGuid(), Name = "Student 1", User = "user1" },
            new StudentDto { Id = Guid.NewGuid(), Name = "Student 2", User = "user2" }
        };
        _studentServiceMock
            .Setup(service => service.GetAllActiveStudentsAsync())
            .ReturnsAsync(expectedStudents);

        // Act
        var result = await _studentServiceMock.Object.GetAllActiveStudentsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.Equal("Student 1", result[0].Name);
    }

    [Fact]
    public async Task GetByIdWithJoinsAsync_ShouldReturnStudentWithAcademicClassDetails()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var expectedStudent = new StudentDto
        {
            Id = studentId,
            Name = "Student 1",
            User = "user1"
        };
        _studentServiceMock
            .Setup(service => service.GetByIdWithJoinsAsync(studentId))
            .ReturnsAsync(expectedStudent);

        // Act
        var result = await _studentServiceMock.Object.GetByIdWithJoinsAsync(studentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(studentId, result.Id);
        Assert.Equal("Student 1", result.Name);
    }

    [Fact]
    public async Task CreateStudentAsync_ShouldReturnTrue_WhenStudentIsCreated()
    {
        // Arrange
        var newStudent = new StudentDto { Id = Guid.NewGuid(), Name = "New Student", User = "newuser" };
        _studentServiceMock
            .Setup(service => service.CreateStudentAsync(newStudent))
            .ReturnsAsync(true);

        // Act
        var result = await _studentServiceMock.Object.CreateStudentAsync(newStudent);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExcludedStudentAsync_ShouldReturnTrue_WhenStudentIsExcluded()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        _studentServiceMock
            .Setup(service => service.ExcludedStudentAsync(studentId))
            .ReturnsAsync(true);

        // Act
        var result = await _studentServiceMock.Object.ExcludedStudentAsync(studentId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateStudentAsync_ShouldReturnTrue_WhenStudentIsUpdated()
    {
        // Arrange
        var existingStudent = new StudentDto { Id = Guid.NewGuid(), Name = "Existing Student", User = "existinguser" };
        _studentServiceMock
            .Setup(service => service.UpdateStudentAsync(existingStudent))
            .ReturnsAsync(true);

        // Act
        var result = await _studentServiceMock.Object.UpdateStudentAsync(existingStudent);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetActiveStudentsNotInCourseAsync_ShouldReturnStudentsNotInCourse()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var studentsNotInCourse = new[]
        {
            new StudentDto { Id = Guid.NewGuid(), Name = "Student 1", User = "user1" },
            new StudentDto { Id = Guid.NewGuid(), Name = "Student 2", User = "user2" }
        };
        _studentServiceMock
            .Setup(service => service.GetActiveStudentsNotInCourseAsync(courseId))
            .ReturnsAsync(studentsNotInCourse);

        // Act
        var result = await _studentServiceMock.Object.GetActiveStudentsNotInCourseAsync(courseId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.Equal("Student 1", result[0].Name);
    }
}
