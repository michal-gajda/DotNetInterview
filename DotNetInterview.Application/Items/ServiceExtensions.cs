namespace DotNetInterview.Application.Items;

using System.Diagnostics.CodeAnalysis;
using DotNetInterview.Application.Items.Interfaces;
using DotNetInterview.Application.Items.Services;
using Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
internal static class ServiceExtensions
{
    public static IServiceCollection AddItems(this IServiceCollection services)
    {
        services.AddScoped<IItemProcessManager, ItemProcessManager>();

        return services;
    }
}
