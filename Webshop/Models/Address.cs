namespace Webshop.Models;

internal class Address
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int? CountryId { get; set; }
    public Country? Country { get; set; }
    public string? City { get; set; }
    public int? PostalCode { get; set; }
    public string? Street { get; set; }
    public string? HouseNumber { get; set; }
}
