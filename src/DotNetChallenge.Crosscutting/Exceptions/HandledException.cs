using System.Net;

namespace DotNetChallenge.Crosscutting.Exceptions;

public sealed class HandledException : Exception
{
    public HttpStatusCode StatusCode { get; private set; }
    
    public HandledException(string message, HttpStatusCode statusCode = HttpStatusCode.PreconditionFailed) : base(message)
    {
        StatusCode = statusCode;
    }
}
