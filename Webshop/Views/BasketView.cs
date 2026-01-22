namespace Webshop.Views;

internal class BasketView(string headerText, WebshopApplication app) : MenuBase<BasketView.MenuItems>(headerText, app)
{
    private CheckoutView? _checkoutView;
    internal enum MenuItems 
    { 
        Clear = 1,
        Checkout
    }
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.Clear, "Töm varukorg" },
        { MenuItems.Checkout, "Checka ut" }
    };

    private protected override async Task RenderMenuAsync()
    {
        await base.RenderMenuAsync();

        App.Basket.ListBasketItems();

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine($"Antal artiklar i varukorgen: {App.Basket.GetTotalItemCount()}");
        Console.WriteLine();
        Console.WriteLine($"Total kostnad: {App.Basket.GetTotal()}");
        Console.WriteLine($"Varav moms: {App.Basket.GetTotalTax()}");
    }

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        switch ((MenuItems)choice) 
        {
            case MenuItems.Clear:
                App.Basket.Clear();
                break;
            case MenuItems.Checkout:
                if (App.Basket.GetTotalItemCount() <= 0) return;
                _checkoutView ??= new CheckoutView(App);
                await _checkoutView.ActivateAsync();
                break;
        }
    }
}