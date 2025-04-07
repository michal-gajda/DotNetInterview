namespace DotNetInterview.Application.Items.CommandHandlers;

using DotNetInterview.Application.Items.Commands;
using DotNetInterview.Application.Items.Exceptions;
using DotNetInterview.Domain.Interfaces;

internal sealed class UpdateItemHandler : IRequestHandler<UpdateItem>
{
    private readonly ILogger<UpdateItemHandler> logger;
    private readonly IItemRepository repository;

    public UpdateItemHandler(ILogger<UpdateItemHandler> logger, IItemRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task Handle(UpdateItem request, CancellationToken cancellationToken)
    {
        var item = await repository.LoadAsync(request.Id, cancellationToken);

        if (item is not null)
        {
            item.SetName(request.Name);
            item.SetPrice(request.Price);
            item.SetReference(request.Reference);
            item.SetPrice(request.Price);

            foreach (var variation in request.Variations)
            {
                item.UpdateVariation(variation.Id, variation.Size, variation.Quantity);
            }

            await this.repository.SaveAsync(item, cancellationToken);
        }
        else
        {
            throw new ItemNotFoundException(request.Id);
        }
    }
}
