namespace Webshop.Views;

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

            List<Models.Customer> customers = await App.DatabaseService.GetAllCustomersAsync();

            var (adminUsername, adminPassword) = Helpers.ConfigHelper.GetAdminCredentials();

            while (true)
            {
                Console.Clear();

                Console.WriteLine("""
                    Logga in:

                    """);

                Console.Write("(Q för att avsluta) Ange användarnamn: ");

                var username = Console.ReadLine();

                if (string.IsNullOrEmpty(username) || username.ToUpper() == "Q")
                {
                    return;
                }

                Console.Write("(Q för att avsluta) Ange lösenord: ");

                var password = Console.ReadLine();

                if (string.IsNullOrEmpty(password) || password.ToUpper() == "Q")
                {
                    return;
                }


                // ToDo: Login as Admin
                if (username == adminUsername && password == adminPassword)
                {
                    Console.WriteLine("Grattis!");
                    Console.ReadKey();
                    return;
                }

                var customer = customers.FirstOrDefault(c => c.Username == username && c.Password == password);

                if (customer == null)
                {
                    Console.WriteLine("""

                        Ogiltigt användarnamn eller lösenord.

                        """);
                    Console.ReadKey(true);
                    continue;
                }

                App.CurrentUser = customer;

                return;
            }
        }
        finally
        {
            Console.CursorVisible = false;
        }
    }
}