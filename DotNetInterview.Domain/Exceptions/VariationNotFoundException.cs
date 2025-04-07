namespace DotNetInterview.Domain.Exceptions;

public sealed class VariationNotFoundException : Exception
{
    public VariationNotFoundException(Guid id) : base($"The '{id}' variation not found")
    {
    }
}
