using DotNetChallenge.Crosscutting.Constants;
using DotNetChallenge.Crosscutting.Helpers;

namespace DotNetChallenge.Domain.Entities;

public sealed class AcademicClass : Entity
{
    private readonly List<Student> _studentsList = new List<Student>();

    public Guid CourseId { get; }

    public string ClassName { get; private set; }

    public int Year { get; private set; }

    public IReadOnlyCollection<Student> Students => _studentsList.AsReadOnly();

    public AcademicClass() { }

    public AcademicClass(string className, int year)
    {
        SetAcademicClass(className, year);
        CourseId = Guid.NewGuid();
    }

    public void SetAcademicClass(string className, int year)
    {
        DomainValidationHelper.IsEmptyOrLessOrEqualsThan(className, AcademicClassConstants.ACADEMIC_CLASS_NAME_MAX_LENGTH, AcademicClassDomainErrorsConstant.INVALID_CLASS_NAME);
        DomainValidationHelper.IsNotNegative(year, AcademicClassDomainErrorsConstant.INVALID_ACADEMIC_CLASS_YEAR);

        ClassName = className;
        Year = year;
    }

    public void AddStudent(Student student)
    {
        if (student != null && !_studentsList.Contains(student))
        {
            _studentsList.Add(student);
        }
    }
}
