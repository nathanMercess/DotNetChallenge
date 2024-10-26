namespace DotNetChallenge.Web.Models;

public class CommonApiResponse<T>
{
    public int StatusCode { get; set; }
    public T Data { get; set; }
    public string Error { get; set; }

    public static CommonApiResponse<T> Success(T data, int statusCode = 200)
    {
        return new CommonApiResponse<T>
        {
            StatusCode = statusCode,
            Data = data,
            Error = null
        };
    }

    public static CommonApiResponse<T> Failure(string error, int statusCode = 500)
    {
        return new CommonApiResponse<T>
        {
            StatusCode = statusCode,
            Data = default,
            Error = error
        };
    }
}
