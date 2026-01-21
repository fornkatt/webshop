
using System.Collections.Immutable;
using System.Text;

namespace Webshop.Views;

internal class MainMenuView(string headerText, WebshopApplication app) : MenuBase<MainMenuView.MenuItems>(headerText, app)
{
    private CategoriesView? _categories;
    internal enum MenuItems
    {
        Categories = 1,
        SaleItems
    }

    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.Categories, "Kategorier" },
        { MenuItems.SaleItems, "Reavaror" }
    };

    private protected override void ExecuteUserMenuChoice(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.Categories:
                _categories ??= new CategoriesView("Kategorier", App);
                _categories.Activate();
                break;
            case MenuItems.SaleItems:
                Console.WriteLine($"Selected: Reavaror");
                Console.ReadKey(true);
                break;
        }
    }

    private protected override void RenderMenu()
    {
        base.RenderMenu();

        ShowSelectedItems();
    }

    private void ShowSelectedItems()
    {
        using var db = new Models.WebshopDbContext();
        List<Models.Product> selectedProducts = App.DatabaseService.GetSelectedProductsAsync(db).GetAwaiter().GetResult();

        foreach (var product in selectedProducts)
        {
            Console.WriteLine($"""
                {product.Name}
                {product.Price}
                {(product.Stock > 0 ? "I lager" : "Slut i lager")}
                """);
            Console.WriteLine();
        }
    }
}
