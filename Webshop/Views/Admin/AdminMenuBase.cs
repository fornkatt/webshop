using Webshop.Views.Admin.Management;
using Webshop.Views.Shared;

namespace Webshop.Views.Admin;

internal abstract class AdminMenuBase<TMenuItems>(string headerText, AdminApplication adminApp) : IMenu
    where TMenuItems : Enum
{
    private protected AdminApplication AdminApp { get; } = adminApp;
    private protected string HeaderText { get; set; } = headerText;
    private protected abstract Dictionary<TMenuItems, string> MenuItemLocalizedNames { get; }
    private bool _shouldExit = false;

    private enum PersistentMenuItems
    {
        Exit = 'A',
        Back = 'B',
        Home = 'S'
    }

    private Dictionary<PersistentMenuItems, string> PersistentMenuItemsLocalizedNames => new()
    {
        { PersistentMenuItems.Exit, "Tillbaka till shoppen" },
        { PersistentMenuItems.Back, "Backa" },
        { PersistentMenuItems.Home, "Admin-startsida" }
    };

    public virtual async Task ActivateAsync()
    {
        _shouldExit = false;
        while (!_shouldExit)
        {
            Console.Clear();
            RenderPersistentMenuItems();
            await RenderMenuAsync();
            await ValidateUserInputAsync();
        }
    }

    private protected void ExitMenu()
    {
        _shouldExit = true;
    }

    private void RenderPersistentMenuItems()
    {
        foreach (PersistentMenuItems item in Enum.GetValues(typeof(PersistentMenuItems)))
        {
            if (ShouldHideMenuItem(item))
            {
                continue;
            }
            Console.Write($"[{(char)item}] {PersistentMenuItemsLocalizedNames[item], -20}");
        }
        Console.WriteLine();
    }

    private protected virtual async Task RenderMenuAsync()
    {
        try
        {
            Console.CursorVisible = true;

            Console.WriteLine();
            Helpers.MessageHelper.ShowHeader(HeaderText);

            foreach (TMenuItems item in Enum.GetValues(typeof(TMenuItems)))
            {
                Console.WriteLine($"{Convert.ToInt16(item)}. {MenuItemLocalizedNames[item]}");
            }
            Console.WriteLine();

            await OnRenderMenuAsync();
        }
        catch (Exception e)
        {
            Helpers.MessageHelper.ShowError($"Något gick fel: {e.Message}");
        }
        finally
        {
            Console.CursorVisible = true;
        }
    }

    protected virtual Task OnRenderMenuAsync() => Task.CompletedTask;

    private bool ShouldHideMenuItem(PersistentMenuItems item)
    {
        return item switch
        {
            PersistentMenuItems.Exit when this is not AdminView => true,
            PersistentMenuItems.Back when this is AdminView => true,
            PersistentMenuItems.Home when this is AdminView => true,
            _ => false
        };
    }

    private async Task ValidateUserInputAsync()
    {
        var input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            char inputChar = input.ToUpper().First();

            if (Enum.IsDefined(typeof(PersistentMenuItems), (int)inputChar))
            {
                await ExecutePersistentUserMenuChoiceAsync(inputChar);
            }
            else if (int.TryParse(inputChar.ToString(), out int choice))
            {
                await ExecuteUserMenuChoiceAsync(choice);
            }
        }
    }

    private async Task ExecutePersistentUserMenuChoiceAsync(int choice)
    {
        switch ((PersistentMenuItems)choice)
        {
            case PersistentMenuItems.Exit:
                if (this is not AdminView) return;
                ExitMenu();
                break;
            case PersistentMenuItems.Back:
                if (this is AdminView) return;
                ExitMenu();
                break;
            case PersistentMenuItems.Home:
                if (this is AdminView) return;
                ExitMenu();
                await AdminApp.GoToAdminViewAsync();
                break;
        }
    }

    private protected abstract Task ExecuteUserMenuChoiceAsync(int choice);
}