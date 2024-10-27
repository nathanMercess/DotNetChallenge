using DotNetChallenge.Crosscutting.Dtos;

namespace DotNetChallenge.Services.Contracts.Services;
public interface IAcademicClassService
{
    Task<AcademicClassDto[]> GetAllActiveClassesAsync();

    Task<AcademicClassDto> GetByIdWithJoinsAsync(Guid classId);

    Task<bool> CreateClassAsync(AcademicClassDto academicClass);

    Task<bool> ExcludeClassAsync(Guid classId);

    Task<bool> UpdateClassAsync(AcademicClassDto academicClass);
    
    Task<bool> AddStudentToClassAsync(Guid classId, Guid studentId);
}