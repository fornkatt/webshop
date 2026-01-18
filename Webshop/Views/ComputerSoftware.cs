
namespace Webshop.Views;

internal class ComputerSoftware(string headerText) : MenuBase<ComputerSoftware.MenuItems>(headerText)
{
    internal enum MenuItems
    {

    }
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {

    };

    private protected override void ExecuteUserMenuChoice(int choice)
    {
        switch ((MenuItems)choice)
        {

        }
    }
}
