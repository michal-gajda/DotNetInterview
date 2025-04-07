namespace DotNetInterview.Application.Items.QueryHandlers;

using DotNetInterview.Application.Items.Interfaces;
using DotNetInterview.Application.Items.Queries;
using DotNetInterview.Application.Items.QueryResults;
using DotNetInterview.Application.Items.ReadModels;
using DotNetInterview.Application.Items.ViewModels;

internal sealed class GetAllItemsHandler : IRequestHandler<GetAllItems, List<Item>>
{
    private readonly ILogger<GetAllItemsHandler> logger;
    private readonly IItemProcessManager processManager;
    private readonly IItemReadService readService;

    public GetAllItemsHandler(ILogger<GetAllItemsHandler> logger, IItemProcessManager processManager, IItemReadService readService)
    {
        this.logger = logger;
        this.processManager = processManager;
        this.readService = readService;
    }

    public async Task<List<Item>> Handle(GetAllItems request, CancellationToken cancellationToken)
    {
        var items = new List<Item>();

        var itemReadModels = await this.readService.GetAllItemsAsync(cancellationToken);

        foreach (var itemReadModel in itemReadModels)
        {
            var item = this.processManager.Process(itemReadModel);

            foreach (VariationReadModel variationReadModel in itemReadModel.Variations)
            {
                item.Variations.Add((Variation)variationReadModel);
            }

            items.Add(item);
        }

        return items;
    }
}
