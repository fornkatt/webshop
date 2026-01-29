using Microsoft.EntityFrameworkCore;
using Webshop.Models;

namespace Webshop.Services;

internal abstract class DatabaseServiceBase
{
    internal async Task AddNewCustomerAsync(Customer customer)
    {
        using var db = new WebshopDbContext();
        await db.Customers.AddAsync(customer);
        await db.SaveChangesAsync();
    }
    internal async Task UpdateCustomerAsync(Customer customer)
    {
        using var db = new WebshopDbContext();
        db.Customers.Update(customer);
        db.Entry(customer).State = EntityState.Modified;
        await db.SaveChangesAsync();
    }
}
