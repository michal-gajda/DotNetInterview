using DotNetInterview.Application.Items.ReadModels;

namespace DotNetInterview.Application.Items.Interfaces;

public interface IItemReadService
{
    Task<List<ItemReadModel>> GetAllItemsAsync(CancellationToken cancellationToken = default);
    Task<ItemReadModel?> GetItemWithVariationsById(Guid id, CancellationToken cancellationToken = default);
}
