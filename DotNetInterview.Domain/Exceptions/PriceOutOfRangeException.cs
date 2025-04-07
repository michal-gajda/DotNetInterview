namespace DotNetInterview.Domain.Exceptions;

public sealed class PriceOutOfRangeException : Exception
{
    public PriceOutOfRangeException(Guid id) : base($"Price for '{id}' is out of range")
    {
    }
}
