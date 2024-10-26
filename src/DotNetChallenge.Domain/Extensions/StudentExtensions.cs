using DotNetChallenge.Crosscutting.Dtos;
using DotNetChallenge.Domain.Entities;

namespace DotNetChallenge.Domain.Extensions;

public static class StudentExtensions
{
    public static StudentDto[] ToArrayStudentDto(this Student[] students)
        => students.Select(student => student.ToStudentDto()).ToArray();

    public static StudentDto ToStudentDto(this Student student)
        => new StudentDto(student.Id, student.Name, student.User, student.AcademicClasses.ToArrayToAcademicClassDto()) { };
}
