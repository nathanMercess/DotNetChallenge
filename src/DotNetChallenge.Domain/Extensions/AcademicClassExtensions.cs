using DotNetChallenge.Crosscutting.Dtos;
using DotNetChallenge.Domain.Entities;

namespace DotNetChallenge.Domain.Extensions;

public static class AcademicClassExtensions
{
    public static AcademicClassDto[] ToArrayToAcademicClassDto(this IReadOnlyCollection<AcademicClass> academicClasses)
        => academicClasses.Select(academic => academic.ToAcademicClassDto()).ToArray();

    public static AcademicClassDto ToAcademicClassDto(this AcademicClass academicClasse)
        => new AcademicClassDto(academicClasse.Id, academicClasse.CourseId, academicClasse.ClassName, academicClasse.Year);
}