namespace DotNetInterview.Infrastructure.Dapper;

using DotNetInterview.Application.Items.Interfaces;
using DotNetInterview.Infrastructure.Dapper.Services;
using DotNetInterview.Infrastructure.Dapper.TypeHandlers;
using global::Dapper;
using Microsoft.Extensions.DependencyInjection;

internal static class ServiceExtensions
{
    public static IServiceCollection AddDapper(this IServiceCollection services)
    {
        SqlMapper.AddTypeHandler(new VariationListTypeHandler());
        SqlMapper.AddTypeHandler(new GuidTypeHandler());

        services.AddScoped<IItemReadService, ItemReadService>();

        return services;
    }
}
