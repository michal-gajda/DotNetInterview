namespace DotNetInterview.Application.Items.CommandHandlers;

using DotNetInterview.Application.Items.Commands;
using DotNetInterview.Domain.Interfaces;

internal sealed class UpdateItemVariationHandler : IRequestHandler<UpdateItemVariation>
{
    private readonly ILogger<UpdateItemVariationHandler> logger;
    private readonly IItemRepository repository;

    public UpdateItemVariationHandler(ILogger<UpdateItemVariationHandler> logger, IItemRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task Handle(UpdateItemVariation request, CancellationToken cancellationToken)
    {
        var item = await repository.LoadAsync(request.Id, cancellationToken);

        if (item is not null)
        {
            foreach (var variation in request.Variations)
            {
                item.UpdateVariation(variation.Id, variation.Size, variation.Quantity);
            }

            await this.repository.SaveAsync(item, cancellationToken);
        }
    }
}
