namespace Webshop.Views.Customer;

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

    private protected override async Task RenderMenuAsync()
    {
        try
        {
            Console.CursorVisible = false;

            Console.WriteLine();

            foreach (MenuItems item in Enum.GetValues(typeof(MenuItems)))
            {
                Console.WriteLine($"{Convert.ToInt16(item)}. {MenuItemLocalizedNames[item]}");
            }

            Console.WriteLine();
            Helpers.MessageHelper.ShowHeader(HeaderText);

            Console.WriteLine($"""
            {Product.ShortDescription}

            {Product.DetailedDescription}

            {Product.Price}
            """);
        }
        finally
        {
            Console.CursorVisible = false;
        }
    }

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.AddItem:
                await App.Basket.AddToBasket(Product, App.CurrentUser.Id, App.CurrentUser.FirstName, App.CurrentUser.Email);
                break;
        }
    }
}