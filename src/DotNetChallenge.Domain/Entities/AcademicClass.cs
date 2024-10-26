using DotNetChallenge.Crosscutting.Constants;
using DotNetChallenge.Crosscutting.Helpers;

namespace DotNetChallenge.Domain.Entities;

public sealed class AcademicClass : Entity
{
    public Guid CourseId { get; }

    public string ClassName { get; private set; }

    public int Year { get; private set; }

    public AcademicClass() { }

    public AcademicClass(Guid courseId, string className, int year)
    {
        DomainValidationHelper.IsNotEmpty(courseId, AcademicClassDomainErrorsConstant.INVALID_COURSE_ID);
        SetAcademicClass(className, year);

        CourseId = courseId;
    }

    public void SetAcademicClass(string className, int year)
    {
        DomainValidationHelper.IsEmptyOrLessOrEqualsThan(className, AcademicClassConstants.ACADEMIC_CLASS_NAME_MAX_LENGTH, AcademicClassDomainErrorsConstant.INVALID_CLASS_NAME);
        DomainValidationHelper.IsNotNegative(year, AcademicClassDomainErrorsConstant.INVALID_ACADEMIC_CLASS_YEAR);

        ClassName = className;
        Year = year;
    }
}
