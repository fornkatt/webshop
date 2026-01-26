using Webshop.Helpers;

namespace Webshop.Views;

internal class AdminProductsView(string headerText, AdminApplication adminApp) :
    AdminMenuBase<AdminProductsView.MenuItems>(headerText, adminApp)
{
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.AddProduct, "Lägg till produkt" },
        { MenuItems.EditOrRemoveProduct, "Ta bort produkt eller redigera" },
    };

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.AddProduct:
                await AddProductAsync();
                break;
            case MenuItems.EditOrRemoveProduct:
                await EditOrRemoveProductAsync();
                break;
        }
    }

    private async Task EditOrRemoveProductAsync()
    {
        try
        {
            var categories = await AdminApp.Database.GetAllCategoriesForAdminAsync();
            var categoryId = await SelectItemAsync("Välj en kategori:", categories, c => c.Id, c => c.Name);

            if (categoryId == null) return;

            var products = await AdminApp.Database.GetProductsPerCategoryForAdminAsync(categoryId);

            Console.Clear();
            Console.WriteLine();

            for (int i = 0; i < products.Count; i++)
            {
                var product = products[i];
                Console.WriteLine($"{i + 1}. {product.Name}");
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.Write("Välj en produkt att ta bort eller redigera (eller Q för att avbryta): ");

            var input = Console.ReadLine();


            if (string.IsNullOrEmpty(input) || input.Equals("Q", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            if (int.TryParse(input, out int choice) && (choice >= 1 && choice <= products.Count))
            {
                var selectedProduct = products[choice - 1];

                Console.Clear();
                Console.WriteLine($"Vill du [T]a bort eller [R]edigera produkten ({selectedProduct.Name})? ");
                Console.WriteLine("Q för att avbryta.");
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Q)
                {
                    return;
                }
                else if (key == ConsoleKey.T)
                {
                    await AdminApp.Database.DeleteProductForAdminAsync(selectedProduct);

                    Console.WriteLine($"""
                    Produkten: {selectedProduct.Name}

                    Har tagits bort!

                    Tryck på valfri tangent för att fortsätta.
                    """);
                    Console.ReadKey(true);
                }
                else if (key == ConsoleKey.R)
                {
                    await EditProductAsync(selectedProduct);
                }
            }
            else
            {
                Console.WriteLine("""
                Ogiltigt val. Försök igen!

                Tryck på valfri tangent för att fortsätta.
                """);
                Console.ReadKey(true);
            }
        }
        catch(Exception e)
        {
            Console.WriteLine($"Ett fel inträffade: {e.Message}");
        }
    }

    private async Task EditProductAsync(Models.Product product)
    {
        while (true)
        {
            DisplayProductEditMenu(product);

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.Q:
                    return;
                case ConsoleKey.M:
                    await SaveProductChanges(product);
                    return;
                case ConsoleKey.N:
                    product.Name = AdminInputHelper.GetTextInput("Nytt namn");
                    break;
                case ConsoleKey.K:
                    product.ShortDescription = AdminInputHelper.GetTextInput("Ny kort beskrivning");
                    break;
                case ConsoleKey.D:
                    product.DetailedDescription = AdminInputHelper.GetTextInput("Ny detaljerad beskrivning");
                    break;
                case ConsoleKey.C:
                    await UpdateCategory(product);
                    break;
                case ConsoleKey.L:
                    await UpdateSupplier(product);
                    break;
                case ConsoleKey.S:
                    product.Stock = AdminInputHelper.GetIntInput("Nytt lagersaldo");
                    break;
                case ConsoleKey.P:
                    product.Price = AdminInputHelper.GetDecimalInput("Nytt pris");
                    break;
                case ConsoleKey.O:
                    product.OriginalPrice = AdminInputHelper.GetDecimalInput("Nytt originalpris");
                    break;
                case ConsoleKey.R:
                    product.IsSaleItem = !product.IsSaleItem;
                    break;
                case ConsoleKey.V:
                    product.IsSelectedItem = !product.IsSelectedItem;
                    break;
                case ConsoleKey.A:
                    product.IsActive = !product.IsActive;
                    break;
                case ConsoleKey.U:
                    product.IsDiscontinued = !product.IsDiscontinued;
                    break;
            }
        }
    }

    private void DisplayProductEditMenu(Models.Product product)
    {
        AdminInputHelper.ShowHeader("Redigera produkt");
        Console.WriteLine("Tryck på en av följande knappar för att ändra:");
        Console.WriteLine("[N] Namn [K] Kort beskrivning [D] Detaljerad beskrivning");
        Console.WriteLine("[C] Kategori [L] Leverantör [S] Lagersaldo");
        Console.WriteLine("[P] Pris [O] Originalpris");
        Console.WriteLine();
        Console.WriteLine("Toggla true/false:");
        Console.WriteLine("[R] Reavara [V] Vald produkt [A] Aktiv [U] Utgått");
        Console.WriteLine();
        Console.WriteLine("[Q] Avbryt [M] Spara ändringar\n");
        Console.WriteLine();
        Console.WriteLine(new string('─', 50));
        Console.WriteLine($"Namn: {product.Name}");
        Console.WriteLine($"Kort beskrivning: {product.ShortDescription}");
        Console.WriteLine($"Detaljerad beskrivning: {product.DetailedDescription}");
        Console.WriteLine($"Kategori: {product.Category?.Name}");
        Console.WriteLine($"Leverantör: {product.Supplier?.Name}");
        Console.WriteLine($"Lagersaldo: {product.Stock}");
        Console.WriteLine($"Pris: {product.Price:C}");
        Console.WriteLine($"Originalpris: {product.OriginalPrice:C}");
        Console.WriteLine($"Vald produkt: {(product.IsSelectedItem ? "Ja" : "Nej")}");
        Console.WriteLine($"Reavara: {(product.IsSaleItem ? "Ja" : "Nej")}");
        Console.WriteLine($"Aktiv: {(product.IsActive ? "Ja" : "Nej")}");
        Console.WriteLine($"Utgått: {(product.IsDiscontinued ? "Ja" : "Nej")}");
        Console.WriteLine(new string('─', 50));
        Console.WriteLine();
    }

    private async Task UpdateCategory(Models.Product product)
    {
        var categories = await AdminApp.Database.GetAllCategoriesForAdminAsync();
        var categoryId = await SelectItemAsync(
            "Välj ny kategori:",
            categories,
            c => c.Id,
            c => c.Name
        );

        if (categoryId != null)
        {
            product.CategoryId = categoryId;
            var selectedCategory = categories.FirstOrDefault(c => c.Id == categoryId);
            product.Category = selectedCategory;
        }
    }

    private async Task UpdateSupplier(Models.Product product)
    {
        var suppliers = await AdminApp.Database.GetAllSuppliersForAdminAsync();
        var supplierId = await SelectItemAsync(
            "Välj ny leverantör:",
            suppliers,
            s => s.Id,
            s => s.Name!
        );

        if (supplierId != null)
        {
            product.SupplierId = supplierId.Value;
            var selectedSupplier = suppliers.FirstOrDefault(s => s.Id == supplierId);
            product.Supplier = selectedSupplier;
        }
    }

    private async Task SaveProductChanges(Models.Product product)
    {
        Console.Clear();

        product.Category = null;
        product.Supplier = null;

        await AdminApp.Database.ReplaceProductForAdminAsync(product);

        Console.WriteLine();
        Console.WriteLine("Ändringar sparade!");
        Console.WriteLine();
        Console.WriteLine("Tryck på valfri tangent för att fortsätta.");
        Console.ReadKey(true);
    }

    private async Task AddProductAsync()
    {
        try
        {
            AdminInputHelper.ShowHeader("Lägg till produkt");

            var categories = await AdminApp.Database.GetAllCategoriesAsync();
            var categoryId = await SelectItemAsync("Välj en kategori:", categories, c => c.Id, c => c.Name);

            if (categoryId == null) return;

            var suppliers = await AdminApp.Database.GetAllSuppliersAsync();
            var supplierId = await SelectItemAsync("Välj en leverantör:", suppliers, s => s.Id, s => s.Name!);

            if (supplierId == null) return;

            var name = AdminInputHelper.GetTextInput("Produktnamn");
            var shortDesc = AdminInputHelper.GetTextInput("Kort beskrivning");
            var detailedDesc = AdminInputHelper.GetTextInput("Detaljerad beskrivning");

            Console.Write("""
                
                Pris: 
                """);
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                AdminInputHelper.ShowError("Ogiltigt pris!");
                return;
            }

            Console.Write("""
                
                Lagersaldo: 
                """);
            if (!int.TryParse(Console.ReadLine(), out int stock))
            {
                AdminInputHelper.ShowError("Ogiltigt lagersaldo!");
                return;
            }

            var product = new Models.Product
            {
                Name = name,
                ShortDescription = shortDesc,
                DetailedDescription = detailedDesc,
                Price = price,
                OriginalPrice = price,
                Stock = stock,
                CategoryId = categoryId,
                SupplierId = supplierId,
                IsSaleItem = false,
                IsSelectedItem = false,
                IsActive = true,
                IsDiscontinued = false
            };

            await AdminApp.Database.AddNewProductForAdminAsync(product);

            Console.WriteLine($"""
                
                Ny produkt ({product.Name}) tillagd!
                """);

            Console.ReadKey(true);
        }
        catch(Exception e)
        {
            Console.WriteLine($"""
                
                Något gick fel. {e.Message}
                """);
        }
    }

    private async Task<int?> SelectItemAsync<T>(string prompt, List<T> items, Func<T, int> idSelector, Func<T, string> nameSelector)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"""
                {prompt}

                """);

            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {nameSelector(items[i])}");
            }
            Console.WriteLine();

            Console.Write("Ditt val (eller Q för att avbryta): ");
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input) || input.Equals("Q", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= items.Count)
            {
                return idSelector(items[choice - 1]);
            }

            Console.WriteLine("""

                Ogiltigt val. Försök igen.
                """);
            Console.ReadKey(true);
        }
    }

    internal enum MenuItems
    {
        AddProduct = 1,
        EditOrRemoveProduct,
    }
}