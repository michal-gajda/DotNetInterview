namespace DotNetInterview.Application.Items.Commands;

using DotNetInterview.Application.Items.ViewModels;

public sealed record class CreateItem : IRequest
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Reference { get; init; }
    public required decimal Price { get; init; }
    public IReadOnlyList<Variation> Variations { get; init; } = new List<Variation>();
}
