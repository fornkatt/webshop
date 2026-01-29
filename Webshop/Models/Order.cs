namespace Webshop.Models;

internal class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalAmount { get; set; }
    public int ShippingAddressId { get; set; }
    public Address? ShippingAddress { get; set; }
    public int ShippingMethodId { get; set; }
    public ShippingMethod? ShippingMethod { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal PaymentMethodCost { get; set; }
    public int PaymentMethodId { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public string? PaymentTransactionId { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    public List<OrderItem> OrderItems { get; set; } = [];
}

internal enum PaymentStatus
{
    Pending,
    Authorized,
    Completed,
    Failed,
    Refunded
}
internal enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Processing = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5,
    Refunded = 6
}