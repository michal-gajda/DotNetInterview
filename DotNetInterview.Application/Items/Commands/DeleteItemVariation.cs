namespace DotNetInterview.Application.Items.Commands;

public sealed record class DeleteItemVariation : IRequest
{
    public required Guid Id { get; init; }
    public IReadOnlyList<Guid> VariationIds { get; init; } = new List<Guid>();
}
