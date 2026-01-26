using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.Models;

internal class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ShortDescription { get; set; }
    public string? DetailedDescription { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public int? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public int? Stock { get; set; }
    public decimal? Price { get; set; }
    public decimal? OriginalPrice { get; set; }
    public bool IsSelectedItem { get; set; } = false;
    public bool IsSaleItem { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public bool IsDiscontinued { get; set; } = false;
    public DateTime? DiscontinuedDate { get; set; }
    public List<OrderItem>? OrderItems { get; set; }

    [NotMapped]
    public decimal? DiscountPercentage =>
        OriginalPrice > 0 ? (OriginalPrice - Price) / OriginalPrice * 100 : 0;
}