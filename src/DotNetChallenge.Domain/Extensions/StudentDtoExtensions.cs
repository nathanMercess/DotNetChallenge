using DotNetChallenge.Crosscutting.Dtos;
using DotNetChallenge.Domain.Entities;

namespace DotNetChallenge.Domain.Extensions;

public static class StudentDtoExtensions
{
    public static Student ToStudentDto(this StudentDto student)
        => new Student(student.Name, student.User, student.Password);
}
