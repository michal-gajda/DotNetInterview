namespace DotNetInterview.Infrastructure.EntityFramework.Services;

using DotNetInterview.Domain.Entities;
using DotNetInterview.Domain.Interfaces;
using DotNetInterview.Infrastructure.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

internal sealed class ItemRepository : IItemRepository
{
    private readonly DataContext context;
    private readonly ILogger<ItemRepository> logger;

    public ItemRepository(DataContext context, ILogger<ItemRepository> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await this.context.Items.FindAsync(id, cancellationToken);

        if (item is not null)
        {
            this.context.Items.Remove(item);

            await this.context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<ItemEntity?> LoadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await this.context.Items
            .Include(i => i.Variations)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        var item = new ItemEntity(entity.Id, entity.Reference, entity.Name, entity.Price);

        foreach (var variation in entity.Variations)
        {
            item.CreateVariation(variation.Id, variation.Size, variation.Quantity);
        }

        return item;
    }

    public async Task SaveAsync(ItemEntity entity, CancellationToken cancellationToken = default)
    {
        await this.DeleteAsync(entity.Id, cancellationToken);

        var item = new Item
        {
            Id = entity.Id,
            Reference = entity.Reference,
            Name = entity.Name,
            Price = entity.Price,
        };

        this.context.Items.Add(item);

        foreach (var variation in entity.Variations)
        {
            item.Variations.Add(new Variation
            {
                Id = variation.Id,
                ItemId = item.Id,
                Size = variation.Size,
                Quantity = variation.Quantity
            });
        }

        await this.context.SaveChangesAsync(cancellationToken);
    }
}
