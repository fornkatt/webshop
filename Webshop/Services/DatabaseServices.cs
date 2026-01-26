using Microsoft.EntityFrameworkCore;
using Dapper;
using Webshop.Models;
using Microsoft.Data.SqlClient;

namespace Webshop.Services;

internal class DatabaseServices
{
    // Queries with EF

    // Admin-facing methods, gets unfiltered results and includes queries to delete, add, update etc
    internal async Task ReplaceProductForAdminAsync(Product product)
    {
        using var db = new WebshopDbContext();
        db.Products.Update(product);
        db.Entry(product).State = EntityState.Modified;
        await db.SaveChangesAsync();
    }
    internal async Task<List<Product>> GetProductsPerCategoryForAdminAsync(int? categoryId)
    {
        using var db = new WebshopDbContext();
        return await db.Products
            .Where(p => p.CategoryId == categoryId)
            .Include(p => p.Supplier)
            .Include(p => p.Category)
            .ToListAsync();
    }
    internal async Task<List<Product>> GetAllProductsForAdminAsync()
    {
        using var db = new WebshopDbContext();
        return await db.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .OrderByDescending(p => p.IsActive)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }
    internal async Task<List<Category>> GetAllCategoriesForAdminAsync()
    {
        using var db = new WebshopDbContext();
        return await db.Categories
            .OrderByDescending(c => c.IsActive)
            .ThenBy(c => c.Name)
            .ToListAsync();
    }
    internal async Task<List<Supplier>> GetAllSuppliersForAdminAsync()
    {
        using var db = new WebshopDbContext();
        return await db.Suppliers
            .OrderBy(s => s.Name)
            .ToListAsync();
    }
    internal async Task<List<Customer>> GetAllCustomersForAdminAsync()
    {
        using var db = new WebshopDbContext();
        return await db.Customers
            .Include(c => c.Address!)
                .ThenInclude(a => a.Country)
            .OrderByDescending(c => c.IsActive)
            .ThenBy(c => c.LastName)
            .ToListAsync();
    }
    internal async Task DeleteProductForAdminAsync(Product product)
    {
        using var db = new WebshopDbContext();
        db.Products.Remove(product);
        db.SaveChanges();
    }
    internal async Task AddNewProductForAdminAsync(Product product)
    {
        using var db = new WebshopDbContext();
        db.Products.Add(product);
        db.SaveChanges();
    }

    // Customer-facing methods, gets filtered results
    internal async Task<List<Category>> GetAllCategoriesAsync()
    {
        using var db = new WebshopDbContext();
        return await db.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    internal async Task<List<Supplier>> GetAllSuppliersAsync()
    {
        using var db = new WebshopDbContext();
        return await db.Suppliers
            .ToListAsync();
    }

    internal async Task<List<Customer>> GetAllCustomersAsync()
    {
        using var db = new WebshopDbContext();
        return await db.Customers
            .Include(c => c.Address!)
                .ThenInclude(a => a.Country)
            .ToListAsync();
    }

    internal async Task<List<Product>> GetSelectedProductsAsync()
    {
        using var db = new WebshopDbContext();
        return await db.Products
            .Where(p => p.IsSelectedItem && !p.IsDiscontinued)
            .Include(p => p.Category)
            .ToListAsync();
    }

    internal async Task<List<Product>> GetProductsPerCategoryAsync(int categoryId)
    {
        using var db = new WebshopDbContext();
        return await db.Products
            .Where(p => p.CategoryId == categoryId && !p.IsDiscontinued)
            .Include(p => p.Supplier)
            .Include(p => p.Category)
            .ToListAsync();
    }

    internal async Task<List<Product>> GetAllProductsAsync()
    {
        using var db = new WebshopDbContext();
        return await db.Products
            .Where(p => !p.IsDiscontinued)
            .ToListAsync();
    }

    internal async Task<List<PaymentMethod>> GetPaymentMethodsAsync()
    {
        using var db = new WebshopDbContext();
        return await db.PaymentMethods
            .Where(pm => pm.IsActive)
            .ToListAsync();
    }

    internal async Task<List<ShippingMethod>> GetShippingMethodsAsync()
    {
        using var db = new WebshopDbContext();
        return await db.ShippingMethods
            .Where(sm => sm.IsActive)
            .ToListAsync();
    }

    internal async Task<string> GetCountryName(int? countryId)
    {
        if (countryId == null || countryId == 0)
        {
            return "Okänd";
        }
        
        using var db = new WebshopDbContext();
        
        return await db.Countries
            .AsNoTracking()
            .Where(c => c.Id == countryId)
            .Select(c => c.Name)
            .FirstOrDefaultAsync() ?? "Okänd";
    }

    // Queries with Dapper
    internal async Task<List<Product>> GetSearchedItems(string userInput)
    {
        string sql = """
            SELECT *
            FROM webshop.Products p
            WHERE p.Name LIKE '%' + @SearchTerm + '%'
            AND p.IsDiscontinued = 0
            AND p.IsActive = 1
            """;

        using var connection = new SqlConnection(Helpers.ConfigHelper.GetConnectionString());

        var searchedItems = await connection.QueryAsync<Product>(sql, new { SearchTerm = userInput });

        return searchedItems.ToList();
    }
}