namespace DotNetInterview.Domain.Exceptions;

public sealed class VariationAlreadyExistsException : Exception
{
    public VariationAlreadyExistsException(Guid id) : base($"The '{id}' variation already exists")
    {
    }
}
