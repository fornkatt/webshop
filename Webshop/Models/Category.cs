namespace Webshop.Models;

internal class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Product>? Products { get; set; } = [];
    public bool IsActive { get; set; }
}
