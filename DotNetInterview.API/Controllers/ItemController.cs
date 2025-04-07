namespace DotNetInterview.API.Controllers;

using DotNetInterview.Application.Items.Commands;
using DotNetInterview.Application.Items.Queries;
using DotNetInterview.Application.Items.QueryResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("Api")]
public sealed class ItemController : ControllerBase
{
    private readonly ILogger<ItemController> logger;
    private readonly IMediator mediator;

    public ItemController(ILogger<ItemController> logger, IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    [HttpGet("Items")]
    [ProducesResponseType(typeof(IEnumerable<Item>), StatusCodes.Status200OK)]
    public async Task<IEnumerable<Item>> GetAllItems(CancellationToken cancellationToken = default) =>
        await mediator.Send(new GetAllItems(), cancellationToken);

    [HttpGet("Item/{id:guid}")]
    [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Item?>> GetItem(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await mediator.Send(new GetItemWithVariationsById { Id = id, }, cancellationToken);

        if (item is null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPost("Items")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task CreateItem([FromBody] CreateItem command, CancellationToken cancellationToken = default)
        => await mediator.Send(command, cancellationToken);

    [HttpDelete("Item/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
        => await mediator.Send(new DeleteItem { Id = id, }, cancellationToken);

    [HttpPut("Item/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task Put([FromBody] UpdateItem item, Guid id, CancellationToken cancellationToken = default)
        => await mediator.Send(item with { Id = id, }, cancellationToken);

    [HttpPost("Item/{id:guid}/Variations")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task CreateItemVariation([FromBody] CreateItemVariation command, Guid id, CancellationToken cancellationToken = default)
        => await mediator.Send(command with { Id = id, }, cancellationToken);

    [HttpPut("Item/{id:guid}/Variations")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task CreateItemVariation([FromBody] UpdateItemVariation command, Guid id, CancellationToken cancellationToken = default)
        => await mediator.Send(command with { Id = id, }, cancellationToken);

    [HttpDelete("Item/{id:guid}/Variations")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task DeleteItemVariation([FromBody] DeleteItemVariation command, Guid id, CancellationToken cancellationToken = default)
        => await mediator.Send(command with { Id = id, }, cancellationToken);
}
