namespace Webshop.Views;

internal abstract class MenuBase<TMenuItems>(string headerText) : IMenu where TMenuItems : Enum
{
    private protected string HeaderText { get; set; } = headerText;
    private protected abstract Dictionary<TMenuItems, string> MenuItemLocalizedNames { get; }
    private bool _shouldExit = false;

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
    private void RenderMenu()
    {
        Console.Clear();

        Console.WriteLine(HeaderText);
        Console.WriteLine();

        foreach (TMenuItems item in Enum.GetValues(typeof(TMenuItems)))
        {
            Console.WriteLine($"{Convert.ToInt16(item)}. {MenuItemLocalizedNames[item]}");
        }

        Console.WriteLine();
    }

    private void ValidateUserInput()
    {
        if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int choice))
        {
            return;
        }

        if (!Enum.IsDefined(typeof(TMenuItems), choice))
        {
            return;
        }

        ExecuteUserMenuChoice(choice);
    }

    private protected abstract void ExecuteUserMenuChoice(int choice);
}
