namespace Webshop.Views;


// Does not extend MenuBase because it simply asks user if they want to log out or not.
// Simply pressing any other key than "Y" will put them back in the previous view.
// I opted for having it in Views because it handles I/O and displays information.
internal class LogoutView(WebshopApplication app)
{
    protected WebshopApplication App { get; } = app;

    internal async Task ActivateAsync()
    {
        try
        {
            Console.CursorVisible = false;

            RenderMessage();

            await GetUserConfirmation();
        }
        finally
        {
            Console.CursorVisible = false;
        }
    }

    private void RenderMessage()
    {
        Console.Clear();
        Console.WriteLine("""
        Är du säker på att du vill logga ut?
        
        Y/n
        """);
    }

    private async Task GetUserConfirmation()
    {
        while (true)
        {
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Y)
            {
                App.CurrentUser = new()
                {
                    FirstName = "guestUser" + Random.Shared.Next(0, 10000000),
                    IsGuest = true
                };
                await App.GoToMainMenuAsync();
                return;
            }
            if (key == ConsoleKey.N)
            {
                return;
            }
        }
    }
}