using System.Text;

namespace Webshop.Views;

internal abstract class MenuBase<TMenuItems>(string headerText, WebshopApplication app) : IMenu where TMenuItems : Enum
{
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
        Search = 'T',
        Checkout = 'C',
        ClearBasket = 'X'
    }
    private static Dictionary<PersistentMenuItems, string> PersistentMenuItemsLocalizedNames => new()
    {
        { PersistentMenuItems.Exit, "Avsluta" },
        { PersistentMenuItems.Login, "Logga in" },
        { PersistentMenuItems.Logout, "Logga ut" },
        { PersistentMenuItems.Basket, "Varukorg" },
        { PersistentMenuItems.Back, "Backa" },
        { PersistentMenuItems.Home, "Startsidan" },
        { PersistentMenuItems.Search, "Sök" },
        { PersistentMenuItems.Checkout, "Kassan" },
        { PersistentMenuItems.ClearBasket, "Töm varukorg" }
    };

    public virtual async Task ActivateAsync()
    {
        _shouldExit = false;
        while (!_shouldExit) 
        {
            Console.Clear();
            await RenderPersistentMenuItemsAsync();
            await RenderMenuAsync();
            await ValidateUserInputAsync();
        }
    }
    private void ExitMenu()
    {
        _shouldExit = true;
    }
    private async Task RenderPersistentMenuItemsAsync()
    {
        foreach (PersistentMenuItems item in Enum.GetValues(typeof(PersistentMenuItems)))
        {
            if (ShouldHideMenuItem(item))
            {
                continue;
            }

            var menuItem = $"[{(char)item}] {PersistentMenuItemsLocalizedNames[item]}";
            Console.Write($"{menuItem, -20}");
        }
        Console.WriteLine($"""
            

            Inloggad som: {App.CurrentUser.FirstName}
            """);
    }
    private protected virtual async Task RenderMenuAsync()
    {
        try
        {
            Console.CursorVisible = false;

            Console.WriteLine($"""

            {HeaderText}

            """);

            foreach (TMenuItems item in Enum.GetValues(typeof(TMenuItems)))
            {
                Console.WriteLine($"{Convert.ToInt16(item)}. {MenuItemLocalizedNames[item]}");
            }
            Console.WriteLine();

            await OnRenderMenuAsync();
        }
        finally
        {
            Console.CursorVisible = false;
        }
    }

    protected virtual Task OnRenderMenuAsync() => Task.CompletedTask;

    private bool ShouldHideMenuItem(PersistentMenuItems item)
    {
        return item switch
        {
            PersistentMenuItems.Basket when this is BasketView or AdminView => true,
            PersistentMenuItems.Back when this is MainMenuView => true,
            PersistentMenuItems.Home when this is MainMenuView => true,
            PersistentMenuItems.Login when App.IsLoggedIn => true,
            PersistentMenuItems.Logout when !App.IsLoggedIn => true,
            PersistentMenuItems.Login when this is AdminView => true,
            PersistentMenuItems.Logout when this is AdminView => true,
            PersistentMenuItems.ClearBasket when this is not BasketView => true,
            PersistentMenuItems.Checkout when this is not BasketView => true,
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
            case PersistentMenuItems.Search:
                await new FreeSearchView(App).ActivateAsync();
                break;
            case PersistentMenuItems.Login:
                if (App.IsLoggedIn || this is AdminView) return;
                await new LoginView(App).ActivateAsync();
                break;
            case PersistentMenuItems.Logout:
                if (App.IsLoggedIn == false || this is AdminView) return;
                await new LogoutView(App).ActivateAsync();
                break;
            case PersistentMenuItems.Basket:
                if (this is BasketView || this is AdminView) return;
                await new BasketView("Varukorgen", App).ActivateAsync(); ;
                break;
            case PersistentMenuItems.Checkout:
                if (this is not BasketView || App.Basket.GetTotalItemCount() <= 0) return;
                await new CheckoutView(App).ActivateAsync();
                break;
            case PersistentMenuItems.ClearBasket:
                if (this is not BasketView) return;
                App.Basket.Clear();
                break;
            case PersistentMenuItems.Back:
                if (this is MainMenuView) return;
                ExitMenu();
                break;
            case PersistentMenuItems.Home:
                if (this is MainMenuView) return;
                ExitMenu();
                await App.GoToMainMenuAsync();
                break;
        }
    }

    private protected abstract Task ExecuteUserMenuChoiceAsync(int choice);
}