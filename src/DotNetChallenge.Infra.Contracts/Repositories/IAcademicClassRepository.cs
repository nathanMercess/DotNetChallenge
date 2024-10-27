using DotNetChallenge.Domain.Entities;

namespace DotNetChallenge.Infra.Contracts.Repositories;

public interface IAcademicClassRepository
{
    Task<AcademicClass[]> GetAllActiveClassesAsync();

    Task<AcademicClass> GetByIdAsync(Guid classId);

    Task<AcademicClass> GetByIdWithJoinsAsync(Guid classId);

    Task<bool> InsertClassAsync(AcademicClass academicClass);

    Task<bool> UpdateClassAsync(AcademicClass academicClass);

    Task<bool> ExcludeClassAsync(Guid classId);

    Task<AcademicClass> ExistsByClassNameAsync(string className);
}
