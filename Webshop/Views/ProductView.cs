namespace Webshop.Views;

internal class ProductView(Models.Product product)
{
    private Models.Product Product { get; set; } = product;
}
