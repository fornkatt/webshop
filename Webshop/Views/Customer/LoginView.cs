namespace Webshop.Views.Customer;

// Does not extend MenuBase because it makes blocking I/O calls to handle user login.
// Displaying PersistentMenuItems and handling menu navigation is pointless here.
// I still put it in Views because it displays information and handles I/O and has minimal business logic.
internal class LoginView(WebshopApplication app)
{
    private readonly WebshopApplication _app = app;

    internal async Task ActivateAsync()
    {
        try
        {
            Console.CursorVisible = true;

            List<Models.Customer> customers = await _app.Database.GetAllCustomersAsync();

            var (adminUsername, adminPassword) = Helpers.ConfigHelper.GetAdminCredentials();

            while (true)
            {
                Console.Clear();

                Console.WriteLine("""
                    Logga in:

                    """);

                Console.Write("(Q för att avsluta) Ange användarnamn: ");

                var username = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(username) || username.Equals("Q", StringComparison.CurrentCultureIgnoreCase))
                {
                    return;
                }

                Console.WriteLine();

                Console.Write("(Q för att avsluta) Ange lösenord: ");

                var password = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(password) || password.Equals("Q", StringComparison.CurrentCultureIgnoreCase))
                {
                    return;
                }


                if (username == adminUsername && password == adminPassword)
                {
                    await _app.MongoLogService.LogActionAsync("Admin inloggning", null, "admin");
                    var adminApp = new AdminApplication();
                    await adminApp.GoToAdminViewAsync();
                    return;
                }

                var customer = customers.FirstOrDefault(c => c.Username == username && c.Password == password && c.IsActive == true);

                if (customer == null)
                {
                    Console.WriteLine("""

                        Ogiltigt användarnamn eller lösenord. Försök igen.

                        """);
                    Console.ReadKey(true);
                    continue;
                }

                _app.CurrentUser = customer;
                await _app.MongoLogService.LogActionAsync("Inloggning", _app.CurrentUser.Id, _app.CurrentUser.FirstName, _app.CurrentUser.Email);
                return;
            }
        }
        catch (Exception e)
        {
            Helpers.MessageHelper.ShowError($"Något gick fel: { e.Message}.");
        }
        finally
        {
            Console.CursorVisible = false;
        }
    }
}