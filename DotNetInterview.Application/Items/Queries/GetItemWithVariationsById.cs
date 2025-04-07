namespace DotNetInterview.Application.Items.Queries;

using DotNetInterview.Application.Items.QueryResults;

public sealed record class GetItemWithVariationsById : IRequest<Item?>
{
    [JsonIgnore]
    public required Guid Id { get; init; }
}
