namespace Webshop.Models;

internal class Customer
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public List<Address> Addresses { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}
