namespace DotNetInterview.Infrastructure.Dapper.Services;

using DotNetInterview.Application.Items.Interfaces;
using DotNetInterview.Application.Items.ReadModels;
using DotNetInterview.Infrastructure.EntityFramework.Services;
using global::Dapper;
using Microsoft.EntityFrameworkCore;
using System;

internal sealed class ItemReadService : IItemReadService
{
    const string GET_ALL_ITEMS =
"""
SELECT
    i.Id,
    i.Reference,
    i.Name,
    i.Price,
    COALESCE(
        (SELECT SUM(v.Quantity)
         FROM Variation v
         WHERE v.ItemId = i.Id),
        0
    ) AS TotalQuantity,
    COALESCE(
        (SELECT json_group_array(
            json_object(
                'Id', v.Id,
                'Size', v.Size,
                'Quantity', v.Quantity
            )
         )
         FROM Variation v
         WHERE v.ItemId = i.Id),
        '[]'
    ) AS Variations
FROM Items i;
""";

    const string GET_ITEM_BY_ID =
"""
SELECT
    i.Id,
    i.Reference,
    i.Name,
    i.Price,
    COALESCE(
        (SELECT SUM(v.Quantity)
         FROM Variation v
         WHERE v.ItemId = i.Id),
        0
    ) AS TotalQuantity,
    COALESCE(
        (SELECT json_group_array(
            json_object(
                'Id', v.Id,
                'Size', v.Size,
                'Quantity', v.Quantity
            )
         )
         FROM Variation v
         WHERE v.ItemId = i.Id),
        '[]'
    ) AS Variations
FROM Items i
WHERE i.Id = @Id
""";

    private readonly DataContext context;
    private readonly ILogger<ItemReadService> logger;

    public ItemReadService(DataContext context, ILogger<ItemReadService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<List<ItemReadModel>> GetAllItemsAsync(CancellationToken cancellationToken = default)
    {
        var connection = context.Database.GetDbConnection();

        var result = (await connection.QueryAsync<ItemReadModel>(GET_ALL_ITEMS)).ToList();

        return result;
    }

    public async Task<ItemReadModel?> GetItemWithVariationsById(Guid id, CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("GetItemWithVariationsById: {Id}", id);
        var connection = context.Database.GetDbConnection();

        var parameters = new
        {
            Id = id,
        };

        var item = await connection.QuerySingleOrDefaultAsync<ItemReadModel>(GET_ITEM_BY_ID, parameters);

        return item;
    }
}
