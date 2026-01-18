
namespace Webshop.Views;

internal class MainMenu(string headerText) : MenuBase<MainMenu.MenuItems>(headerText)
{
    private Categories? _categories;
    //private List<Product> SelectedItems { get; set; } = saleItems;
    internal enum MenuItems
    {
        LogIn = 1,
        LogOut,
        Basket,
        Categories,
        Exit = 0
    }

    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.LogIn, "Logga in" },
        { MenuItems.LogOut, "Logga ut" },
        { MenuItems.Basket, "Varukorg" },
        { MenuItems.Categories, "Kategorier" },
        { MenuItems.Exit, "Avsluta" }
    };

    private protected override void ExecuteUserMenuChoice(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.LogIn:
                break;
            case MenuItems.LogOut:
                break;
            case MenuItems.Basket:
                break;
            case MenuItems.Categories:
                _categories ??= new Categories("Kategorier");
                _categories.Activate();
                break;
            case MenuItems.Exit:
                Environment.Exit(0);
                break;
        }
    }

    private void ShowSelectedItems()
    {

    }
}
