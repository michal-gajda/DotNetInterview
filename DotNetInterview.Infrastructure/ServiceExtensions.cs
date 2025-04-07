namespace DotNetInterview.Infrastructure;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DotNetInterview.Infrastructure.Dapper;
using DotNetInterview.Infrastructure.EntityFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(setup => setup.RegisterServicesFromAssembly(assembly));

        services.AddEntityFramework(configuration);
        services.AddDapper();

        return services;
    }
}
