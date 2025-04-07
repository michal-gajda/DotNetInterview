namespace DotNetInterview.Application.Items.CommandHandlers;

using DotNetInterview.Application.Items.Commands;
using DotNetInterview.Application.Items.Exceptions;
using DotNetInterview.Domain.Interfaces;

internal sealed class DeleteItemVariationHandler : IRequestHandler<DeleteItemVariation>
{
    private readonly ILogger<DeleteItemVariationHandler> logger;
    private readonly IItemRepository repository;

    public DeleteItemVariationHandler(ILogger<DeleteItemVariationHandler> logger, IItemRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task Handle(DeleteItemVariation request, CancellationToken cancellationToken)
    {
        var item = await repository.LoadAsync(request.Id, cancellationToken);

        if (item is not null)
        {
            foreach (var id in request.VariationIds)
            {
                item.DeleteVariation(id);
            }

            await this.repository.SaveAsync(item, cancellationToken);
        }
        else
        {
            throw new ItemNotFoundException(request.Id);
        }
    }
}
