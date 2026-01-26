
namespace Webshop.Views;

internal class AdminView(string headerText, AdminApplication adminApp) : 
    AdminMenuBase<AdminView.MenuItems>(headerText, adminApp)
{
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.HandleProducts, "Hantera produkter" },
        { MenuItems.HandleCategories, "Hantera kategorier" },
        { MenuItems.HandleCustomers, "Hantera kunder" },
        { MenuItems.HandleSuppliers, "Hantera leverantörer" },
        { MenuItems.Statistics, "Se statistik" }
    };

    internal enum MenuItems
    {
        HandleProducts = 1,
        HandleCategories,
        HandleSuppliers,
        HandleCustomers,
        Statistics
    }

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.HandleProducts:
                await new AdminProductsView("Hantera produkter", AdminApp).ActivateAsync();
                break;
            case MenuItems.HandleCategories:
                Console.WriteLine("Kategorier");
                Console.ReadKey(true);
                break;
            case MenuItems.HandleCustomers:
                Console.WriteLine("Kunder");
                Console.ReadKey(true);
                break;
            case MenuItems.HandleSuppliers:
                Console.WriteLine("Leverantörer");
                Console.ReadKey(true);
                break;
            case MenuItems.Statistics:
                Console.WriteLine("Statistik");
                Console.ReadKey(true);
                break;
        }
    }
}
