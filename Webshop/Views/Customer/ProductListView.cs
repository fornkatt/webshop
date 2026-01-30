namespace Webshop.Views.Customer;

internal class ProductListView(string headerText, List<Models.Product> products, WebshopApplication app) : MenuBase<ProductListView.MenuItems>(headerText, app)
{
    private readonly List<Models.Product> _products = products;
    internal enum MenuItems { }
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => [];

    protected override async Task OnRenderMenuAsync()
    {
        for (int i = 0; i < _products.Count; i++)
        {
            var product = _products[i];
            Console.WriteLine($""" 
                {i + 1}. {product.Name}
                {product.ShortDescription}
                {product.OriginalPrice:C} {(product.IsSaleItem ? $"-> {product.Price}" : "")}
                {(product.Stock > 0 ? "I lager" : "Slut i lager")}
                {(product.IsSaleItem ? "REA!" : "")}
                {(product.IsSelectedItem ? "UTVALD!" : "")}

                """);
        }
        Console.WriteLine();
    }
    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        if (choice >= 1 && choice <= _products.Count)
        {
            var selectedProduct = _products[choice - 1];

            var productView = new ProductView(selectedProduct.Name!, selectedProduct, App);
            await productView.ActivateAsync();
        }
    }
}