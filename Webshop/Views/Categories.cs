
namespace Webshop.Views;

internal class Categories(string headerText) : MenuBase<Categories.MenuItems>(headerText)
{
    private ComputerHardware? _computerHardware;
    private ComputerSoftware? _computerSoftware;
    private Monitors? _monitors;
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
                _computerHardware ??= new ComputerHardware("Datorkomponenter");
                _computerHardware.Activate();
                break;
            case MenuItems.ComputerSoftware:
                _computerSoftware ??= new ComputerSoftware("Mjukvara");
                _computerSoftware.Activate();
                break;
            case MenuItems.Monitors:
                _monitors ??= new Monitors("Bildskärmar");
                _monitors.Activate();
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
