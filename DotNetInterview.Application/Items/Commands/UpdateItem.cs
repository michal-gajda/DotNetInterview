namespace DotNetInterview.Application.Items.Commands;

using DotNetInterview.Application.Items.ViewModels;

public sealed record class UpdateItem : IRequest
{
    [JsonIgnore]
    public Guid Id { get; set; } = Guid.Empty;
    public required string Name { get; init; }
    public required string Reference { get; init; }
    public required decimal Price { get; init; }
    public IReadOnlyList<Variation> Variations { get; init; } = new List<Variation>();
}
