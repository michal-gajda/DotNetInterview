namespace DotNetInterview.Tests;

using DotNetInterview.Application;
using DotNetInterview.Application.Items.Queries;
using DotNetInterview.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Shouldly;

public sealed class ApplicationMondayBetween12and5FunctionalTests
{
    private readonly ServiceProvider provider;
    private static readonly DateTimeOffset MondayBetween12and5 = new(new DateTime(2025, 4, 7, 12, 1, 1, DateTimeKind.Utc));

    public ApplicationMondayBetween12and5FunctionalTests()
    {
        var fileName = $"C:\\temp\\_SQLite_\\{Guid.NewGuid()}.sqlite";

        var inMemoryCollection = new Dictionary<string, string?>()
        {
            { "ConnectionStrings__DefaultConnection", $"Data Source={fileName};Cache=Shared" },
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemoryCollection)
            .Build();

        var services = new ServiceCollection();
        services.AddApplication();
        services.AddInfrastructure(configuration);
        services.AddLogging(setup => setup.AddDebug());
        services.AddSingleton<TimeProvider>(new FakeTimeProvider(MondayBetween12and5));

        this.provider = services.BuildServiceProvider();
    }

    //[Test]
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
        shorts.CurrentPrice.Value.ShouldBe(17.50m);
        shorts.Discount.ShouldNotBeNull();
        shorts.Discount.Value.ShouldBe(50);
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
        shoes.CurrentPrice.Value.ShouldBe(35);
        shoes.Discount.ShouldNotBeNull();
        shoes.Discount.Value.ShouldBe(50);
        shoes.Price.ShouldBe(70);
        shoes.Reference.ShouldBe("C789");
        shoes.Status.ShouldBe("In Stock");
        shoes.TotalQuantity.ShouldBe(15);
    }

    //[Test]
    public async Task Get_Result_For_Tie()
    {
        // Given
        var mediator = this.provider.GetRequiredService<IMediator>();
        var items = await mediator.Send(new GetAllItems { }, CancellationToken.None);
        var id = items.First(i => i.Name == "Tie").Id;
        var query = new GetItemWithVariationsById
        {
            Id = id,
        };

        // When
        var tie = await mediator.Send(query, CancellationToken.None);

        // Then
        tie.ShouldNotBeNull();
        tie.CurrentPrice.ShouldBeNull();
        tie.Discount.ShouldBeNull();
        tie.Price.ShouldBe(15);
        tie.Reference.ShouldBe("B456");
        tie.Status.ShouldBe("Sold Out");
        tie.TotalQuantity.ShouldBe(0);
        tie.Variations.ShouldBeEmpty();
    }

    //[Test]
    public async Task Get_Result_For_Shorts()
    {
        // Given
        var mediator = this.provider.GetRequiredService<IMediator>();
        var items = await mediator.Send(new GetAllItems { }, CancellationToken.None);
        var id = items.First(i => i.Name == "Shorts").Id;
        var query = new GetItemWithVariationsById
        {
            Id = id,
        };

        // When
        var shorts = await mediator.Send(query, CancellationToken.None);

        // Then
        shorts.ShouldNotBeNull();
        shorts.CurrentPrice.ShouldNotBeNull();
        shorts.CurrentPrice.Value.ShouldBe(17.50m);
        shorts.Discount.ShouldNotBeNull();
        shorts.Discount.Value.ShouldBe(50);
        shorts.Price.ShouldBe(35);
        shorts.Reference.ShouldBe("A123");
        shorts.Status.ShouldBe("In Stock");
        shorts.TotalQuantity.ShouldBe(10);
        shorts.Variations.ShouldNotBeEmpty();
        shorts.Variations.Count.ShouldBe(3);

        shorts.Variations.SingleOrDefault(variation => "Large".Equals(variation.Size, StringComparison.InvariantCultureIgnoreCase) && variation.Quantity == 3).ShouldNotBeNull();
        shorts.Variations.SingleOrDefault(variation => "Medium".Equals(variation.Size, StringComparison.InvariantCultureIgnoreCase) && variation.Quantity == 0).ShouldNotBeNull();
        shorts.Variations.SingleOrDefault(variation => "Small".Equals(variation.Size, StringComparison.InvariantCultureIgnoreCase) && variation.Quantity == 7).ShouldNotBeNull();
    }

    //[Test]
    public async Task Get_Result_For_Shoes()
    {
        // Given
        var mediator = this.provider.GetRequiredService<IMediator>();
        var items = await mediator.Send(new GetAllItems { }, CancellationToken.None);
        var id = items.First(i => i.Name == "Shoes").Id;
        var query = new GetItemWithVariationsById
        {
            Id = id,
        };

        // When
        var shoes = await mediator.Send(query, CancellationToken.None);

        // Then
        shoes.ShouldNotBeNull();
        shoes.CurrentPrice.ShouldNotBeNull();
        shoes.CurrentPrice.Value.ShouldBe(35);
        shoes.Discount.ShouldNotBeNull();
        shoes.Discount.Value.ShouldBe(50);
        shoes.Price.ShouldBe(70);
        shoes.Reference.ShouldBe("C789");
        shoes.Status.ShouldBe("In Stock");
        shoes.TotalQuantity.ShouldBe(15);
        shoes.Variations.ShouldNotBeEmpty();
        shoes.Variations.Count.ShouldBe(2);

        shoes.Variations.SingleOrDefault(variation => "10".Equals(variation.Size, StringComparison.InvariantCultureIgnoreCase) && variation.Quantity == 8).ShouldNotBeNull();
        shoes.Variations.SingleOrDefault(variation => "9".Equals(variation.Size, StringComparison.InvariantCultureIgnoreCase) && variation.Quantity == 7).ShouldNotBeNull();
    }
}
