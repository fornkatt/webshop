namespace Webshop.Views.Admin.Statistics;

internal class AdminStatisticsView(string headerText, AdminApplication adminApp) : 
    AdminMenuBase<AdminStatisticsView.MenuItems>(headerText, adminApp)
{
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.Orders, "Orderstatistik" },
        { MenuItems.Profits, "Inkomststatistik" }
    };

    protected override async Task OnRenderMenuAsync()
    {
        await DisplayGeneralStatistics();
    }

    private async Task DisplayGeneralStatistics()
    {
        var totalCustomers = await AdminApp.Database.GetTotalAmountOfCustomersAsync();
        var totalOrders = await AdminApp.Database.GetTotalAmountOfOrdersAsync();
        var totalSoldProducts = await AdminApp.Database.GetTotalAmountOfSoldProductsAsync();

        Console.WriteLine($"""
            Totalt antal kunder: {totalCustomers} | Totalt antal ordrar: {totalOrders} | Totalt antal sålda produkter: {totalSoldProducts}

            """);
    }

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.Orders:
                await new AdminOrderStatisticsView("Orderstatistik", AdminApp).ActivateAsync();
                break;
            case MenuItems.Profits:
                await new AdminProfitStatisticsView("Inkomststatistik", AdminApp).ActivateAsync();
                break;
        }
    }

    internal enum MenuItems
    {
        Orders = 1,
        Profits,
    }
}
