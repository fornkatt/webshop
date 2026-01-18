
namespace Webshop.Views;

internal class Categories(string headerText) : MenuBase<Categories.MenuItems>(headerText)
{
    internal enum MenuItems
    {
        ComputerHardware = 1,
        ComputerSoftware,
        Monitors,
        Basket,
        Back,
        Exit = 0
    }

    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.ComputerHardware, "Datorkomponenter" },
        { MenuItems.ComputerSoftware, "Mjukvara" },
        { MenuItems.Monitors, "Bildskärmar" },
        { MenuItems.Basket, "Varukorg" },
        { MenuItems.Back, "Backa" },
        { MenuItems.Exit, "Avsluta" }
    };

    private protected override void ExecuteUserMenuChoice(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.ComputerHardware:
                break;
            case MenuItems.ComputerSoftware:
                break;
            case MenuItems.Monitors:
                break;
            case MenuItems.Basket:
                break;
            case MenuItems.Back:
                ExitMenu();
                break;
            case MenuItems.Exit:
                Environment.Exit(0);
                break;
        }
    }
}
