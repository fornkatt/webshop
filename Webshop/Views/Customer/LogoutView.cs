namespace Webshop.Views.Customer;


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

            Console.Clear();

            Helpers.MessageHelper.ShowHeader("Utloggning");

            if (!Helpers.InputHelper.GetConfirmation("Är du säker på att du vill logga ut?")) return;

            await App.MongoLogService.LogActionAsync("Kund utloggad", App.CurrentUser.Id, 
                App.CurrentUser.FirstName, App.CurrentUser.Email);

            App.CurrentUser = new()
            {
                FirstName = "guestUser" + Random.Shared.Next(0, 10000000),
                IsGuest = true
            };
            await App.GoToMainMenuAsync();
            return;
        }
        finally
        {
            Console.CursorVisible = false;
        }
    }
}