#nullable disable
namespace DotNetInterview.Infrastructure.EntityFramework.Models;

public record Item
{
    public Guid Id { get; set; }
    public string Reference { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public ICollection<Variation> Variations { get; set; } = new List<Variation>();
}
