using DotNetInterview.Domain.Exceptions;

namespace DotNetInterview.Domain.Entities;

public sealed class ItemEntity
{
    public Guid Id { get; private init; }

    public string Reference { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; } = decimal.Zero;

    public IReadOnlyList<VariationEntity> Variations => this.variations.Values
        .ToList()
        .AsReadOnly();

    private readonly Dictionary<Guid, VariationEntity> variations = new();

    public ItemEntity(Guid id, string reference, string name, decimal price)
    {
        this.Id = id;
        this.SetReference(reference);
        this.SetName(name);
        this.SetPrice(price);
    }

    public void SetName(string name)
    {
        this.Name = name;
    }

    public void SetReference(string reference)
    {
        this.Reference = reference;
    }

    public void SetPrice(decimal price)
    {
        if (price < 0)
        {
            throw new PriceOutOfRangeException(this.Id);
        }

        this.Price = price;
    }

    public void CreateVariation(Guid id, string size, int quantity)
    {
        if (this.variations.ContainsKey(id))
        {
            throw new VariationAlreadyExistsException(id);
        }

        this.variations[id] = new VariationEntity(
            id,
            size,
            quantity
        );
    }

    public void UpdateVariation(Guid id, string size, int quantity)
    {
        if (this.variations.TryGetValue(id, out var variation))
        {
            variation.Size = size;
            variation.Quantity = quantity;
        }
        else
        {
            throw new VariationNotFoundException(id);
        }
    }

    public void DeleteVariation(Guid id)
    {
        this.variations.Remove(id);
    }
}
