
namespace Webshop.Views;

internal class ComputerHardware(string headerText) : MenuBase<ComputerHardware.MenuItems>(headerText)
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
