namespace DotNetInterview.Application.Items.Queries;

using DotNetInterview.Application.Items.QueryResults;

public sealed record class GetAllItems : IRequest<List<Item>>
{
}
