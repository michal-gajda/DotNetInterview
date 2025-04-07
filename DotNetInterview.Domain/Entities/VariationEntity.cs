using DotNetInterview.Domain.Exceptions;

namespace DotNetInterview.Domain.Entities;

public sealed record class VariationEntity
{
    public Guid Id { get; private init; }
    public string Size { get; set; } = string.Empty;
    public int Quantity { get; set; } = 0;

    public VariationEntity(Guid id, string size, int quantity)
    {
        this.Id = id;
        this.SetSize(size);
        this.SetQuantity(quantity);
    }

    public void SetSize(string size)
    {
        this.Size = size;
    }

    public void SetQuantity(int quantity)
    {
        if (quantity < 0)
        {
            throw new QuantityOutOfRangeException(this.Id);
        }

        this.Quantity = quantity;
    }
}
