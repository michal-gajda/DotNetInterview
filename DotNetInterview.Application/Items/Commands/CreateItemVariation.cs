namespace DotNetInterview.Application.Items.Commands;

using DotNetInterview.Application.Items.ViewModels;

public sealed record class CreateItemVariation : IRequest
{
    [JsonIgnore]
    public required Guid Id { get; init; }
    public IReadOnlyList<Variation> Variations { get; init; } = new List<Variation>();
}
