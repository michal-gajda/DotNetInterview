#nullable disable
namespace DotNetInterview.Infrastructure.EntityFramework.Services;

using DotNetInterview.Infrastructure.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

public sealed class DataContext : DbContext
{
    public DbSet<Item> Items { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        _ = Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SeedData.Load(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}
