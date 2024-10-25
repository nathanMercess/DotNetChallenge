using DotNetChallenge.Crosscutting.Constants;
using DotNetChallenge.Crosscutting.Helpers;

namespace DotNetChallenge.Domain.Entities;

public sealed class Student : Entity
{
    public string Name { get; private set; }

    public string User { get; private set; }

    public string Password { get; private set; }

    public Student(string name, string user, string password)
    {
        SetBasicInformation(name, user);
        SetPassword(password);
    }

    public void SetBasicInformation(string name, string user)
    {
        DomainValidationHelper.IsEmptyOrLessOrEqualsThan(name, StudentConstants.STUDANT_NAME_MAX_LENGTH, StudantDomainErrorsConstant.INVALID_STUDANT_NAME);
        DomainValidationHelper.IsEmptyOrLessOrEqualsThan(user, StudentConstants.STUDANT_USER_MAX_LENGTH, StudantDomainErrorsConstant.INVALID_STUDANT_USER);

        Name = name;
        User = user;
    }

    public void SetPassword(string password)
    {
        DomainValidationHelper.IsNotNull(Password, nameof(Password));
        Password = password;
    }
}