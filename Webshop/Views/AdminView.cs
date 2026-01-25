
namespace Webshop.Views;

internal class AdminView(string headerText, WebshopApplication app) : MenuBase<AdminView.MenuItems>(headerText, app)
{
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => throw new NotImplementedException();

    private protected override Task ExecuteUserMenuChoiceAsync(int choice)
    {
        throw new NotImplementedException();
    }

    internal enum MenuItems { }
}
