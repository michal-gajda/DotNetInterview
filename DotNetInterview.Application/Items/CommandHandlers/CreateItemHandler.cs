namespace DotNetInterview.Application.Items.CommandHandlers;

using DotNetInterview.Application.Items.Commands;
using DotNetInterview.Domain.Entities;
using DotNetInterview.Domain.Interfaces;

internal sealed class CreateItemHandler : IRequestHandler<CreateItem>
{
    private readonly ILogger<CreateItemHandler> logger;
    private readonly IItemRepository repository;

    public CreateItemHandler(ILogger<CreateItemHandler> logger, IItemRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task Handle(CreateItem request, CancellationToken cancellationToken)
    {
        var item = new ItemEntity(request.Id, request.Reference, request.Name, request.Price);

        foreach (var variation in request.Variations)
        {
            item.CreateVariation(variation.Id, variation.Size, variation.Quantity);
        }

        await this.repository.SaveAsync(item, cancellationToken);
    }
}
