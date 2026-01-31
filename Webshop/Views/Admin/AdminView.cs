using Webshop.Views.Admin.Management;
using Webshop.Views.Admin.Statistics;

namespace Webshop.Views.Admin;

internal sealed class AdminView(string headerText, AdminApplication adminApp) :
    AdminMenuBase<AdminView.MenuItems>(headerText, adminApp)
{
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.HandleProducts, "Hantera produkter" },
        { MenuItems.HandleCategories, "Hantera kategorier" },
        { MenuItems.HandleCustomers, "Hantera kunder" },
        { MenuItems.HandleSuppliers, "Hantera leverantörer" },
        { MenuItems.Statistics, "Se statistik" },
        { MenuItems.Logs, "Se loggar (10 senaste)" }
    };

    internal enum MenuItems
    {
        HandleProducts = 1,
        HandleCategories,
        HandleSuppliers,
        HandleCustomers,
        Statistics,
        Logs
    }

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.HandleProducts:
                await new AdminProductsView("Hantera produkter", AdminApp).ActivateAsync();
                break;
            case MenuItems.HandleCategories:
                await new AdminCategoriesView("Hantera kategorier", AdminApp).ActivateAsync();
                break;
            case MenuItems.HandleSuppliers:
                await new AdminSuppliersView("Hantera leverantörer", AdminApp).ActivateAsync();
                break;
            case MenuItems.HandleCustomers:
                await new AdminCustomersView("Hantera kunder", AdminApp).ActivateAsync();
                break;
            case MenuItems.Statistics:
                await new AdminStatisticsView("Se statistik", AdminApp).ActivateAsync();
                break;
            case MenuItems.Logs:
                await new AdminLogsView("Loggar", AdminApp).DisplayLogs();
                break;
        }
    }
}