namespace DotNetInterview.Application.Items.CommandHandlers;

using DotNetInterview.Application.Items.Commands;
using DotNetInterview.Domain.Interfaces;

internal sealed class DeleteItemHandler : IRequestHandler<DeleteItem>
{
    private readonly ILogger<DeleteItemHandler> logger;
    private readonly IItemRepository repository;

    public DeleteItemHandler(ILogger<DeleteItemHandler> logger, IItemRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task Handle(DeleteItem request, CancellationToken cancellationToken)
    {
        await this.repository.DeleteAsync(request.Id, cancellationToken);
    }
}
