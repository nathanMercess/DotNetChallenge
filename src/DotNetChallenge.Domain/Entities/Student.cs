using DotNetChallenge.Crosscutting.Constants;
using DotNetChallenge.Crosscutting.Helpers;

namespace DotNetChallenge.Domain.Entities;

public sealed class Student : Entity
{
    private readonly List<AcademicClass> _classList = new List<AcademicClass>();

    public string Name { get; private set; }

    public string User { get; private set; }

    public string Password { get; private set; }

    public IReadOnlyCollection<AcademicClass> AcademicClasses => _classList.AsReadOnly();

    public Student() { }

    public Student(string name, string user, string password)
    {
        SetBasicInformation(name, user);
        SetPassword(password);
    }

    public void SetBasicInformation(string name, string user)
    {
        DomainValidationHelper.IsEmptyOrLessOrEqualsThan(name, StudentConstants.STUDENT_NAME_MAX_LENGTH, StudentDomainErrorsConstant.INVALID_STUDENT_NAME);
        DomainValidationHelper.IsEmptyOrLessOrEqualsThan(user, StudentConstants.STUDENT_USER_MAX_LENGTH, StudentDomainErrorsConstant.INVALID_STUDENT_USER);

        Name = name;
        User = user;
    }

    public void SetPassword(string password)
    {
        DomainValidationHelper.IsNotNull(password, StudentDomainErrorsConstant.INVALID_STUDENT_PASSWORD);
        Password = password;
    }

    public void AddAcademicClass(AcademicClass academicClass) => _classList.Add(academicClass);
}