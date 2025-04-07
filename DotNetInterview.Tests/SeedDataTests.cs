namespace DotNetInterview.Tests;

using DotNetInterview.Infrastructure.EntityFramework.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class SeedDataTests
{
    private DataContext _dataContext;

    [SetUp]
    public void Setup()
    {
        var fileName = $"C:\\temp\\_SQLite_\\{Guid.NewGuid()}.sqlite";

        var connection = new SqliteConnection($"Data Source={fileName};Cache=Shared");
        connection.Open();
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite(connection)
            .Options;
        _dataContext = new DataContext(options);
    }

    //[Test]
    public void Example_to_ensure_dbcontext_has_seed_data()
    {
        Assert.That(_dataContext.Items.Count(), Is.EqualTo(3));
    }
}
