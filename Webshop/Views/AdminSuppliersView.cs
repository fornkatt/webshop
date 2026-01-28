using Webshop.Helpers;

namespace Webshop.Views;

internal class AdminSuppliersView(string headerText, AdminApplication adminApp) : 
    AdminMenuBase<AdminSuppliersView.MenuItems>(headerText, adminApp)
{
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.AddSupplier, "Lägg till ny leverantör" },
        { MenuItems.EditOrRemoveSupplier, "Redigera eller ta bort leverantör" }
    };

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.AddSupplier:
                await AddSupplier();
                break;
            case MenuItems.EditOrRemoveSupplier:
                await EditOrRemoveSupplierAsync();
                break;
        }
    }

    private async Task EditOrRemoveSupplierAsync()
    {
        try
        {
            Console.Clear();

            MessageHelper.ShowHeader("Redigera eller ta bort leverantör");

            Console.WriteLine("""
                Nuvarande leverantörer:

                """);

            var suppliers = await AdminApp.Database.GetAllSuppliersForAdminAsync();

            for (int i = 0; i < suppliers.Count; i++)
            {
                var supplier = suppliers[i];

                Console.WriteLine($"""
                    {i + 1}.    {supplier.Name}

                    """);
            }

            Console.Write("Välj leverantör att ta bort eller redigera (eller Q för att avbryta): ");

            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input) || input.Equals("Q", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            if (int.TryParse(input, out int choice) && (choice >= 1 && choice <= suppliers.Count))
            {
                var selectedSupplier = suppliers[choice - 1];

                Console.Clear();
                Console.WriteLine($"""
                    Vill du [T]a bort eller [R]edigera leverantören ({selectedSupplier.Name})?
                    Q för att avbryta.

                    OBS! Går endast att ta bort om leverantören inte har produkter i sortimentet.

                    """);

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Q:
                        return;
                    case ConsoleKey.T:
                        await AdminApp.Database.RemoveSupplierForAdminAsync(selectedSupplier);

                        MessageHelper.ShowSuccess($"""
                        Leverantören: {selectedSupplier.Name}
                        
                        Har tagits bort!
                        """);
                        break;
                    case ConsoleKey.R:
                        await EditSupplierAsync(selectedSupplier);
                        break;
                }
            }
        }
        catch (Exception e)
        {
            MessageHelper.ShowError($"Något gick fel: {e.Message}.");
        }
    }

    private async Task EditSupplierAsync(Models.Supplier supplier)
    {
        try
        {
            while (true)
            {
                DisplaySupplierEditMenu(supplier);

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Q:
                        return;
                    case ConsoleKey.N:
                        supplier.Name = InputHelper.GetTextInput("Nytt namn");
                        break;
                    case ConsoleKey.M:
                        await SaveSupplierChanges(supplier);
                        return;
                }
            }
        }
        catch (Exception e)
        {
            MessageHelper.ShowError($"Något gick fel: {e.Message}.");
        }
    }

    private async Task SaveSupplierChanges(Models.Supplier supplier)
    {
        Console.Clear();

        await AdminApp.Database.UpdateSupplierForAdminAsync(supplier);

        MessageHelper.ShowSuccess("Ändringar sparade!");
    }

    private void DisplaySupplierEditMenu(Models.Supplier supplier)
    {
        Console.Clear();

        MessageHelper.ShowHeader("Redigera kategori");

        Console.WriteLine($"""
            Tryck på en av följande knappar för att ändra:
            [N] Namn

            [Q] Avbryt [M] Spara ändringar

            {new string('─', 50)}

            Namn:   {supplier.Name}

            {new string('─', 50)}
            """);
        Console.WriteLine("Tryck på en av följande knappar för att ändra:");
        Console.WriteLine("[N] Namn");
        Console.WriteLine();
        Console.WriteLine("[Q] Avbryt [M] Spara ändringar");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(new string('─', 50));
        Console.WriteLine($"Namn: {supplier.Name}");
        Console.WriteLine(new string('─', 50));
        Console.WriteLine();
    }

    private async Task AddSupplier()
    {
        try
        {
            Console.Clear();

            MessageHelper.ShowHeader("Lägg till leverantör");

            var suppliers = await AdminApp.Database.GetAllSuppliersForAdminAsync();

            foreach (var supplier in suppliers)
            {
                Console.WriteLine($"""
                    {supplier.Name}

                    """);
            }

            var name = InputHelper.GetTextInput("(EXIT för att avsluta) Ange namn");

            if (string.IsNullOrWhiteSpace(name) || name.Equals("EXIT", StringComparison.OrdinalIgnoreCase)) return;

            var newSupplier = new Models.Supplier
            { 
                Name = name 
            };

            if (InputHelper.GetConfirmation($"Ny leverantör [{newSupplier.Name}] kommer att läggas till. Är detta okej?"))
            {
                await AdminApp.Database.AddSupplierForAdminAsync(newSupplier);

                MessageHelper.ShowSuccess($"Leverantören [{newSupplier.Name}] har lagts till!");
                return;
            }
            return;
        }
        catch (Exception e)
        {
            MessageHelper.ShowError($"Något gick fel: { e.Message}.");
        }
    }

    internal enum MenuItems
    {
        AddSupplier = 1,
        EditOrRemoveSupplier
    }
}
