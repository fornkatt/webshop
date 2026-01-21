using Microsoft.EntityFrameworkCore;
using Webshop.Models;

namespace Webshop.Services;

internal class DatabaseServices
{
    internal async Task<List<Category>> GetCategoriesAsync(WebshopDbContext db)
    {
        return await db.Categories.ToListAsync();
    }

    internal async Task<List<Product>> GetProductsPerCategoryAsync(WebshopDbContext db, int categoryId)
    {
        var products = await db.Products
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
        return products;
    }

    internal async Task<List<Customer>> GetAllCustomers(WebshopDbContext db)
    {
        var customers = await db.Customers.ToListAsync();
        return customers;
    }

    internal async Task<List<Product>> GetSelectedProductsAsync(WebshopDbContext db)
    {
        return await db.Products
            .Where(p => p.IsSelectedItem)
            .Include(p => p.Category)
            .ToListAsync();
    }
}
