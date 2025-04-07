namespace DotNetInterview.Application;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DotNetInterview.Application.Items;
using Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(setup => setup.RegisterServicesFromAssembly(assembly));

        services.AddItems();

        return services;
    }
}
