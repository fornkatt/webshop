using Webshop.Helpers;

namespace Webshop.Views.Admin.Statistics;

internal class AdminProfitStatisticsView(string headerText, AdminApplication adminApp) : 
    AdminMenuBase<AdminProfitStatisticsView.MenuItems>(headerText, adminApp)
{
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.TotalRevenue, "Total intäkt" },
        { MenuItems.RevenuePerCategory, "Intäkt per kategori" },
        { MenuItems.RevenuePerSupplier, "Intäkt per leverantör" }
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
        // ToDo
        MessageHelper.ShowSuccess("Klar!");
    }

    private async Task ShowRevenuePerCategory()
    {
        // ToDo
        MessageHelper.ShowSuccess("Klar!");
    }

    private async Task ShowRevenuePerSupplier()
    {
        // ToDo
        MessageHelper.ShowSuccess("Klar!");
    }

    internal enum MenuItems
    {
        TotalRevenue = 1,
        RevenuePerCategory,
        RevenuePerSupplier
    }
}