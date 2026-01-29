using Microsoft.EntityFrameworkCore;
using Webshop.Models;

namespace Webshop.Services;

internal sealed class AdminDatabaseService : DatabaseServiceBase
{
    internal async Task<List<(string SupplierName, decimal TotalProfits)>> GetProfitsPerSupplierAsync()
    {
        using var db = new WebshopDbContext();

        var results = await db.OrderItems
            .Include(oi => oi.Product)
                .ThenInclude(p => p!.Supplier)
            .Include(oi => oi.Order)
            .GroupBy(oi => new
            {
                SupplierId = oi.Product!.SupplierId,
                SupplierName = oi.Product.Supplier!.Name
            })
            .Select(group => new
            {
                SupplierName = group.Key.SupplierName ?? "Okänd",
                TotalProfit = group.Sum(oi => (oi.Quantity * oi.UnitPrice / oi.Order!.TotalAmount) * oi.Order.TotalAmount)
            })
            .OrderByDescending(x => x.TotalProfit)
            .ToListAsync();

        return results
            .Select(x => (x.SupplierName, x.TotalProfit))
            .ToList();
    }
    internal async Task<List<(string CategoryName, decimal TotalProfits)>> GetProfitsPerCategoryAsync()
    {
        using var db = new WebshopDbContext();

        var results = await db.OrderItems
            .Include(oi => oi.Product)
                .ThenInclude(p => p!.Category)
            .Include(oi => oi.Order)
            .GroupBy(oi => new
            {
                CategoryId = oi.Product!.CategoryId,
                CategoryName = oi.Product.Category!.Name
            })
            .Select(group => new
            {
                CategoryName = group.Key.CategoryName ?? "Okänd",
                TotalProfit = group.Sum(oi => (oi.Quantity * oi.UnitPrice / oi.Order!.TotalAmount) * oi.Order.TotalAmount)
            })
            .OrderByDescending(x => x.TotalProfit)
            .ToListAsync();

        return results
            .Select(x => (x.CategoryName, x.TotalProfit))
            .ToList();
    }
    internal async Task<decimal> GetYearlyProfitsAsync()
    {
        using var db = new WebshopDbContext();

        var thisYear = DateTime.Today.Year;

        return await db.Orders
            .Where(o => o.OrderDate.Year == thisYear)
            .SumAsync(o => o.TotalAmount);
    }
    internal async Task<decimal> GetMonthlyProfitsAsync()
    {
        using var db = new WebshopDbContext();

        var thisYear = DateTime.Today.Year;
        var thisMonth = DateTime.Today.Month;

        return await db.Orders
            .Where(o => o.OrderDate.Month == thisMonth && o.OrderDate.Year == thisYear)
            .SumAsync(o => o.TotalAmount);
    }
    internal async Task<decimal> GetDailyProfitsAsync()
    {
        using var db = new WebshopDbContext();

        var today = DateTime.Today;


        return await db.Orders
            .Where(o => o.OrderDate.Date == today)
            .SumAsync(o => o.TotalAmount);
    }
    internal async Task<decimal> GetAllTimeProfitsAsync()
    {
        using var db = new WebshopDbContext();

        return await db.Orders
            .SumAsync(o => o.TotalAmount);
    }
    internal async Task<List<(string SupplierName, int ProductsSold)>> GetProductsSoldPerSupplierAsync()
    {
        using var db = new WebshopDbContext();

        var results = await db.OrderItems
            .Include(oi => oi.Product)
                .ThenInclude(p => p!.Supplier)
            .GroupBy(oi => oi.Product!.Supplier!.Name)
            .Select(group => new
            {
                SupplierName = group.Key ?? "Okänd",
                ProductsSold = group.Sum(oi => oi.Quantity)
            })
            .OrderByDescending(x => x.ProductsSold)
            .ToListAsync();

        return results
            .Select(x => (x.SupplierName, x.ProductsSold))
            .ToList();
    }
    internal async Task<List<(string CityName, int OrderCount)>> GetOrdersPerCityAsync()
    {
        using var db = new WebshopDbContext();

        var results = await db.Orders
            .Include(o => o.ShippingAddress)
            .GroupBy(o => o.ShippingAddress!.City)
            .Select(group => new
            {
                CityName = group.Key ?? "Okänd",
                OrderCount = group.Count()
            })
            .OrderByDescending(x => x.OrderCount)
            .ToListAsync();

        return results
            .Select(x => (x.CityName, x.OrderCount))
            .ToList();
    }
    internal async Task<List<(string CategoryName, string ProductName, int QuantitySold)>> GetPopularProductsPerCategoryAsync()
    {
        using var db = new WebshopDbContext();

        var results = await (
        from oi in db.OrderItems
        join p in db.Products on oi.ProductId equals p.Id
        join c in db.Categories on p.CategoryId equals c.Id
        group oi by new { CategoryName = c.Name, ProductName = p.Name } into g
        select new
        {
            CategoryName = g.Key.CategoryName ?? "Okänd",
            ProductName = g.Key.ProductName ?? "Okänd",
            QuantitySold = g.Sum(oi => oi.Quantity)
        })
        .OrderBy(x => x.CategoryName)
        .ThenByDescending(x => x.QuantitySold)
        .ToListAsync();

        return results
            .Select(x => (x.CategoryName, x.ProductName, x.QuantitySold))
            .ToList();
    }
    internal async Task<List<(string CategoryName, int QuantityOfProductsSold)>> GetPopularCategoriesAsync()
    {
        using var db = new WebshopDbContext();
        var results = await db.OrderItems
            .Include(oi => oi.Product)
                .ThenInclude(p => p!.Category)
            .GroupBy(oi => new
            {
                CategoryId = oi.Product!.CategoryId,
                CategoryName = oi.Product.Category!.Name
            })
            .Select(group => new
            {
                CategoryName = group.Key.CategoryName ?? "Okänd",
                QuantityOfProductsSold = group.Sum(oi => oi.Quantity)
            })
            .OrderByDescending(x => x.QuantityOfProductsSold)
            .ToListAsync();

        return results
            .Select(x => (x.CategoryName, x.QuantityOfProductsSold))
            .ToList();
    }
    internal async Task<List<(string ProductName, int TotalQuantitySold)>> GetBestSellingProductsAsync()
    {
        using var db = new WebshopDbContext();
        var results = await db.OrderItems
            .GroupBy(oi => new
            {
                oi.ProductId,
                oi.ProductName
            })
            .Select(group => new
            {
                ProductName = group.Key.ProductName ?? "Okänd",
                TotalQuantitySold = group.Sum(oi => oi.Quantity)
            })
            .OrderByDescending(x => x.TotalQuantitySold)
            .ToListAsync();

        return results
            .Select(x => (x.ProductName, x.TotalQuantitySold))
            .ToList();
    }
    internal async Task<int> GetTotalAmountOfSoldProductsAsync()
    {
        using var db = new WebshopDbContext();
        return await db.OrderItems
            .SumAsync(oi => oi.Quantity);
    }
    internal async Task<int> GetTotalAmountOfOrdersAsync()
    {
        using var db = new WebshopDbContext();
        return await db.Orders
            .CountAsync();
    }
    internal async Task<int> GetTotalAmountOfCustomersAsync()
    {
        using var db = new WebshopDbContext();
        return await db.Customers
            .CountAsync();
    }
    internal async Task<List<Order>> GetCustomerOrdersAsync(int customerId)
    {
        using var db = new WebshopDbContext();
        return await db.Orders
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.OrderItems)
            .Include(o => o.PaymentMethod)
            .Include(o => o.ShippingMethod)
            .Include(o => o.ShippingAddress)
                .ThenInclude(a => a!.Country)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }
    internal async Task UpdateCustomerAddressAsync(Address address)
    {
        using var db = new WebshopDbContext();
        db.Addresses.Update(address);
        db.Entry(address).State = EntityState.Modified;
        await db.SaveChangesAsync();
    }
    internal async Task<Address?> GetCustomerAddressAsync(int customerId)
    {
        using var db = new WebshopDbContext();
        return await db.Addresses
            .Where(a => a.CustomerId == customerId)
            .FirstOrDefaultAsync();
    }
    internal async Task RemoveCustomerAsync(Customer customer)
    {
        using var db = new WebshopDbContext();
        db.Customers.Remove(customer);
        await db.SaveChangesAsync();
    }
    internal async Task RemoveSupplierAsync(Supplier supplier)
    {
        using var db = new WebshopDbContext();
        db.Suppliers.Remove(supplier);
        await db.SaveChangesAsync();
    }
    internal async Task UpdateSupplierAsync(Supplier supplier)
    {
        using var db = new WebshopDbContext();
        db.Suppliers.Update(supplier);
        db.Entry(supplier).State = EntityState.Modified;
        await db.SaveChangesAsync();
    }
    internal async Task AddSupplierAsync(Supplier supplier)
    {
        using var db = new WebshopDbContext();
        db.Suppliers.Add(supplier);
        await db.SaveChangesAsync();
    }
    internal async Task UpdateCategoryAsync(Category category)
    {
        using var db = new WebshopDbContext();
        db.Categories.Update(category);
        db.Entry(category).State = EntityState.Modified;
        await db.SaveChangesAsync();
    }
    internal async Task RemoveCategoryAsync(Category category)
    {
        using var db = new WebshopDbContext();
        db.Categories.Remove(category);
        await db.SaveChangesAsync();
    }
    internal async Task AddCategoryAsync(Category category)
    {
        using var db = new WebshopDbContext();
        db.Categories.Add(category);
        await db.SaveChangesAsync();
    }
    internal async Task UpdateProductAsync(Product product)
    {
        using var db = new WebshopDbContext();
        db.Products.Update(product);
        db.Entry(product).State = EntityState.Modified;
        await db.SaveChangesAsync();
    }
    internal async Task<List<Product>> GetProductsPerCategoryAsync(int? categoryId)
    {
        using var db = new WebshopDbContext();
        return await db.Products
            .Where(p => p.CategoryId == categoryId)
            .Include(p => p.Supplier)
            .Include(p => p.Category)
            .ToListAsync();
    }
    internal async Task<List<Product>> GetAllProductsAsync()
    {
        using var db = new WebshopDbContext();
        return await db.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .OrderByDescending(p => p.IsActive)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }
    internal async Task<List<Category>> GetAllCategoriesAsync()
    {
        using var db = new WebshopDbContext();
        return await db.Categories
            .OrderByDescending(c => c.IsActive)
            .ThenBy(c => c.Name)
            .ToListAsync();
    }
    internal async Task<List<Supplier>> GetAllSuppliersAsync()
    {
        using var db = new WebshopDbContext();
        return await db.Suppliers
            .OrderBy(s => s.Name)
            .ToListAsync();
    }
    internal async Task<List<Customer>> GetAllCustomersAsync()
    {
        using var db = new WebshopDbContext();
        return await db.Customers
            .Include(c => c.Address!)
                .ThenInclude(a => a.Country)
            .OrderByDescending(c => c.IsActive)
            .ThenBy(c => c.LastName)
            .ToListAsync();
    }
    internal async Task RemoveProductAsync(Product product)
    {
        using var db = new WebshopDbContext();
        db.Products.Remove(product);
        db.SaveChanges();
    }
    internal async Task AddNewProductAsync(Product product)
    {
        using var db = new WebshopDbContext();
        await db.Products.AddAsync(product);
        await db.SaveChangesAsync();
    }
}