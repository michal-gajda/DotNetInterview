namespace DotNetInterview.Application.Items.ReadModels;

public sealed record class ItemReadModel
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Reference { get; init; }
    public required decimal Price { get; init; }
    public required int TotalQuantity { get; init; }

    public IReadOnlyList<VariationReadModel> Variations { get; init; } = new List<VariationReadModel>();
}
