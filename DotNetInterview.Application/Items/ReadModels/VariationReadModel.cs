namespace DotNetInterview.Application.Items.ReadModels;

using DotNetInterview.Application.Items.ViewModels;

public sealed record class VariationReadModel
{
    public required Guid Id { get; init; }
    public required string Size { get; init; }
    public required int Quantity { get; init; }

    public static explicit operator Variation(VariationReadModel readModel) => new()
    {
        Id = readModel.Id,
        Quantity = readModel.Quantity,
        Size = readModel.Size,
    };
}
