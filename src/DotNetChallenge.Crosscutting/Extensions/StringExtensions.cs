using System.Text.Json;

namespace DotNetChallenge.Crosscutting.Extensions;

public static class StringExtensions
{
    public static bool IsValidJson(this string strInput)
    {
        return strInput.ToJsonDocument() != null;
    }

    private static JsonDocument ToJsonDocument(this string strInput)
    {
        if (string.IsNullOrEmpty(strInput))
        {
            return null;
        }

        strInput = strInput.Trim();
        if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || (strInput.StartsWith("[") && strInput.EndsWith("]")))
        {
            try
            {
                return JsonDocument.Parse(strInput);
            }
            catch (JsonException value)
            {
                Console.WriteLine(value);
                return null;
            }
            catch (Exception value2)
            {
                Console.WriteLine(value2);
                return null;
            }
        }

        return null;
    }
}
