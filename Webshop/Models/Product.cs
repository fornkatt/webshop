namespace Webshop.Models;

internal class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ShortDescription { get; set; }
    public string? DetailedDescription { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public string? Supplier { get; set; }
    public int? Stock { get; set; }
    public decimal? Price { get; set; }
    public bool IsSelectedItem { get; set; } = false;
}