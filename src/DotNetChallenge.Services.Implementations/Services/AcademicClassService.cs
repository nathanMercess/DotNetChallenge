using DotNetChallenge.Crosscutting.Constants;
using DotNetChallenge.Crosscutting.Dtos;
using DotNetChallenge.Crosscutting.Exceptions;
using DotNetChallenge.Crosscutting.Extensions;
using DotNetChallenge.Domain.Entities;
using DotNetChallenge.Domain.Extensions;
using DotNetChallenge.Infra.Contracts.Repositories;
using DotNetChallenge.Services.Contracts.Services;

namespace DotNetChallenge.Services.Implementations.Services;

internal sealed class AcademicClassService : IAcademicClassService
{
    private readonly IAcademicClassRepository _academicClassRepository;

    public AcademicClassService(IAcademicClassRepository academicClassRepository) => _academicClassRepository = academicClassRepository;

    public async Task<AcademicClassDto[]> GetAllActiveClassesAsync()
    {
        AcademicClass[]? academicClasses = await _academicClassRepository.GetAllActiveClassesAsync();

        if (academicClasses is null)
            throw new HandledException(ExceptionConstants.CLASS_NOT_FOUND);

        return academicClasses.ToArrayAcademicClassDto();
    }

    public async Task<AcademicClassDto> GetByIdWithJoinsAsync(Guid classId)
    {
        if (classId.IsInvalid())
            throw new HandledException(ExceptionConstants.INVALID_CONTRACT);

        AcademicClass academicClass = await _academicClassRepository.GetByIdWithJoinsAsync(classId);

        if (academicClass is null)
            throw new HandledException(ExceptionConstants.CLASS_NOT_FOUND);

        return academicClass.ToAcademicClassDto();
    }

    public async Task<bool> CreateClassAsync(AcademicClassDto request)
    {
        if (request is null)
            throw new HandledException(ExceptionConstants.INVALID_CONTRACT);

        var classExists = await _academicClassRepository.ExistsByClassNameAsync(request.ClassName);

        if (classExists is not null)
            throw new HandledException(ExceptionConstants.DUPLICATE_CLASS_NAME);

        AcademicClass academicClass = request.ToAcademicClass();

        return await _academicClassRepository.InsertClassAsync(academicClass);
    }

    public async Task<bool> UpdateClassAsync(AcademicClassDto request)
    {
        if (request is null)
            throw new HandledException(ExceptionConstants.INVALID_CONTRACT);

        var classExists = await _academicClassRepository.ExistsByClassNameAsync(request.ClassName);

        if (classExists is not null && classExists.Id != request.Id)
            throw new HandledException(ExceptionConstants.DUPLICATE_CLASS_NAME);

        AcademicClass academicClass = await GetAcademicClassById(request.Id);

        if (academicClass is null)
            throw new HandledException(ExceptionConstants.CLASS_NOT_FOUND);

        academicClass.SetAcademicClass(request.ClassName, request.Year);

        return await _academicClassRepository.UpdateClassAsync(academicClass);
    }

    public async Task<bool> ExcludeClassAsync(Guid classId)
    {
        if (classId.IsInvalid())
            throw new HandledException(ExceptionConstants.INVALID_CONTRACT);

        AcademicClass academicClass = await GetAcademicClassById(classId);

        academicClass.SetExcluded();

        return await _academicClassRepository.ExcludeClassAsync(classId);
    }

    public async Task<bool> AddStudentToClassAsync(Guid classId, Guid studentId)
    {
        if (classId.IsInvalid() || studentId.IsInvalid())
            throw new HandledException(ExceptionConstants.INVALID_CONTRACT);

        StudentClassEnrollment studentClassEnrollment = new StudentClassEnrollment(classId, studentId) { };

        return await _academicClassRepository.AddStudentToClassAsync(studentClassEnrollment);
    }

    private async Task<AcademicClass> GetAcademicClassById(Guid classId)
    {
        AcademicClass academicClass = await _academicClassRepository.GetByIdAsync(classId);

        if (academicClass is null)
            throw new HandledException(ExceptionConstants.CLASS_NOT_FOUND);

        return academicClass;
    }
}
