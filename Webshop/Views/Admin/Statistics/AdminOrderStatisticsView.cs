using Webshop.Helpers;

namespace Webshop.Views.Admin.Statistics;

internal class AdminOrderStatisticsView(string headerText, AdminApplication adminApp) : 
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
                await ShowBestSellingProducts();
                break;
            case MenuItems.PopularCategories:
                await ShowPopularCategories();
                break;
            case MenuItems.PopularProductsPerCategory:
                await ShowPopularProductsPerCategory();
                break;
            case MenuItems.OrdersPerCity:
                await ShowOrdersPerCity();
                break;
            case MenuItems.ProductsSoldPerSupplier:
                await ShowProductsSoldPerSupplier();
                break;
        }
    }

    private async Task ShowBestSellingProducts()
    {
        Console.Clear();
        MessageHelper.ShowHeader("Bäst säljande produkter");

        var bestSellingProducts = await AdminApp.Database.GetBestSellingProductsAsync();

        foreach (var (ProductName, TotalQuantitySold) in bestSellingProducts)
        {
            Console.WriteLine($"""
                {ProductName}
                {TotalQuantitySold} st

                """);
        }

        MessageHelper.ShowSuccess("Klar!");
    }

    private async Task ShowPopularCategories()
    {
        Console.Clear();
        MessageHelper.ShowHeader("Populärast kategorier");

        var popularCategories = await AdminApp.Database.GetPopularCategoriesAsync();

        foreach (var (CategoryName, QuantityOfProductsSold) in popularCategories)
        {
            Console.WriteLine($"""
                {CategoryName}
                {QuantityOfProductsSold} sålda produkter

                """);
        }

        MessageHelper.ShowSuccess("Klar!");
    }

    private async Task ShowPopularProductsPerCategory()
    {
        Console.Clear();
        MessageHelper.ShowHeader("Populärast produkter per kategori");

        var popularProductsPerCategory = await AdminApp.Database.GetPopularProductsPerCategoryAsync();

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

        MessageHelper.ShowSuccess("Klar!");
    }

    private async Task ShowOrdersPerCity()
    {
        Console.Clear();
        MessageHelper.ShowHeader("Flest ordrar per stad");

        var ordersPerCity = await AdminApp.Database.GetOrdersPerCityAsync();

        foreach (var (CityName, OrderCount) in ordersPerCity)
        {
            Console.WriteLine($"""
                {CityName}
                {OrderCount} {(OrderCount == 1 ? "order" : "ordrar")}

                """);
        }

        MessageHelper.ShowSuccess("Klar!");
    }

    private async Task ShowProductsSoldPerSupplier()
    {
        Console.Clear();
        MessageHelper.ShowHeader("Försäljning per leverantör");

        var productsSoldPerSupplier = await AdminApp.Database.GetProductsSoldPerSupplierAsync();

        foreach (var (SupplierName, ProductsSold) in productsSoldPerSupplier)
        {
            Console.WriteLine($"""
                {SupplierName}
                {ProductsSold} sålda produkter

                """);
        }

        MessageHelper.ShowSuccess("Klar!");
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