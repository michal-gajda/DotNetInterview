namespace DotNetInterview.Application.Items.Exceptions;

public sealed class ItemNotFoundException : Exception
{
    public ItemNotFoundException(Guid id) : base($"Item '{id}' not found")
    { }
}
