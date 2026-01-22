namespace Webshop.Models;

internal class ShippingMethod
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public required decimal Price { get; set; }
    public int EstimatedDaysMin { get; set; }
    public int EstimatedDaysMax { get; set; }
    public bool IsActive { get; set; } = true;
    public List<Order>? Orders { get; set; }
}
