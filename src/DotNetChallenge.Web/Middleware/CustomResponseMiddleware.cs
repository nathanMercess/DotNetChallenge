using DotNetChallenge.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace DotNetChallenge.Web.Middleware;

public class CustomResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomResponseMiddleware> _logger;

    public CustomResponseMiddleware(RequestDelegate next, ILogger<CustomResponseMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;
        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        try
        {
            await _next(context);

            if (context.Response.ContentType?.Contains("application/json") == true)
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                var responseContent = await new StreamReader(memoryStream).ReadToEndAsync();

                var responseObject = JsonConvert.DeserializeObject<object>(responseContent);
                var result = CommonApiResponse<object>.Success(responseObject, context.Response.StatusCode);

                context.Response.Body = originalBodyStream;
                await WriteResponseAsync(context, result);
            }
            else
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex, originalBodyStream);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, Stream originalBodyStream)
    {
        context.Response.Body = originalBodyStream;
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorResponse = CommonApiResponse<object>.Failure(ex.Message, context.Response.StatusCode);

        await WriteResponseAsync(context, errorResponse);
    }

    private async Task WriteResponseAsync<T>(HttpContext context, CommonApiResponse<T> response)
    {
        context.Response.ContentType = "application/json";
        var jsonResponse = JsonConvert.SerializeObject(response, Formatting.Indented);
        await context.Response.WriteAsync(jsonResponse);
    }
}
