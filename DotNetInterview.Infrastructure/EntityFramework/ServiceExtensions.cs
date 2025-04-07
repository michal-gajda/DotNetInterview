namespace DotNetInterview.Infrastructure.EntityFramework;

using DotNetInterview.Domain.Interfaces;
using DotNetInterview.Infrastructure.EntityFramework.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal static class ServiceExtensions
{
    public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = new SqliteConnection(configuration.GetConnectionString("DefaultConnection"));
        connection.Open();
        services.AddDbContext<DataContext>(options => options.UseSqlite(connection));
        services.AddScoped<IItemRepository, ItemRepository>();

        return services;
    }
}
