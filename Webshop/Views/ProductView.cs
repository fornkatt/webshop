
namespace Webshop.Views;

internal class ProductView(string headerText, Models.Product product, WebshopApplication app) : MenuBase<ProductView.MenuItems>(headerText, app)
{
    internal enum MenuItems { }
    private Models.Product Product { get; set; } = product;

    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => throw new NotImplementedException();

    private protected override void RenderMenu()
    {
        base.RenderMenu();

        Console.WriteLine($"""
            {Product.ShortDescription}

            {Product.DetailedDescription}

            {Product.Price}

            Tryck 1 för att lägga till a varukorgen.
            """);
    }

    private protected override void ExecuteUserMenuChoice(int choice)
    {
        switch (choice) 
        {
            case 1:
                App.Basket.AddToBasket(Product);
                break;
        }
    }
}
