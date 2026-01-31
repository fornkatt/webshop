using System.Diagnostics;
using Webshop.Helpers;

namespace Webshop.Views.Admin.Statistics;

internal sealed class AdminOrderStatisticsView(string headerText, AdminApplication adminApp) : 
    AdminMenuBase<AdminOrderStatisticsView.MenuItems>(headerText, adminApp)
{
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.BestSellingProducts, "Bäst säljande produkter" },
        { MenuItems.PopularCategories, "Populärast kategorier" },
        { MenuItems.PopularProductsPerCategory, "Populärast produkter per kategori" },
        { MenuItems.OrdersPerCity, "Flest ordrar per stad" },
        { MenuItems.ProductsSoldPerSupplier, "Försäljning per leverantör" }
    };

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.BestSellingProducts:
                await ShowBestSellingProductsAsync();
                break;
            case MenuItems.PopularCategories:
                await ShowPopularCategoriesAsync();
                break;
            case MenuItems.PopularProductsPerCategory:
                await ShowPopularProductsPerCategoryAsync();
                break;
            case MenuItems.OrdersPerCity:
                await ShowOrdersPerCityAsync();
                break;
            case MenuItems.ProductsSoldPerSupplier:
                await ShowProductsSoldPerSupplierAsync();
                break;
        }
    }

    private async Task ShowBestSellingProductsAsync()
    {
        Console.Clear();
        MessageHelper.ShowHeader("Bäst säljande produkter");

        Stopwatch stopwatch = Stopwatch.StartNew();

        var bestSellingProducts = await AdminApp.Database.GetBestSellingProductsAsync();

        stopwatch.Stop();

        foreach (var (ProductName, TotalQuantitySold) in bestSellingProducts)
        {
            Console.WriteLine($"""
                {ProductName}
                {TotalQuantitySold} st

                """);
        }

        MessageHelper.ShowSuccess($"Klar! Total tid: {stopwatch.ElapsedMilliseconds} ms");
    }

    private async Task ShowPopularCategoriesAsync()
    {
        Console.Clear();
        MessageHelper.ShowHeader("Populärast kategorier");

        Stopwatch stopwatch = Stopwatch.StartNew();

        var popularCategories = await AdminApp.Database.GetPopularCategoriesAsync();

        stopwatch.Stop();

        foreach (var (CategoryName, QuantityOfProductsSold) in popularCategories)
        {
            Console.WriteLine($"""
                {CategoryName}
                {QuantityOfProductsSold} sålda produkter

                """);
        }

        MessageHelper.ShowSuccess($"Klar! Total tid: {stopwatch.ElapsedMilliseconds} ms");
    }

    private async Task ShowPopularProductsPerCategoryAsync()
    {
        Console.Clear();
        MessageHelper.ShowHeader("Populärast produkter per kategori");

        Stopwatch stopwatch = Stopwatch.StartNew();

        var popularProductsPerCategory = await AdminApp.Database.GetPopularProductsPerCategoryAsync();

        stopwatch.Stop();

        string currentCategory = "";

        foreach (var (CategoryName, ProductName, QuantitySold) in popularProductsPerCategory)
        {
            if (currentCategory != CategoryName)
            {
                Console.WriteLine($"""
                    === {CategoryName.ToUpper()} ===

                    """);
                currentCategory = CategoryName;
            }

            Console.WriteLine($"""
                {ProductName}: {QuantitySold} st

                """);
        }

        MessageHelper.ShowSuccess($"Klar! Total tid: {stopwatch.ElapsedMilliseconds} ms");
    }

    private async Task ShowOrdersPerCityAsync()
    {
        Console.Clear();
        MessageHelper.ShowHeader("Flest ordrar per stad");

        Stopwatch stopwatch = Stopwatch.StartNew();

        var ordersPerCity = await AdminApp.Database.GetOrdersPerCityAsync();

        stopwatch.Stop();

        foreach (var (CityName, OrderCount) in ordersPerCity)
        {
            Console.WriteLine($"""
                {CityName}
                {OrderCount} {(OrderCount == 1 ? "order" : "ordrar")}

                """);
        }

        MessageHelper.ShowSuccess($"Klar! Total tid: {stopwatch.ElapsedMilliseconds} ms");
    }

    private async Task ShowProductsSoldPerSupplierAsync()
    {
        Console.Clear();
        MessageHelper.ShowHeader("Försäljning per leverantör");

        Stopwatch stopwatch = Stopwatch.StartNew();

        var productsSoldPerSupplier = await AdminApp.Database.GetProductsSoldPerSupplierAsync();

        stopwatch.Stop();

        foreach (var (SupplierName, ProductsSold) in productsSoldPerSupplier)
        {
            Console.WriteLine($"""
                {SupplierName}
                {ProductsSold} sålda produkter

                """);
        }

        MessageHelper.ShowSuccess($"Klar! Total tid: {stopwatch.ElapsedMilliseconds} ms");
    }

    internal enum MenuItems
    {
        BestSellingProducts = 1,
        PopularCategories,
        PopularProductsPerCategory,
        OrdersPerCity,
        ProductsSoldPerSupplier
    }
}