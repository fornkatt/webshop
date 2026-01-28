using Webshop.Models;

namespace Webshop.Views;

internal class BasketView(string headerText, WebshopApplication app) : MenuBase<BasketView.MenuItems>(headerText, app)
{
    private int _currentPage = 0;
    private const int ItemsPerPage = 8;

    protected override async Task OnRenderMenuAsync()
    {
        DisplayBasketItems();
    }

    private void HandleSelectedProduct((Product Product, int Quantity) selectedItem)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"""
            {selectedItem.Product.Name}
            {selectedItem.Quantity} st
            Totalt pris: {selectedItem.Product.Price * selectedItem.Quantity:C}

            [Y] öka antal
            [Z] minska antal
            [X] radera produkt
            [S] spara
            [Q] gå tillbaka utan att spara
            """);

            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Y)
            {
                selectedItem.Quantity++;
            }
            if (key == ConsoleKey.Z)
            {
                if (selectedItem.Quantity <= 1) continue;
                selectedItem.Quantity--;
            }
            if (key == ConsoleKey.X)
            {
                App.Basket.RemoveFromBasket(selectedItem.Product);
                return;
            }
            if (key == ConsoleKey.S)
            {
                App.Basket.ReplaceBasketItem((selectedItem.Product, selectedItem.Quantity));
                return;
            }
            if (key == ConsoleKey.Q)
            {
                return;
            }
        }
    }

    private void DisplayBasketItems()
    {
        var basketItems = App.Basket.GetItems();
        var pageItems = basketItems.Skip(_currentPage * ItemsPerPage).Take(ItemsPerPage).ToList();

        int i = 1;

        foreach (var item in pageItems)
        {
            var totalProductPrice = (item.Product.Price ?? 0) * item.Quantity;
            Console.WriteLine($"""

                {i}. {item.Product.Name} - {totalProductPrice} kr
                    {item.Quantity} st

                """);
            i++;
        }

        Console.WriteLine($"""
            Antal artiklar i varukorgen:    {App.Basket.GetTotalItemCount()}
            Total kostnad:                  {App.Basket.GetTotal()}
            Varav moms:                     {App.Basket.GetTotalTax()}

            Välj en produkt för att ändra! (Sida {_currentPage + 1}/{(basketItems.Count + ItemsPerPage - 1) / ItemsPerPage})
            """);
    }

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        var basketItems = App.Basket.GetItems();
        var totalPages = Math.Max(1, (basketItems.Count + ItemsPerPage - 1) / ItemsPerPage);

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

        var pageItems = basketItems.Skip(_currentPage * ItemsPerPage).Take(ItemsPerPage).ToList();

        if (choice >= 1 && choice <= pageItems.Count)
        {
            var selectedItem = pageItems[choice - 1];

            HandleSelectedProduct(selectedItem);
        }
    }
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
}