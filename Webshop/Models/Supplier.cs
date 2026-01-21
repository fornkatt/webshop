namespace Webshop.Models;

internal class Supplier
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<Product>? Products { get; set; }
}