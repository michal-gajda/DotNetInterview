namespace DotNetInterview.Infrastructure.Dapper.TypeHandlers;

using System.Data;
using global::Dapper;

internal sealed class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
        parameter.Value = value.ToString();
    }

    public override Guid Parse(object value)
    {
        if (value is string stringValue)
        {
            return Guid.Parse(stringValue);
        }

        if (value is Guid guidValue)
        {
            return guidValue;
        }

        throw new DataException($"You cannot convert the type {value.GetType()} to Guid.");
    }
}
