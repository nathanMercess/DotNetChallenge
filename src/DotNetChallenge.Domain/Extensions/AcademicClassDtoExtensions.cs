using DotNetChallenge.Crosscutting.Dtos;
using DotNetChallenge.Domain.Entities;

namespace DotNetChallenge.Domain.Extensions;

public static class AcademicClassDtoExtensions
{
    public static AcademicClass ToAcademicClass(this AcademicClassDto academicClassDto)
        => new AcademicClass(academicClassDto.ClassName, academicClassDto.Year);
}
