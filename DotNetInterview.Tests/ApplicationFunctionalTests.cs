namespace DotNetInterview.Tests;

using DotNetInterview.Application;
using DotNetInterview.Application.Items.Queries;
using DotNetInterview.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;

public sealed class ApplicationFunctionalTests
{
    private readonly ServiceProvider provider;
    private static readonly DateTimeOffset MondayBefore12 = new(new DateTime(2025, 4, 7, 11, 59, 59, DateTimeKind.Utc));

    public ApplicationFunctionalTests()
    {
        var inMemoryCollection = new Dictionary<string, string?>()
        {
            { "ConnectionStrings__DefaultConnection", "Data Source=DotNetInterview;Mode=Memory;Cache=Shared" },
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemoryCollection)
            .Build();

        var services = new ServiceCollection();
        services.AddApplication();
        services.AddInfrastructure(configuration);
        services.AddLogging(setup => setup.AddDebug());
        services.AddSingleton<TimeProvider>(new FakeTimeProvider(MondayBefore12));

        this.provider = services.BuildServiceProvider();
    }

    [Test]
    public async Task Get_All_Items_Result()
    {
        // Given
        var mediator = this.provider.GetRequiredService<IMediator>();
        var query = new GetAllItems { };

        // When
        var result = await mediator.Send(query, CancellationToken.None);

        // Then
        result.ShouldNotBeNull();
        result.Count.ShouldBe(3);

        var shorts = result.SingleOrDefault(i => i.Name == "Shorts");
        shorts.ShouldNotBeNull();
        shorts.CurrentPrice.ShouldNotBeNull();
        shorts.CurrentPrice.Value.ShouldBe(31.50m);
        shorts.Discount.ShouldNotBeNull();
        shorts.Discount.Value.ShouldBe(10);
        shorts.Price.ShouldBe(35);
        shorts.Reference.ShouldBe("A123");
        shorts.Status.ShouldBe("In Stock");
        shorts.TotalQuantity.ShouldBe(10);

        var tie = result.SingleOrDefault(i => i.Name == "Tie");
        tie.ShouldNotBeNull();
        tie.CurrentPrice.ShouldBeNull();
        tie.Discount.ShouldBeNull();
        tie.Price.ShouldBe(15);
        tie.Reference.ShouldBe("B456");
        tie.Status.ShouldBe("Sold Out");
        tie.TotalQuantity.ShouldBe(0);

        var shoes = result.SingleOrDefault(i => i.Name == "Shoes");
        shoes.ShouldNotBeNull();
        shoes.CurrentPrice.ShouldNotBeNull();
        shoes.CurrentPrice.Value.ShouldBe(56m);
        shoes.Discount.ShouldNotBeNull();
        shoes.Discount.Value.ShouldBe(20);
        shoes.Price.ShouldBe(70);
        shoes.Reference.ShouldBe("C789");
        shoes.Status.ShouldBe("In Stock");
        shoes.TotalQuantity.ShouldBe(15);
    }
}
