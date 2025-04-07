namespace DotNetInterview.Application.Items.Services;

using DotNetInterview.Application.Items.Interfaces;
using DotNetInterview.Application.Items.QueryResults;
using DotNetInterview.Application.Items.ReadModels;

internal sealed class ItemProcessManager : IItemProcessManager
{
    private readonly TimeProvider timeProvider;

    public ItemProcessManager(TimeProvider timeProvider)
    {
        this.timeProvider = timeProvider;
    }

    public Item Process(ItemReadModel model)
    {
        var time = timeProvider.GetUtcNow();
        var dayOfWeek = time.DayOfWeek;
        var hour = time.Hour;

        var (discount, currentPrice) = CalculateDiscountAndCurrentPrice(model.Price, model.TotalQuantity, dayOfWeek, hour);

        return new Item
        {
            Id = model.Id,
            CurrentPrice = currentPrice,
            Discount = discount,
            Name = model.Name,
            Price = model.Price,
            Reference = model.Reference,
            Status = CalculateStatus(model.TotalQuantity),
            TotalQuantity = model.TotalQuantity,
        };
    }

    private static Tuple<int?, decimal?> CalculateDiscountAndCurrentPrice(decimal basePrice, int quantity, DayOfWeek dayOfWeek, int hour)
    {
        int? discount = null;

        if (quantity is 0)
        {
            return new Tuple<int?, decimal?>(null, null);
        }

        if (dayOfWeek.Equals(DayOfWeek.Monday) && hour is >= 12 and <= 17)
        {
            discount = 50;
        }
        else
        {
            switch (quantity)
            {
                case > 10:
                    discount = 20;
                    break;
                case > 5:
                    discount = 10;
                    break;
                default:
                    break;
            }
        }

        var currentPrice = CalculateCurrentPrice(basePrice, discount);

        return new Tuple<int?, decimal?>(discount, currentPrice);
    }

    private static decimal? CalculateCurrentPrice(decimal basePrice, int? discount)
    {
        if (discount is null)
        {
            return basePrice;
        }

        return basePrice * (100 - discount.Value) / 100;
    }

    private static string CalculateStatus(int quantity) => quantity > 0
        ? "In Stock"
        : "Sold Out";
}
