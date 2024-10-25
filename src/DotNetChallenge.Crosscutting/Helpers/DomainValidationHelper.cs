using DotNetChallenge.Crosscutting.Constants;
using DotNetChallenge.Crosscutting.Exceptions;
using DotNetChallenge.Crosscutting.Extensions;

namespace DotNetChallenge.Crosscutting.Helpers;

public static class DomainValidationHelper
{
    public static void IsNotEmpty(Guid guid, string errorCode)
    {
        if (guid.IsInvalid())
            ThrowHandledException(errorCode);
    }

    public static void IsEmptyOrLessOrEqualsThan(string value, int maxLength, string errorCode)
    {
        if (!string.IsNullOrWhiteSpace(value) && value.Length > maxLength)
            ThrowHandledException(errorCode);
    }

    public static void IsNotNegative(int value, string errorCode)
    {
        if (value < NumberConstants.ZERO)
            ThrowHandledException(errorCode);
    }

    public static void ThrowHandledException(string errorCode) => throw new HandledException(errorCode);
}
