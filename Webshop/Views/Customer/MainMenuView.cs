using Webshop.Helpers;

namespace Webshop.Views.Customer;

internal sealed class MainMenuView(string headerText, WebshopApplication app) : MenuBase<MainMenuView.MenuItems>(headerText, app)
{
    private List<Models.Product> _selectedProducts = [];

    internal enum MenuItems
    {
        Categories = 4,
    }

    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.Categories, "Kategorier" },
    };

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        switch (choice)
        {
            case 4:
                await new CategoriesView("Kategorier", App).ActivateAsync();
                break;
            case 1 or 2 or 3:
                await HandleSelectedProductAsync(choice);
                break;
        }
    }

    private async Task HandleSelectedProductAsync(int choice)
    {
        var index = choice - 1;

        if (index >= 0 && index < _selectedProducts.Count)
        {
            var selectedProduct = _selectedProducts[index];
            await new ProductView(selectedProduct.Name!, selectedProduct, App).ActivateAsync();
        }
    }

    protected override async Task OnRenderMenuAsync()
    {
        _selectedProducts = await App.Database.GetSelectedProductsAsync();

        Console.WriteLine();
        Console.WriteLine("""






            """);
        MessageHelper.ShowHeader("Utvalda produkter");

        DisplaySelectedProducts();

        Console.WriteLine("""

            Välj en produkt med 1, 2 eller 3 för att gå direkt till produktsidan!
            """);
    }

    private void DisplaySelectedProducts()
    {
        for (int i = 0; i < _selectedProducts.Count && i < 3; i++)
        {
            var product = _selectedProducts[i];
            Console.WriteLine($"""
                {new string('─', 50)}
                {i + 1}. {product.Name}

                {product.OriginalPrice:C} -> {product.Price:C}!

                {(product.Stock > 0 ? "I lager" : "Slut i lager")}
                """);
        }
        Console.WriteLine(new string('─', 50));
    }
}