using Webshop.Helpers;

namespace Webshop.Views.Admin.Statistics;

internal class AdminProfitStatisticsView(string headerText, AdminApplication adminApp) : 
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
                await ShowTotalRevenue();
                break;
            case MenuItems.RevenuePerCategory:
                await ShowRevenuePerCategory();
                break;
            case MenuItems.RevenuePerSupplier:
                await ShowRevenuePerSupplier();
                break;
        }
    }

    private async Task ShowTotalRevenue()
    {
        Console.Clear();
        MessageHelper.ShowHeader("Totala intäkter");

        var totalAllTimeRevenue = await AdminApp.Database.GetAllTimeProfitsAsync();
        var totalDailyProfits = await AdminApp.Database.GetDailyProfitsAsync();
        var totalMonthlyProfits = await AdminApp.Database.GetMonthlyProfitsAsync();
        var totalYearlyProfits = await AdminApp.Database.GetYearlyProfitsAsync();

        Console.WriteLine($"""
            Totalt sedan start:     {totalAllTimeRevenue:C}

            Totalt idag:            {totalDailyProfits:C}

            Totalt denna måndad:    {totalMonthlyProfits:C}

            Total i år:             {totalYearlyProfits:C}
            """);

        MessageHelper.ShowSuccess("Klar!");
    }

    private async Task ShowRevenuePerCategory()
    {
        Console.Clear();
        MessageHelper.ShowHeader("Intäkter per kategori");

        var profitsPerCategory = await AdminApp.Database.GetProfitsPerCategoryAsync();

        foreach (var (CategoryName, TotalProfits) in profitsPerCategory)
        {
            Console.WriteLine($"""
                === {CategoryName.ToUpper()} ===

                {TotalProfits:C}

                """);
        }

        MessageHelper.ShowSuccess("Klar!");
    }

    private async Task ShowRevenuePerSupplier()
    {
        Console.Clear();
        MessageHelper.ShowHeader("Intäkter per leverantör");

        var profitsPerSupplier = await AdminApp.Database.GetProfitsPerSupplierAsync();

        foreach (var (SupplierName, TotalProfits) in profitsPerSupplier)
        {
            Console.WriteLine($"""
                === {SupplierName.ToUpper()} ===

                {TotalProfits:C}

                """);
        }

        MessageHelper.ShowSuccess("Klar!");
    }

    internal enum MenuItems
    {
        TotalRevenue = 1,
        RevenuePerCategory,
        RevenuePerSupplier
    }
}