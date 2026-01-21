namespace Webshop.Views;

internal class ProductListView(string headerText, List<Models.Product> products, WebshopApplication app) : MenuBase<ProductListView.MenuItems>(headerText, app)
{
    private readonly List<Models.Product> _products = products;
    internal enum MenuItems { }
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => [];

    private protected override void RenderMenu()
    {
        base.RenderMenu();

        for (int i = 0; i < _products.Count; i++)
        {
            var product = _products[i];
            Console.WriteLine($""" 
                {i + 1}. {product.Name}
                {product.ShortDescription}
                {product.Price} kr
                {(product.Stock > 0 ? "I lager" : "Slut i lager")}
                """);
            Console.WriteLine();
        }

        Console.WriteLine();
    }
    private protected override void ExecuteUserMenuChoice(int choice)
    {
        if (choice >= 1 && choice <= _products.Count)
        {
            var selectedProduct = _products[choice - 1];

            var productView = new ProductView(selectedProduct.Name, selectedProduct, App);
            productView.Activate();
        }
    }
}