namespace Webshop.Views.Customer;


// Does not extend MenuBase because it simply asks user if they want to log out or not.
// Simply pressing any other key than "Y" will put them back in the previous view.
// I opted for having it in Views because it handles I/O and displays information.
internal sealed class LogoutView(WebshopApplication app)
{
    private readonly WebshopApplication _app = app;

    internal async Task ActivateAsync()
    {
        try
        {
            Console.CursorVisible = false;

            Console.Clear();

            Helpers.MessageHelper.ShowHeader("Utloggning");

            if (!Helpers.InputHelper.GetConfirmation("Är du säker på att du vill logga ut?")) return;

            await _app.MongoLogService.LogActionAsync("Kund utloggad", _app.CurrentUser.Id, 
                _app.CurrentUser.FirstName, _app.CurrentUser.Email);

            _app.CurrentUser = new()
            {
                FirstName = "guestUser" + Random.Shared.Next(0, 10000000),
                IsGuest = true
            };
            await _app.GoToMainMenuAsync();
            return;
        }
        finally
        {
            Console.CursorVisible = false;
        }
    }
}