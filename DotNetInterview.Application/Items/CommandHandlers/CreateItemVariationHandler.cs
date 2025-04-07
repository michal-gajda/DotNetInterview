namespace DotNetInterview.Application.Items.CommandHandlers;

using DotNetInterview.Application.Items.Commands;
using DotNetInterview.Domain.Interfaces;

internal sealed class CreateItemVariationHandler : IRequestHandler<CreateItemVariation>
{
    private readonly ILogger<CreateItemVariationHandler> logger;
    private readonly IItemRepository repository;

    public CreateItemVariationHandler(ILogger<CreateItemVariationHandler> logger, IItemRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task Handle(CreateItemVariation request, CancellationToken cancellationToken)
    {
        var item = await repository.LoadAsync(request.Id, cancellationToken);

        if (item is not null)
        {
            foreach (var variation in request.Variations)
            {
                item.CreateVariation(variation.Id, variation.Size, variation.Quantity);
            }

            await this.repository.SaveAsync(item, cancellationToken);
        }
    }
}
