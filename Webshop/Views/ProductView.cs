namespace Webshop.Views;

internal class ProductView(string headerText, Models.Product product, WebshopApplication app) : MenuBase<ProductView.MenuItems>(headerText, app)
{
    internal enum MenuItems
    {
        AddItem = 1
    }
    private Models.Product Product { get; set; } = product;

    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.AddItem, "Lägg till i varukorgen." }
    };

    protected override async Task OnRenderMenuAsync()
    {
        Console.WriteLine($"""
            {Product.ShortDescription}

            {Product.DetailedDescription}

            {Product.Price}
            """);
    }

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.AddItem:
                App.Basket.AddToBasket(Product);
                break;
        }
    }
}