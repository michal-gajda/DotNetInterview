namespace DotNetInterview.Infrastructure.Dapper.TypeHandlers;

using System.Data;
using System.Text.Json;
using DotNetInterview.Application.Items.ReadModels;
using global::Dapper;

internal sealed class VariationListTypeHandler : SqlMapper.TypeHandler<IReadOnlyList<VariationReadModel>>
{
    public override IReadOnlyList<VariationReadModel>? Parse(object value)
    {
        if (value is string json)
        {
            try
            {
                var variations = JsonSerializer.Deserialize<IReadOnlyList<VariationReadModel>>(json);

                return variations ?? new List<VariationReadModel>();
            }
            catch (Exception exception)
            {
                throw new DataException("Parse error", exception);
            }
        }

        throw new DataException($"Incorrect data: {value.GetType()}");
    }

    public override void SetValue(IDbDataParameter parameter, IReadOnlyList<VariationReadModel>? value)
    {
        parameter.Value = JsonSerializer.Serialize(value);
    }
}
