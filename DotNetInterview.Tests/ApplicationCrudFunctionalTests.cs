namespace DotNetInterview.Tests;

using System.Diagnostics.CodeAnalysis;
using DotNetInterview.Application;
using DotNetInterview.Application.Items.Commands;
using DotNetInterview.Application.Items.Queries;
using DotNetInterview.Application.Items.ViewModels;
using DotNetInterview.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Shouldly;

[ExcludeFromCodeCoverage]
public sealed class ApplicationCrudFunctionalTests
{
    private readonly ServiceProvider provider;
    private static readonly DateTimeOffset mondayBefore12 = new(new DateTime(2025, 4, 7, 11, 59, 59, DateTimeKind.Utc));

    public ApplicationCrudFunctionalTests()
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
        services.AddSingleton<TimeProvider>(new FakeTimeProvider(mondayBefore12));

        this.provider = services.BuildServiceProvider();
    }

    [Test]
    public async Task CreateItem_Should_Add_Item_Without_Variations()
    {
        // Given
        var mediator = this.provider.GetRequiredService<IMediator>();

        var id = Guid.NewGuid();

        var command = new CreateItem
        {
            Id = id,
            Name = "Test",
            Reference = "Test",
            Price = 10,
        };

        // When
        await mediator.Send(command, CancellationToken.None);

        var query = new GetItemWithVariationsById
        {
            Id = id,
        };

        var result = await mediator.Send(query, CancellationToken.None);

        // Then
        result.ShouldNotBeNull();
        result.CurrentPrice.ShouldBeNull();
        result.Discount.ShouldBeNull();
        result.Name.ShouldBe("Test");
        result.Price.ShouldBe(10);
        result.Reference.ShouldBe("Test");
        result.Status.ShouldBe("Sold Out");
        result.TotalQuantity.ShouldBe(0);
        result.Variations.ShouldBeEmpty();
    }

    //[Test]
    public async Task CreateItem_Should_Add_Item_With_Variations()
    {
        // Given
        var mediator = this.provider.GetRequiredService<IMediator>();

        var id = Guid.NewGuid();

        var variation = new Variation
        {
            Id = Guid.NewGuid(),
            Size = "Test",
            Quantity = 1,
        };

        var createItemCommand = new CreateItem
        {
            Id = id,
            Name = "Test",
            Reference = "Test",
            Price = 10,
            Variations = new List<Variation>
            {
                variation,
            }
        };

        // When
        await mediator.Send(createItemCommand, CancellationToken.None);

        var query = new GetItemWithVariationsById
        {
            Id = id,
        };

        var result = await mediator.Send(query, CancellationToken.None);

        // Then
        result.ShouldNotBeNull();
        result.CurrentPrice.ShouldNotBeNull();
        result.CurrentPrice.Value.ShouldBe(10);
        result.Discount.ShouldBeNull();
        result.Name.ShouldBe("Test");
        result.Price.ShouldBe(10);
        result.Reference.ShouldBe("Test");
        result.Status.ShouldBe("In Stock");
        result.TotalQuantity.ShouldBe(1);
        result.Variations.ShouldNotBeEmpty();
        result.Variations.Count.ShouldBe(1);

        result.Variations[0].Size.ShouldBe("Test");
        result.Variations[0].Quantity.ShouldBe(1);
    }

    //[Test]
    public async Task CreateItem_And_CreateItemVariation_Should_Add_Item_With_Variations()
    {
        // Given
        var mediator = this.provider.GetRequiredService<IMediator>();

        var id = Guid.Parse("bc99b883-b495-4d74-b1d0-ac196e03b627");

        var createItemCommand = new CreateItem
        {
            Id = id,
            Name = "Test",
            Reference = "Test",
            Price = 10
        };

        await mediator.Send(createItemCommand, CancellationToken.None);

        // When
        var variation = new Variation
        {
            Id = Guid.NewGuid(),
            Size = "Test",
            Quantity = 1,
        };

        var createItemVariation = new CreateItemVariation
        {
            Id = id,
            Variations = new List<Variation>
            {
                variation,
            }
        };

        await mediator.Send(createItemVariation, CancellationToken.None);

        var query = new GetItemWithVariationsById
        {
            Id = id,
        };

        var result = await mediator.Send(query, CancellationToken.None);

        // Then
        result.ShouldNotBeNull();
        //result.CurrentPrice.ShouldNotBeNull();
        //result.CurrentPrice.Value.ShouldBe(10);
        result.Discount.ShouldBeNull();
        result.Name.ShouldBe("Test");
        result.Price.ShouldBe(10);
        result.Reference.ShouldBe("Test");
        result.Status.ShouldBe("In Stock");
        result.TotalQuantity.ShouldBe(1);
        result.Variations.ShouldNotBeEmpty();
        result.Variations.Count.ShouldBe(1);

        result.Variations[0].Size.ShouldBe("Test");
        result.Variations[0].Quantity.ShouldBe(1);
    }
}
