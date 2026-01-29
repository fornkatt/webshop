using Webshop.Models;

namespace Webshop.Services;

internal sealed class BasketService(MongoLogService? logService = null)
{
    private readonly Dictionary<int, (Product Product, int Quantity)> _basketItems = [];
    private readonly MongoLogService? _logService = logService;

    internal void ReplaceBasketItem((Product Product, int Quantity) item)
    {
        _basketItems[item.Product.Id] = (item.Product, item.Quantity);
    }

    internal async Task AddToBasket(Product product, int? userId = null, string? name = null, string? email = null)
    {
        if (_basketItems.ContainsKey(product.Id))
        {
            var item = _basketItems[product.Id];
            _basketItems[product.Id] = (item.Product, item.Quantity + 1);
        }
        else
        {
            _basketItems[product.Id] = (product, 1);
        }
        if (_logService != null)
        {
            await _logService.LogActionAsync("Produkt tillagd i varukorgen", userId, name, email, $"Produkt: {product.Name}");
        }
    }

    internal async Task RemoveFromBasket(Product product, int? userId = null, string? name = null, string? email = null)
    {
        if (_basketItems.ContainsKey(product.Id))
        {
            _basketItems.Remove(product.Id);

            if (_logService != null)
            {
                await _logService.LogActionAsync("Produkt borttagen ur varukorgen", userId, name, email, $"Produkt: {product.Name}");
            }
        }
    }

    internal void ListBasketItems()
    {
        if (_basketItems.Count == 0)
        {
            Console.WriteLine("""
                Varukorgen är tom. Get shopping!

                """);
            return;
        }

        int i = 1;
        foreach (var item in _basketItems.Values)
        {
            var totalProductPrice = (item.Product.Price ?? 0) * item.Quantity;
            Console.WriteLine($"""
                    {i}.    {item.Product.Name} - {totalProductPrice} kr
                                                  {item.Quantity} st

                    """);
            i++;
        }
    }

    internal List<(Product Product, int Quantity)> GetItems()
    {
        return _basketItems.Values.ToList();
    }

    internal int GetTotalItemCount() => _basketItems.Values.Sum(i => i.Quantity);

    internal decimal GetTotal() => _basketItems.Values.Sum(i => (i.Product.Price ?? 0) * i.Quantity);

    internal decimal GetTotalTax() => _basketItems.Values.Sum(i => (i.Product.Price ?? 0) * i.Quantity * 0.25m);

    internal void Clear() => _basketItems.Clear();
}
