namespace DotNetInterview.Application.Items.QueryHandlers;

using DotNetInterview.Application.Items.Interfaces;
using DotNetInterview.Application.Items.Queries;
using DotNetInterview.Application.Items.QueryResults;
using DotNetInterview.Application.Items.ViewModels;

internal sealed class GetItemWithVariationsByIdHandler : IRequestHandler<GetItemWithVariationsById, Item?>
{
    private readonly ILogger<GetItemWithVariationsByIdHandler> logger;
    private readonly IItemProcessManager processManager;
    private readonly IItemReadService readService;

    public GetItemWithVariationsByIdHandler(ILogger<GetItemWithVariationsByIdHandler> logger, IItemProcessManager processManager, IItemReadService readService)
    {
        this.logger = logger;
        this.processManager = processManager;
        this.readService = readService;
    }

    public async Task<Item?> Handle(GetItemWithVariationsById request, CancellationToken cancellationToken)
    {
        var itemReadModel = await this.readService.GetItemWithVariationsById(request.Id, cancellationToken);

        if (itemReadModel is null)
        {
            return null;
        }

        var item = this.processManager.Process(itemReadModel);

        foreach (var variationReadModel in itemReadModel.Variations)
        {
            item.Variations.Add((Variation)variationReadModel);
        }

        return item;
    }
}
