using DotNetChallenge.Infra.Contracts.Repositories;
using DotNetChallenge.Infra.Implementations.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetChallenge.Infra.Implementations.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IStudentRepository, StudentRepository>();

        return services;
    }
}
