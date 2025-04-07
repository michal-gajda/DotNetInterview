namespace DotNetInterview.Domain.Interfaces;

using DotNetInterview.Domain.Entities;

public interface IItemRepository
{
    Task<ItemEntity?> LoadAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveAsync(ItemEntity entity, CancellationToken cancellationToken = default);
}
