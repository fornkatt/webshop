namespace Webshop.Models;

internal class PaymentMethod
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Provider { get; set; }
    public bool IsActive { get; set; } = true;
    public required decimal TransactionFee { get; set; }
    public List<Order>? Orders { get; set; }
}
