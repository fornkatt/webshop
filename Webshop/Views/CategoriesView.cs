namespace Webshop.Views;

internal class CategoriesView(string headerText, WebshopApplication app) : MenuBase<CategoriesView.MenuItems>(headerText, app)
{
    internal enum MenuItems { }

    private List<Models.Category> _categories =
        [
            new Models.Category { Name = "Datorkomponenter" },
            new Models.Category { Name = "Mjukvara" },
            new Models.Category { Name = "Bildskärmar" },
            new Models.Category { Name = "Datortillbehör" },
        ];

    protected override void RenderMenu()
    {
        Console.Clear();

        Console.Write(HeaderText);
        if (App.CurrentUser != null)
        {
            Console.WriteLine($"\t\t\t\t\tInloggad som: {App.CurrentUser.Name}\tÄr gäst: {App.CurrentUser.IsGuest}\tÄr inloggad: {App.IsLoggedIn}");
        }
        Console.WriteLine();

        RenderPersistentMenuItems();

        Console.WriteLine();
        Console.WriteLine();

        for (int i = 0; i < _categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_categories[i].Name}");
        }

        Console.WriteLine();
    }

    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => [];

    private protected override void ExecuteUserMenuChoice(int choice)
    {
        if (choice >= 1 && choice <= _categories.Count)
        {
            var category = _categories[choice - 1];
            Console.WriteLine($"Selected: {category.Name}");
            Console.ReadKey(true);
        }
    }
}
