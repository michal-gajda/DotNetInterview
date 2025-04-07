namespace DotNetInterview.Application.Items.Commands;

public sealed record class DeleteItemVariation : IRequest
{
    [JsonIgnore]
    public Guid Id { get; set; } = Guid.Empty;
    public IReadOnlyList<Guid> VariationIds { get; init; } = new List<Guid>();
}
