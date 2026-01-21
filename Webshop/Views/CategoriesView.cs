namespace Webshop.Views;

internal class CategoriesView : MenuBase<CategoriesView.MenuItems>
{
    internal enum MenuItems { }

    private readonly List<Models.Category> _categories;

    public CategoriesView(string headerText, WebshopApplication app) : base(headerText, app)
    {
        using var db = new Models.WebshopDbContext();
        _categories = App.DatabaseService.GetCategoriesAsync(db).GetAwaiter().GetResult();
    }

    private protected override void RenderMenu()
    {
        base.RenderMenu();

        foreach (var category in _categories)
        {
            Console.WriteLine($"{category.Id}. {category.Name}");
        }

        Console.WriteLine();
    }

    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => [];

    private protected override void ExecuteUserMenuChoice(int choice)
    {
        if (choice >= 1 && choice <= _categories.Count)
        {
            var category = _categories[choice - 1];

            using var db = new Models.WebshopDbContext();
            var products = App.DatabaseService.GetProductsPerCategoryAsync(db, choice).GetAwaiter().GetResult();

            var productListView = new ProductListView(category.Name, products, App);
            productListView.Activate();
        }
    }
}
