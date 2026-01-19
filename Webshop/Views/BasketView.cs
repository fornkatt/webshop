
namespace Webshop.Views;

internal class BasketView(string headerText, WebshopApplication app) : MenuBase<BasketView.MenuItems>(headerText, app)
{
    public List<Models.Product>? Products { get; set; }
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => [];

    private protected override void ExecuteUserMenuChoice(int choice)
    {
        throw new NotImplementedException();
    }

    internal enum MenuItems 
    { 

    }
}
