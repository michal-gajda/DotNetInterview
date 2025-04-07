namespace DotNetInterview.Application.Items.ViewModels;

public sealed record class Variation
{
    public required Guid Id { get; init; }
    public required string Size { get; init; }
    public required int Quantity { get; init; }
}
