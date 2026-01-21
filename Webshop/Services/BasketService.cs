using Webshop.Models;

namespace Webshop.Services
{
    internal class BasketService
    {
        private readonly Dictionary<int, (Product Product, int Quantity)> _basketItems = [];

        internal void AddToBasket(Product product)
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
        }

        internal void RemoveFromBasket(Product product)
        {
            if (_basketItems.ContainsKey(product.Id))
            {
                var item = _basketItems[product.Id];
                if (item.Quantity > 1)
                {
                    _basketItems[product.Id] = (item.Product, item.Quantity - 1);
                }
                else
                {
                    _basketItems.Remove(product.Id);
                }
            }
        }

        internal void ListBasketItems()
        {
            if (_basketItems.Count == 0)
            {
                Console.WriteLine("Varukorgen är tom. Get shopping!");
                Console.WriteLine();
                return;
            }

            foreach (var item in _basketItems.Values)
            {
                var totalProductPrice = (item.Product.Price ?? 0) * item.Quantity;
                Console.WriteLine($"""
                        {item.Quantity}st {item.Product.Name} - {totalProductPrice} kr

                        """);
            }
        }

        internal int GetTotaltemCount() => _basketItems.Values.Sum(i => i.Quantity);

        internal decimal GetTotal() => _basketItems.Values.Sum(i => (i.Product.Price ?? 0) * i.Quantity);

        internal void Clear() => _basketItems.Clear();
    }
}
