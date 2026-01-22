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
        Home = 'S',
        Search = 'T'
    }
    private static Dictionary<PersistentMenuItems, string> PersistentMenuItemsLocalizedNames => new()
    {
        { PersistentMenuItems.Exit, "Avsluta" },
        { PersistentMenuItems.Login, "Logga in" },
        { PersistentMenuItems.Logout, "Logga ut" },
        { PersistentMenuItems.Basket, "Varukorg" },
        { PersistentMenuItems.Back, "Backa" },
        { PersistentMenuItems.Home, "Startsidan" },
        { PersistentMenuItems.Search, "Sök" }
    };

    public async Task ActivateAsync()
    {
        _shouldExit = false;
        while (!_shouldExit) 
        {
            await RenderMenuAsync();
            await ValidateUserInputAsync();
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
    private protected virtual async Task RenderMenuAsync()
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

        await OnRenderMenuAsync();
    }

    protected virtual Task OnRenderMenuAsync() => Task.CompletedTask;

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

    private async Task ValidateUserInputAsync()
    {
        char input = char.ToUpper(Console.ReadKey(true).KeyChar);

        if (Enum.IsDefined(typeof(PersistentMenuItems), (int)input))
        {
            await ExecutePersistentUserMenuChoiceAsync(input);
        }

        else if (int.TryParse(input.ToString(), out int choice))
        {
            await ExecuteUserMenuChoiceAsync(choice);
        }
    }

    private async Task ExecutePersistentUserMenuChoiceAsync(int choice)
    {
        switch ((PersistentMenuItems)choice)
        {
            case PersistentMenuItems.Exit:
                Environment.Exit(0);
                break;
            case PersistentMenuItems.Login:
                if (App.IsLoggedIn) return;
                _loginView ??= new LoginView("Inloggning", App);
                await _loginView.ActivateAsync();
                break;
            case PersistentMenuItems.Logout:
                if (App.IsLoggedIn == false) return;
                await new LogoutService(App).LogoutAsync();
                break;
            case PersistentMenuItems.Basket:
                if (this is BasketView) return;
                await new BasketView("Varukorg", App).ActivateAsync();
                break;
            case PersistentMenuItems.Back:
                if (this is MainMenuView) return;
                ExitMenu();
                break;
            case PersistentMenuItems.Home:
                if (this is MainMenuView) return;
                ExitMenu();
                await App.ReturnToMainMenuAsync();
                break;
        }
    }

    private protected abstract Task ExecuteUserMenuChoiceAsync(int choice);
}
