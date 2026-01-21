using Webshop.Services;

namespace Webshop.Views;

internal abstract class MenuBase<TMenuItems>(string headerText, WebshopApplication app) : IMenu where TMenuItems : Enum
{
    private LoginView? _loginView;
    protected WebshopApplication App { get; } = app;
    private protected string HeaderText { get; set; } = headerText;
    private protected abstract Dictionary<TMenuItems, string> MenuItemLocalizedNames { get; }
    private bool _shouldExit = false;
    private enum PersistentMenuItems
    {
        Exit = 'A',
        Login = 'L',
        Logout = 'O',
        Basket = 'V',
        Back = 'B',
        Home = 'S'
    }
    private static Dictionary<PersistentMenuItems, string> PersistentMenuItemsLocalizedNames => new()
    {
        { PersistentMenuItems.Exit, "Avsluta" },
        { PersistentMenuItems.Login, "Logga in" },
        { PersistentMenuItems.Logout, "Logga ut" },
        { PersistentMenuItems.Basket, "Varukorg" },
        { PersistentMenuItems.Back, "Backa" },
        { PersistentMenuItems.Home, "Startsidan" }
    };

    public void Activate()
    {
        _shouldExit = false;
        while (!_shouldExit) 
        {
            RenderMenu();
            ValidateUserInput();
        }
    }
    private protected void ExitMenu()
    {
        _shouldExit = true;
    }
    protected void RenderPersistentMenuItems()
    {
        foreach (PersistentMenuItems item in Enum.GetValues(typeof(PersistentMenuItems)))
        {
            if (ShouldHideMenuItem(item))
            {
                continue;
            }

            Console.Write($"[{(char)item}] {PersistentMenuItemsLocalizedNames[item],-15}");
        }
        Console.WriteLine($"Inloggad som: {App.CurrentUser.FirstName, -15}");
    }
    private protected virtual void RenderMenu()
    {
        Console.Clear();

        RenderPersistentMenuItems();

        Console.WriteLine();
        Console.WriteLine();

        Console.Write(HeaderText);

        Console.WriteLine();
        Console.WriteLine();

        foreach (TMenuItems item in Enum.GetValues(typeof(TMenuItems)))
        {
            Console.WriteLine($"{Convert.ToInt16(item)}. {MenuItemLocalizedNames[item]}");
        }

        Console.WriteLine();
    }

    private bool ShouldHideMenuItem(PersistentMenuItems item)
    {
        return item switch
        {
            PersistentMenuItems.Basket when this is BasketView => true,
            PersistentMenuItems.Back when this is MainMenuView => true,
            PersistentMenuItems.Home when this is MainMenuView => true,
            PersistentMenuItems.Login when App.IsLoggedIn => true,
            PersistentMenuItems.Logout when !App.IsLoggedIn => true,
            _ => false
        };
    }

    private void ValidateUserInput()
    {
        char input = char.ToUpper(Console.ReadKey(true).KeyChar);

        if (Enum.IsDefined(typeof(PersistentMenuItems), (int)input))
        {
            ExecutePersistentUserMenuChoice(input);
        }

        else if (int.TryParse(input.ToString(), out int choice))
        {
            ExecuteUserMenuChoice(choice);
        }
    }

    private void ExecutePersistentUserMenuChoice(int choice)
    {
        switch ((PersistentMenuItems)choice)
        {
            case PersistentMenuItems.Exit:
                Environment.Exit(0);
                break;
            case PersistentMenuItems.Login:
                if (App.IsLoggedIn) return;
                _loginView ??= new LoginView("Inloggning", App);
                _loginView.Activate();
                break;
            case PersistentMenuItems.Logout:
                if (App.IsLoggedIn == false) return;
                new LogoutView(App).Activate();
                break;
            case PersistentMenuItems.Basket:
                if (this is BasketView) return;
                new BasketView("Varukorg", App).Activate();
                break;
            case PersistentMenuItems.Back:
                if (this is MainMenuView) return;
                ExitMenu();
                break;
            case PersistentMenuItems.Home:
                if (this is MainMenuView) return;
                ExitMenu();
                App.ReturnToMainMenu();
                break;
        }
    }

    private protected abstract void ExecuteUserMenuChoice(int choice);
}
