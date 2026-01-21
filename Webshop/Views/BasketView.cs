
namespace Webshop.Views;

internal class BasketView(string headerText, WebshopApplication app) : MenuBase<BasketView.MenuItems>(headerText, app)
{
    //private CheckoutView _checkoutView;
    public List<Models.Product>? Products { get; set; }
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

    private protected override void RenderMenu()
    {
        base.RenderMenu();

        App.Basket.ListBasketItems();

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine($"Antal artiklar i varukorgen: {App.Basket.GetTotaltemCount()}");
        Console.WriteLine();
        Console.WriteLine($"Total kostnad: {App.Basket.GetTotal()}");
    }

    private protected override void ExecuteUserMenuChoice(int choice)
    {
        switch ((MenuItems)choice) 
        {
            case MenuItems.Clear:
                App.Basket.Clear();
                break;
            case MenuItems.Checkout:
                if (App.Basket.GetTotaltemCount() <= 0) return;
                // ToDo: Implement checkout
                //_checkoutView ??= new CheckoutView(App);
                //_checkoutView.Activate();
                break;
        }
    }
}