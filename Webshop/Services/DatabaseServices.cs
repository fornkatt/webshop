using Microsoft.EntityFrameworkCore;
using Dapper;
using Webshop.Models;
using Microsoft.Data.SqlClient;

namespace Webshop.Services;

internal class DatabaseServices
{
    // Queries med EF
    internal async Task<List<Category>> GetCategoriesAsync()
    {
        using var db = new WebshopDbContext();
        return await db.Categories
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

    // Queries med Dapper
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