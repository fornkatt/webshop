using Webshop.Helpers;

namespace Webshop.Views.Customer;

// Does not extend MenuBase because it makes blocking I/O calls to handle user login.
// Displaying PersistentMenuItems and handling menu navigation is pointless here.
// I still put it in Views because it displays information and handles I/O and has minimal business logic.
internal class LoginView(WebshopApplication app)
{
    protected WebshopApplication App { get; } = app;

    internal async Task ActivateAsync()
    {
        try
        {
            Console.CursorVisible = true;

            List<Models.Customer> customers = await App.Database.GetAllCustomersAsync();

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

                App.CurrentUser = customer;

                return;
            }
        }
        catch (Exception e)
        {
            MessageHelper.ShowError($"Något gick fel: { e.Message}.");
        }
        finally
        {
            Console.CursorVisible = false;
        }
    }
}