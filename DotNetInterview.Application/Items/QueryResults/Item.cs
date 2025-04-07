namespace DotNetInterview.Application.Items.QueryResults;

using DotNetInterview.Application.Items.ViewModels;

public record class Item
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Reference { get; init; }
    public required decimal Price { get; init; }
    public required string Status { get; init; }
    public required decimal? CurrentPrice { get; init; }
    public required int TotalQuantity { get; init; }
    public required int? Discount { get; init; }
    public List<Variation> Variations { get; init; } = new List<Variation>();
}
