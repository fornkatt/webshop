
namespace Webshop.Views;

internal class MainMenuView(string headerText, WebshopApplication app) : MenuBase<MainMenuView.MenuItems>(headerText, app)
{
    private CategoriesView? _categories;
    //private List<Product> SelectedItems { get; set; } = saleItems;
    internal enum MenuItems
    {
        Categories = 1,
    }

    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.Categories, "Kategorier" },
    };

    private protected override void ExecuteUserMenuChoice(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.Categories:
                _categories ??= new CategoriesView("Kategorier", App);
                _categories.Activate();
                break;
        }
    }

    private void ShowSelectedItems()
    {

    }
}
