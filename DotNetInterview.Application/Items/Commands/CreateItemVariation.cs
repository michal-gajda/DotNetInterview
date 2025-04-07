namespace DotNetInterview.Application.Items.Commands;

using DotNetInterview.Application.Items.ViewModels;

public sealed record class CreateItemVariation : IRequest
{
    [JsonIgnore]
    public Guid Id { get; set; } = Guid.Empty;
    public IReadOnlyList<Variation> Variations { get; init; } = new List<Variation>();
}
