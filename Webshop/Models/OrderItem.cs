namespace Webshop.Models;

internal class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public required Order Order { get; set; }
    public int ProductId { get; set; }
    public required Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
    public required string ProductName { get; set; }
}
