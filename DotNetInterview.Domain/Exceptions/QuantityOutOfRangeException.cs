namespace DotNetInterview.Domain.Exceptions;

public sealed class QuantityOutOfRangeException : Exception
{
    public QuantityOutOfRangeException(Guid id) : base($"Quantity for '{id}' is out of range")
    {
    }
}
