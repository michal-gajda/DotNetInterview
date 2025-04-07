namespace DotNetInterview.Application.Items.Commands;

using DotNetInterview.Application.Items.ViewModels;

public sealed record class UpdateItemVariation : IRequest
{
    public required Guid Id { get; init; }
    public IReadOnlyList<Variation> Variations { get; init; } = new List<Variation>();
}
