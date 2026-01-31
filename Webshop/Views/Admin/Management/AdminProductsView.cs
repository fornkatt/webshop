using Webshop.Helpers;

namespace Webshop.Views.Admin.Management;

internal sealed class AdminProductsView(string headerText, AdminApplication adminApp) :
    AdminMenuBase<AdminProductsView.MenuItems>(headerText, adminApp)
{
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.AddProduct, "Lägg till ny produkt" },
        { MenuItems.ListProducts, "Lista befintliga produkter" },
        { MenuItems.EditOrRemoveProduct, "Redigera eller ta bort produkt" }
    };

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.ListProducts:
                await ListAllProductsAsync();
                MessageHelper.ShowSuccess("Alla produkter listade!");
                break;
            case MenuItems.AddProduct:
                await AddProductAsync();
                break;
            case MenuItems.EditOrRemoveProduct:
                await EditOrRemoveProductAsync();
                break;
        }
    }

    private async Task ListAllProductsAsync()
    {
        var products = await AdminApp.Database.GetAllProductsAsync();

        foreach (var product in products)
        {
            Console.WriteLine($"""
                {new string('─', 50)}

                Namn:               {product.Name}
                Kategori:           {product.Category!.Name}
                Leverantör:         {product.Supplier!.Name}
                Kort beskrivning:   {product.ShortDescription}
                Nuvarande pris:     {product.Price:C}
                Originalpris:       {product.OriginalPrice:C}
                Lager:              {product.Stock}
                Aktiv:              {(product.IsActive ? "Ja" : "Nej")}
                Vald:               {(product.IsSelectedItem ? "Ja" : "Nej")}
                Rea:                {(product.IsSaleItem ? "Ja" : "Nej")}
                Utgått:             {(product.IsDiscontinued ? "Ja" : "Nej")}
                Ugick:              {product.DiscontinuedDate}

                """);
        }
    }

    private async Task EditOrRemoveProductAsync()
    {
        try
        {
            var categories = await AdminApp.Database.GetAllCategoriesAsync();
            var categoryId = AdminHelper.SelectItem("Välj en kategori:", categories, c => c.Id, c => c.Name);

            if (categoryId == null) return;

            var products = await AdminApp.Database.GetProductsPerCategoryAsync(categoryId);
            while (true)
            {
                Console.Clear();

                for (int i = 0; i < products.Count; i++)
                {
                    var product = products[i];
                    Console.WriteLine($"""
                    {i + 1}.    {product.Name}

                    """);
                }

                Console.Write("Välj en produkt att ta bort eller redigera (eller Q för att avbryta): ");

                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input) || input.Equals("Q", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
                if (int.TryParse(input, out int choice) && (choice >= 1 && choice <= products.Count))
                {
                    var selectedProduct = products[choice - 1];

                    Console.Clear();
                    Console.WriteLine($"""
                    Vill du [T]a bort eller [R]edigera produkten ({selectedProduct.Name})?

                    OBS! Går endast att ta bort produkten om den inte finns med i ordrar!
                    Vill du 'ta bort' en produkt, fundera istället på en 'soft-delete' genom att sätta produkten till inaktiv och/eller utgått i redigeringsläget!
                    
                    Q för att avbryta.
                    
                    """);
                    var key = Console.ReadKey(true).Key;

                    switch (key)
                    {
                        case ConsoleKey.Q:
                            return;
                        case ConsoleKey.T:
                            await AdminApp.Database.RemoveProductAsync(selectedProduct);

                            MessageHelper.ShowSuccess($"""
                    Produkten: {selectedProduct.Name}

                    Har tagits bort!
                    """);

                            return;
                        case ConsoleKey.R:
                            await EditProductAsync(selectedProduct);
                            return;
                    }
                }
                else
                {
                    MessageHelper.ShowError("Ogiltigt val. Försök igen!");
                    continue;
                }
            }
        }
        catch(Exception e)
        {
            MessageHelper.ShowError($"Ett fel inträffade: {e.Message}");
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
                    product.Name = InputHelper.GetTextInput("Nytt namn");
                    break;
                case ConsoleKey.K:
                    product.ShortDescription = InputHelper.GetTextInput("Ny kort beskrivning");
                    break;
                case ConsoleKey.D:
                    product.DetailedDescription = InputHelper.GetTextInput("Ny detaljerad beskrivning");
                    break;
                case ConsoleKey.C:
                    await UpdateCategory(product);
                    break;
                case ConsoleKey.L:
                    await UpdateSupplier(product);
                    break;
                case ConsoleKey.S:
                    product.Stock = InputHelper.GetIntInput("Nytt lagersaldo");
                    break;
                case ConsoleKey.P:
                    product.Price = InputHelper.GetDecimalInput("Nytt pris");
                    break;
                case ConsoleKey.O:
                    product.OriginalPrice = InputHelper.GetDecimalInput("Nytt originalpris");
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
        Console.Clear();

        MessageHelper.ShowHeader("Redigera produkt");

        Console.WriteLine($"""
            Tryck på en av följande knappar för att ändra:
            [N] Namn
            [K] Kort beskrivning
            [D] Detaljerad beskrivning
            [C] Kategori
            [L] Leverantör
            [S] Lagersaldo
            [P] Pris
            [O] Originalpris

            Toggla sant/falskt:
            [R] Reavara [V] Vald produkt [A] Aktiv [U] Utgått

            [Q] Avbryt [M] Spara ändringar

            {new string('─', 50)}

            Namn:                   {product.Name}
            Kort beskrivning:       {product.ShortDescription}
            Detaljerad beskrivning: {product.DetailedDescription}
            Kategori:               {product.Category?.Name}
            Leverantör:             {product.Supplier?.Name}
            Lagersaldo:             {product.Stock}
            Nuvarande pris:         {product.Price:C}
            Originalpris:           {product.OriginalPrice:C}
            Vald produkt:           {(product.IsSelectedItem ? "Ja" : "Nej")}
            Reavara:                {(product.IsSaleItem ? "Ja" : "Nej")}
            Aktiv:                  {(product.IsActive ? "Ja" : "Nej")}
            Utgått:                 {(product.IsDiscontinued ? "Ja" : "Nej")}

            {new string('─', 50)}

            """);
    }

    private async Task UpdateCategory(Models.Product product)
    {
        var categories = await AdminApp.Database.GetAllCategoriesAsync();
        var categoryId = AdminHelper.SelectItem(
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
        var suppliers = await AdminApp.Database.GetAllSuppliersAsync();
        var supplierId = AdminHelper.SelectItem(
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

        await AdminApp.Database.UpdateProductAsync(product);

        MessageHelper.ShowSuccess("Ändringar sparade!");
    }

    private async Task AddProductAsync()
    {
        try
        {
            Console.Clear();

            MessageHelper.ShowHeader("Lägg till produkt");

            var categories = await AdminApp.Database.GetAllCategoriesAsync();
            var categoryId = AdminHelper.SelectItem("Välj en kategori:", categories, c => c.Id, c => c.Name);

            if (categoryId == null) return;

            var suppliers = await AdminApp.Database.GetAllSuppliersAsync();
            var supplierId = AdminHelper.SelectItem("Välj en leverantör:", suppliers, s => s.Id, s => s.Name!);

            if (supplierId == null) return;

            var name = InputHelper.GetTextInput("Produktnamn");
            var shortDesc = InputHelper.GetTextInput("Kort beskrivning");
            var detailedDesc = InputHelper.GetTextInput("Detaljerad beskrivning");

            Console.WriteLine();
            Console.Write("Pris: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                MessageHelper.ShowError("Ogiltigt pris!");
                return;
            }

            Console.WriteLine();
            Console.Write("Lagersaldo: ");
            if (!int.TryParse(Console.ReadLine(), out int stock))
            {
                MessageHelper.ShowError("Ogiltigt lagersaldo!");
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

            await AdminApp.Database.AddNewProductAsync(product);

            await AdminApp.MongoLogService.LogActionAsync("Ny produkt tillagd", null, "admin", null, product.Name);

            MessageHelper.ShowSuccess($"Ny produkt ({product.Name}) tillagd!");
        }
        catch(Exception e)
        {
            MessageHelper.ShowError($"Något gick fel. {e.Message}.");
        }
    }

    internal enum MenuItems
    {
        ListProducts = 1,
        AddProduct,
        EditOrRemoveProduct,
    }
}