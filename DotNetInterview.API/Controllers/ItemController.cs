namespace DotNetInterview.API.Controllers;

using DotNetInterview.Application.Items.Commands;
using DotNetInterview.Application.Items.Queries;
using DotNetInterview.Application.Items.QueryResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("")]
public sealed class ItemController : ControllerBase
{
    private readonly ILogger<ItemController> logger;
    private readonly IMediator mediator;

    public ItemController(ILogger<ItemController> logger, IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    [HttpGet(Name = "Items")]
    public async Task<IEnumerable<Item>> GetAllItems(CancellationToken cancellationToken = default) =>
        await mediator.Send(new GetAllItems(), cancellationToken);

    [HttpGet("Item/{id:guid}")]
    public async Task<Item?> GetItem(Guid id, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetItemWithVariationsById { Id = id, }, cancellationToken);
    }

    [HttpPost("Items")]
    public async Task CreateItem([FromBody] CreateItem command, CancellationToken cancellationToken = default)
        => await mediator.Send(command, cancellationToken);

    [HttpDelete("Item/{id:guid}")]
    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
        => await mediator.Send(new DeleteItem { Id = id, }, cancellationToken);

    [HttpPut("Item/{id:guid}")]
    public async Task Put([FromBody] UpdateItem item, Guid id, CancellationToken cancellationToken = default)
        => await mediator.Send(item with { Id = id, }, cancellationToken);

    [HttpPost("CreateVariationsForItem/{id:guid}")]
    public async Task CreateItemVariation([FromBody] CreateItemVariation command, Guid id, CancellationToken cancellationToken = default)
        => await mediator.Send(command with { Id = id, }, cancellationToken);

    [HttpPut("UpdateVariationsForItem/{id:guid}")]
    public async Task CreateItemVariation([FromBody] UpdateItemVariation command, Guid id, CancellationToken cancellationToken = default)
        => await mediator.Send(command with { Id = id, }, cancellationToken);

    [HttpDelete("DeleteVariationsForItem/{id:guid}")]
    public async Task DeleteItemVariation([FromBody] DeleteItemVariation command, Guid id, CancellationToken cancellationToken = default)
        => await mediator.Send(command with { Id = id, }, cancellationToken);
}
