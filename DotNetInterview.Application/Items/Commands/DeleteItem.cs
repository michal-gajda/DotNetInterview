namespace DotNetInterview.Application.Items.Commands;

using MediatR;

public sealed record class DeleteItem : IRequest
{
    public required Guid Id { get; init; }
}
