using Webshop.Models;

namespace Webshop.Views.Customer;

internal sealed class CategoriesView(string headerText, WebshopApplication app) : MenuBase<CategoriesView.MenuItems>(headerText, app)
{
    private int _currentPage = 0;
    private const int ItemsPerPage = 8;

    private List<Models.Category> _categories = [];

    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.NextPage, "Nästa sida"  },
        { MenuItems.PreviousPage, "Föregående sida" }
    };

    internal enum MenuItems
    {
        NextPage = 0,
        PreviousPage = 9
    }

    protected override async Task OnRenderMenuAsync()
    {
        if (_categories.Count == 0)
        {
            _categories = await App.Database.GetAllCategoriesAsync();
        }

        var pageItems = _categories.Skip(_currentPage * ItemsPerPage).Take(ItemsPerPage).ToList();

        Console.WriteLine();
        for (int i = 0; i < pageItems.Count; i++)
        {
            var category = pageItems[i];
            Console.WriteLine($"{i + 1}. {category.Name}");
        }
        Console.WriteLine($"""

            Sida {_currentPage + 1}/{(_categories.Count + ItemsPerPage) / ItemsPerPage}
            """);
    }

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        var totalPages = Math.Max(1, (_categories.Count + ItemsPerPage - 1) / ItemsPerPage);

        if (choice == (int)MenuItems.NextPage)
        {
            if (_currentPage < totalPages - 1)
            {
                _currentPage++;
            }
            return;
        }

        if (choice == (int)MenuItems.PreviousPage)
        {
            if (_currentPage > 0)
            {
                _currentPage--;
            }
            return;
        }

        var pageItems = _categories.Skip(_currentPage * ItemsPerPage).Take(ItemsPerPage).ToList();

        if (choice >= 1 && choice <= pageItems.Count)
        {
            var category = pageItems[choice - 1];

            var products = await App.Database.GetProductsPerCategoryAsync(category.Id);

            var productListView = new ProductListView(category.Name, products, App);
            await productListView.ActivateAsync();
        }
    }
}
