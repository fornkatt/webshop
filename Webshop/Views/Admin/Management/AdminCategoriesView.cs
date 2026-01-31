using Webshop.Helpers;

namespace Webshop.Views.Admin.Management;

internal sealed class AdminCategoriesView(string headerText, AdminApplication adminApp) :
    AdminMenuBase<AdminCategoriesView.MenuItems>(headerText, adminApp)
{
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.AddCategory, "Lägg till ny kategori" },
        { MenuItems.EditOrRemoveCategory, "Redigera eller ta bort kategori" }
    };

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.AddCategory:
                await AddCategoryAsync();
                break;
            case MenuItems.EditOrRemoveCategory:
                await EditOrRemoveCategoryAsync();
                break;
        }
    }

    private async Task EditOrRemoveCategoryAsync()
    {
        try
        {
            Console.Clear();

            MessageHelper.ShowHeader("Redigera eller ta bort kategori");

            Console.WriteLine("Nuvarande kategorier:");
            Console.WriteLine();

            var categories = await AdminApp.Database.GetAllCategoriesAsync();

            for (int i = 0; i < categories.Count; i++)
            {
                var category = categories[i];

                Console.WriteLine($"""
                    {i + 1}.    {category.Name}
                    Aktiv:      {(category.IsActive ? "Ja" : "Nej")}

                    """);
            }

            Console.Write("Välj kategori att ta bort eller redigera (eller Q för att avbryta): ");

            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input) || input.Equals("Q", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            if (int.TryParse(input, out int choice) && (choice >= 1 && choice <= categories.Count))
            {
                var selectedCategory = categories[choice - 1];

                Console.Clear();
                Console.WriteLine($"""
                    Vill du [T]a bort eller [R]edigera kategorin ({selectedCategory.Name})?
                    Q för att avbryta.

                    OBS! Går endast att ta bort om kategorin inte innehåller produkter.
                    Vill du 'ta bort' en kategori, fundera istället på en 'soft-delete' genom att sätta kategorin till inaktiv i redigeringsläget!

                    """);

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Q:
                        return;
                    case ConsoleKey.T:
                        await AdminApp.Database.RemoveCategoryAsync(selectedCategory);

                        MessageHelper.ShowSuccess($"""
                        Kategorin: {selectedCategory.Name}
                        
                        Har tagits bort!
                        """);
                        break;
                    case ConsoleKey.R:
                        await EditCategoryAsync(selectedCategory);
                        break;
                }
            }
        }
        catch (Exception e)
        {
            MessageHelper.ShowError($"Något gick fel: {e.Message}.");
        }
    }

    private async Task EditCategoryAsync(Models.Category category)
    {
        try
        {
            while (true)
            {
                DisplayCategoryEditMenu(category);

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Q:
                        return;
                    case ConsoleKey.N:
                        category.Name = InputHelper.GetTextInput("Nytt namn");
                        break;
                    case ConsoleKey.A:
                        category.IsActive = !category.IsActive;
                        break;
                    case ConsoleKey.M:
                        await SaveCategoryChanges(category);
                        return;
                }
            }
        }
        catch (Exception e)
        {
            MessageHelper.ShowError($"Något gick fel: {e.Message}.");
        }
    }

    private async Task SaveCategoryChanges(Models.Category category)
    {
        Console.Clear();

        await AdminApp.Database.UpdateCategoryAsync(category);

        MessageHelper.ShowSuccess("Ändringar sparade!");
    }

    private void DisplayCategoryEditMenu(Models.Category category)
    {
        Console.Clear();

        MessageHelper.ShowHeader("Redigera kategori");

        Console.WriteLine($"""
            Tryck på en av följande knappar för att ändra:
            [N] Namn

            Toggla true/false:
            [A] Aktiv

            [Q] Avbryt [M] Spara ändringar


            {new string('─', 50)}

            Namn:   {category.Name}
            Aktiv:  {(category.IsActive ? "Ja" : "Nej")}

            {new string('─', 50)}

            """);
    }

    private async Task AddCategoryAsync()
    {
        try
        {
            Console.Clear();

            MessageHelper.ShowHeader("Lägg till kategori");

            Console.WriteLine("""
                Nuvarande kategorier:

                """);

            var categories = await AdminApp.Database.GetAllCategoriesAsync();

            foreach (var item in categories)
            {
                Console.WriteLine($"""
                    Namn:   {item.Name}
                    Aktiv:  {(item.IsActive ? "Ja" : "Nej")}

                    """);
            }

            var name = InputHelper.GetTextInput("(EXIT för att avsluta) Ange namn");

            if (string.IsNullOrWhiteSpace(name) || name.Equals("EXIT", StringComparison.OrdinalIgnoreCase)) return;

            var category = new Models.Category
            {
                Name = name,
                IsActive = true
            };

            if (InputHelper.GetConfirmation($"Ny kategori [{category.Name}] kommer att läggas till. Är detta okej?"))
            {
                await AdminApp.Database.AddCategoryAsync(category);

                MessageHelper.ShowSuccess($"Kategorin [{category.Name}] har lagts till!");
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
        AddCategory = 1,
        EditOrRemoveCategory = 2,
    }
}
