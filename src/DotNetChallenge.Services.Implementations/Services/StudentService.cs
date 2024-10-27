using DotNetChallenge.Crosscutting.Constants;
using DotNetChallenge.Crosscutting.Dtos;
using DotNetChallenge.Crosscutting.Exceptions;
using DotNetChallenge.Crosscutting.Extensions;
using DotNetChallenge.Domain.Entities;
using DotNetChallenge.Domain.Extensions;
using DotNetChallenge.Infra.Contracts.Repositories;
using DotNetChallenge.Services.Contracts.Services;

namespace DotNetChallenge.Services.Implementations.Services;

internal sealed class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;

    private readonly IPasswordHasherService _passwordHasherService;

    public StudentService(
        IStudentRepository studentRepository,
        IPasswordHasherService passwordHasherService)
    {
        _studentRepository = studentRepository;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<StudentDto[]> GetAllActiveStudentsAsync()
    {
        Student[] student = await _studentRepository.GetAllActiveStudentsAsync();

        if (student is null) throw new HandledException(ExceptionConstants.STUDENT_NOT_FOUND);

        return student.ToArrayStudentDto();
    }

    public async Task<StudentDto> GetByIdWithJoinsAsync(Guid studentId)
    {
        if (studentId.IsInvalid()) throw new HandledException(ExceptionConstants.INVALID_CONTRACT);

        Student student = await _studentRepository.GetByIdWithJoinsAsync(studentId);

        if (student is null) throw new HandledException(ExceptionConstants.STUDENT_NOT_FOUND);

        return student.ToStudentDto();
    }


    public async Task<bool> CreateStudentAsync(StudentDto request)
    {
        if (request is null) throw new HandledException(ExceptionConstants.INVALID_CONTRACT);

        request.Password = _passwordHasherService.HashPassword(request.Password);

        Student student = request.ToStudentDto();

        return await _studentRepository.InsertStudentAsync(student);
    }

    public async Task<bool> UpdateStudentAsync(StudentDto request)
    {
        if (request is null) throw new HandledException(ExceptionConstants.INVALID_CONTRACT);

        Student student = await GetStudentById(request.Id);

        student.SetBasicInformation(request.Name, request.User);

        return await _studentRepository.UpdateStudentAsync(student);

    }

    public async Task<bool> ExcludedStudentAsync(Guid studentId)
    {
        if (studentId.IsInvalid()) throw new HandledException(ExceptionConstants.INVALID_CONTRACT);

        Student student = await GetStudentById(studentId);

        student.SetExcluded();

        return await _studentRepository.ExcludedStudentAsync(student);

    }

    public async Task<StudentDto[]> GetActiveStudentsNotInCourseAsync(Guid courseId)
    {
        if (courseId.IsInvalid()) throw new HandledException(ExceptionConstants.INVALID_CONTRACT);

        Student[] student = await _studentRepository.GetAllActiveStudentsNotInCourseAsync(courseId);

        return student.ToArrayStudentDto();
    }

    private async Task<Student> GetStudentById(Guid studentId)
    {
        Student student = await _studentRepository.GetByIdAsync(studentId);

        if (student is null) throw new HandledException(ExceptionConstants.STUDENT_NOT_FOUND);

        return student;
    }
}