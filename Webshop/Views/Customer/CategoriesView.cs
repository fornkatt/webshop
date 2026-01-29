namespace Webshop.Views.Customer;

internal class CategoriesView(string headerText, WebshopApplication app) : MenuBase<CategoriesView.MenuItems>(headerText, app)
{
    internal enum MenuItems { }

    private List<Models.Category> _categories = [];

    protected override async Task OnRenderMenuAsync()
    {
        if (_categories.Count == 0)
        {
            _categories = await App.Database.GetAllCategoriesAsync();
        }

        for (int i = 0; i < _categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_categories[i].Name}");
        }
        Console.WriteLine();
    }

    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => [];

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        if (choice >= 1 && choice <= _categories.Count)
        {
            var category = _categories[choice - 1];

            var products = await App.Database.GetProductsPerCategoryAsync(category.Id);

            var productListView = new ProductListView(category.Name, products, App);
            await productListView.ActivateAsync();
        }
    }
}
