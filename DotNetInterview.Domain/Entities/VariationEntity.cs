namespace DotNetInterview.Domain.Entities;

public sealed record class VariationEntity
{
    public required Guid Id { get; init; }
    public required String Size { get; set; }
    public required int Quantity { get; set; }
}
