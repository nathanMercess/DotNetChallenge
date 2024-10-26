using DotNetChallenge.Services.Contracts.Services;
using DotNetChallenge.Services.Implementations.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetChallenge.Services.Implementations.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();

        return services;
    }
}
