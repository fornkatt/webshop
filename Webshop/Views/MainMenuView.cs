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

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.Categories:
                _categories ??= new CategoriesView("Kategorier", App);
                await _categories.ActivateAsync();
                break;
            case MenuItems.SaleItems:
                Console.WriteLine($"Selected: Reavaror");
                Console.ReadKey(true);
                break;
        }
    }

    private protected override async Task RenderMenuAsync()
    {
        await base.RenderMenuAsync();

        await ShowSelectedItemsAsync();
    }

    private async Task ShowSelectedItemsAsync()
    {
        List<Models.Product> selectedItems = await App.DatabaseService.GetSelectedProductsAsync();

        foreach (var product in selectedItems)
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
