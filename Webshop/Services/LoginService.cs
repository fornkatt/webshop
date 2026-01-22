using Webshop.Helpers;

namespace Webshop.Services;

internal class LoginService(WebshopApplication app)
{
    protected WebshopApplication App { get; } = app;

    internal async Task LoginAsync()
    {
        Console.CursorVisible = true;

        List<Models.Customer> customers = await App.DatabaseService.GetAllCustomersAsync();

        var (adminUsername, adminPassword) = ConfigHelper.GetAdminCredentials();

        while (true)
        {
            Console.Write("(Q för att avsluta) Ange användarnamn: ");

            var username = Console.ReadLine();

            if (string.IsNullOrEmpty(username) || username.ToUpper() == "Q")
            {
                Console.CursorVisible = false;
                return;
            }

            Console.Write("(Q för att avsluta) Ange lösenord: ");

            var password = Console.ReadLine();

            if (string.IsNullOrEmpty(password) || password.ToUpper() == "Q")
            {
                Console.CursorVisible = false;
                return;
            }


            // ToDo: Login as Admin
            if (username == adminUsername && password == adminPassword)
            {
                Console.WriteLine("Grattis!");
                return;
            }

            var customer = customers.FirstOrDefault(c => c.Username == username && c.Password == password);

            if (customer == null)
            {
                Console.WriteLine();
                Console.WriteLine("Ogiltigt användarnamn eller lösenord.");
                Console.WriteLine();
                Console.ReadKey(true);
                continue;
            }

            App.CurrentUser = customer;

            Console.CursorVisible = false;
            return;
        }
    }
}
