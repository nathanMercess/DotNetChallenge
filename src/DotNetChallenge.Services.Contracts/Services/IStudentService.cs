using DotNetChallenge.Crosscutting.Dtos;

namespace DotNetChallenge.Services.Contracts.Services;

public interface IStudentService
{
    public Task<StudentDto[]> GetAllActiveStudentsAsync();

    public Task<StudentDto> GetByIdWithJoinsAsync(Guid studentId);

    public Task<bool> CreateStudentAsync(StudentDto student);

    public Task<bool> ExcludedStudentAsync(Guid studentId);

    public Task<bool> UpdateStudentAsync(StudentDto student);
    
    public Task<StudentDto[]> GetActiveStudentsNotInCourseAsync(Guid courseId);
}
