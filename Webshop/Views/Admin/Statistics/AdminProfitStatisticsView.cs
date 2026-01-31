using System.Diagnostics;
using Webshop.Helpers;

namespace Webshop.Views.Admin.Statistics;

internal sealed class AdminProfitStatisticsView(string headerText, AdminApplication adminApp) : 
    AdminMenuBase<AdminProfitStatisticsView.MenuItems>(headerText, adminApp)
{
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.TotalRevenue, "Totala intäkter" },
        { MenuItems.RevenuePerCategory, "Intäkter per kategori" },
        { MenuItems.RevenuePerSupplier, "Intäkter per leverantör" }
    };

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.TotalRevenue:
                await ShowTotalRevenueAsync();
                break;
            case MenuItems.RevenuePerCategory:
                await ShowRevenuePerCategoryAsync();
                break;
            case MenuItems.RevenuePerSupplier:
                await ShowRevenuePerSupplierAsync();
                break;
        }
    }

    private async Task ShowTotalRevenueAsync()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        
        Console.Clear();
        MessageHelper.ShowHeader("Totala intäkter");

        var totalAllTimeRevenue = await AdminApp.Database.GetAllTimeProfitsAsync();
        var totalDailyProfits = await AdminApp.Database.GetDailyProfitsAsync();
        var totalMonthlyProfits = await AdminApp.Database.GetMonthlyProfitsAsync();
        var totalYearlyProfits = await AdminApp.Database.GetYearlyProfitsAsync();

        stopwatch.Stop();

        Console.WriteLine($"""
            Totalt sedan start:     {totalAllTimeRevenue:C}

            Totalt idag:            {totalDailyProfits:C}

            Totalt denna måndad:    {totalMonthlyProfits:C}

            Total i år:             {totalYearlyProfits:C}
            """);
        
        MessageHelper.ShowSuccess($"Klar! Total tid: {stopwatch.ElapsedMilliseconds} ms");
    }

    private async Task ShowRevenuePerCategoryAsync()
    {
        Console.Clear();
        MessageHelper.ShowHeader("Intäkter per kategori");

        Stopwatch stopwatch = Stopwatch.StartNew();

        var profitsPerCategory = await AdminApp.Database.GetProfitsPerCategoryAsync();

        stopwatch.Stop();

        foreach (var (CategoryName, TotalProfits) in profitsPerCategory)
        {
            Console.WriteLine($"""
                === {CategoryName.ToUpper()} ===

                {TotalProfits:C}

                """);
        }

        MessageHelper.ShowSuccess($"Klar! Total tid: {stopwatch.ElapsedMilliseconds} ms");
    }

    private async Task ShowRevenuePerSupplierAsync()
    {
        Console.Clear();
        MessageHelper.ShowHeader("Intäkter per leverantör");

        Stopwatch stopwatch = Stopwatch.StartNew();

        var profitsPerSupplier = await AdminApp.Database.GetProfitsPerSupplierAsync();

        stopwatch.Stop();

        foreach (var (SupplierName, TotalProfits) in profitsPerSupplier)
        {
            Console.WriteLine($"""
                === {SupplierName.ToUpper()} ===

                {TotalProfits:C}

                """);
        }

        MessageHelper.ShowSuccess($"Klar! Total tid: {stopwatch.ElapsedMilliseconds} ms");
    }

    internal enum MenuItems
    {
        TotalRevenue = 1,
        RevenuePerCategory,
        RevenuePerSupplier
    }
}