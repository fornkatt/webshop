namespace Webshop.Views.Customer;

internal sealed class ProductListView(string headerText, List<Models.Product> products, WebshopApplication app) : MenuBase<ProductListView.MenuItems>(headerText, app)
{
    private int _currentPage = 0;
    private const int ItemsPerPage = 8;

    private readonly List<Models.Product> _products = products;
    internal enum MenuItems
    {
        NextPage = 0,
        PreviousPage = 9
    }
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.NextPage, "Nästa sida"  },
        { MenuItems.PreviousPage, "Föregående sida" }
    };

    protected override async Task OnRenderMenuAsync()
    {
        var pageItems = _products.Skip(_currentPage * ItemsPerPage).Take(ItemsPerPage).ToList();

        for (int i = 0; i < pageItems.Count; i++)
        {
            var product = pageItems[i];
            Console.WriteLine($""" 

                {i + 1}. {product.Name}
                {product.ShortDescription}
                {product.OriginalPrice:C} {(product.IsSaleItem ? $"-> {product.Price}" : "")}
                {(product.Stock > 0 ? "I lager" : "Slut i lager")}
                {(product.IsSaleItem ? "REA!" : "")}
                {(product.IsSelectedItem ? "UTVALD!" : "")}
                """);
        }
        Console.WriteLine($"""

            Sida {_currentPage + 1}/{(_products.Count + ItemsPerPage) / ItemsPerPage}
            """);
    }
    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        var totalPages = Math.Max(1, (_products.Count + ItemsPerPage - 1) / ItemsPerPage);

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

        var pageItems = _products.Skip(_currentPage * ItemsPerPage).Take(ItemsPerPage).ToList();

        if (choice >= 1 && choice <= pageItems.Count)
        {
            var selectedItem = pageItems[choice - 1];

            var productView = new ProductView(selectedItem.Name!, selectedItem, App);
            await productView.ActivateAsync();
        }
    }
}