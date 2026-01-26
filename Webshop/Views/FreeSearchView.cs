namespace Webshop.Views;

// This view does not extend MenuBase, I made this decision because it makes blocking I/O calls so displaying PersistentMenuItems here is pointless.
// It also does not need to be coupled to MenuBase since we probably do not want to go to a separate view from here
// we can simply exit to the previous view and go from there.
// It is a view all the same because it displays information and handles I/O.
internal class FreeSearchView(WebshopApplication app)
{
    protected WebshopApplication App { get; } = app;

    internal async Task ActivateAsync()
    {
        try
        {
            Console.CursorVisible = true;

            while (true)
            {
                var input = GetSearchInput();
                if (input == null) return;

                var searchedItems = await App.DatabaseService.GetSearchedItems(input);

                if (!HandleEmptyResults(searchedItems))
                {
                    continue;
                }

                DisplaySearchResults(searchedItems);

                var selectedItem = GetProductSelection(searchedItems);

                if (selectedItem != null)
                {
                    await NavigateToProduct(selectedItem);
                }

                return;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ett fel inträffade, försök igen. {e.Message}");
        }
        finally
        {
            Console.CursorVisible = false;
        }
    }

    private string? GetSearchInput()
    {
        while (true)
        {
            Console.Clear();
            Console.Write("""
            Sök efter valfri produkt med fritext. Skriv "exit" för att avsluta.

            Söktext: 
            """);

            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                continue;
            }
            if (input!.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return input;
        }
    }

    private bool HandleEmptyResults(List<Models.Product> results)
    {
        if (results.Count > 0) return true;

        Console.WriteLine("""

            Inga produkter hittades.

            Tryck på valfri tangent för att försöka igen...
            """);
        Console.ReadKey(true);
        return false;
    }

    private Models.Product? GetProductSelection(List<Models.Product> items)
    {
        Console.WriteLine($"""

            Välj produkt (1 - {items.Count}) eller skriv 'exit' för att avsluta.

            """);

        var input = Console.ReadLine();

        if (string.IsNullOrEmpty(input) ||
            input.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
        if (int.TryParse(input, out int index) &&
            index >= 1 && index <= items.Count)
        {
            return items[index - 1];
        }

        Console.WriteLine("Ogiltigt val. Tryck på valfri tangent för att fortsätta.");
        Console.ReadKey(true);
        return null;
    }

    private async Task NavigateToProduct(Models.Product product)
    {
        await new ProductView(product.Name!, product, App).ActivateAsync();
    }

    private void DisplaySearchResults(List<Models.Product> searchedItems)
    {
        Console.Clear();
        Console.WriteLine($"""
            Antal träffar: {searchedItems.Count}

            """);

        for (int i = 0; i < searchedItems.Count; i++)
        {
            var item = searchedItems[i];
            Console.WriteLine($"""
                {i + 1}. {item.Name}
                {item.Price:C}

                """);
        }
    }
}