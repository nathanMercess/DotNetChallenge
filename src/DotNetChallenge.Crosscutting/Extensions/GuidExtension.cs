
namespace DotNetChallenge.Crosscutting.Extensions;

public static class GuidExtension
{
    public static bool IsValid(this Guid value) => value != Guid.Empty;

    public static bool IsInvalid(this Guid value) => !value.IsValid();
}
