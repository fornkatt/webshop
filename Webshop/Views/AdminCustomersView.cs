
using Webshop.Helpers;

namespace Webshop.Views;

internal class AdminCustomersView(string headerText, AdminApplication adminApp) : 
    AdminMenuBase<AdminCustomersView.MenuItems>(headerText, adminApp)
{
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.ListCustomers, "Lista befintliga kunder" },
        { MenuItems.AddCustomer, "Lägg till ny kund" },
        { MenuItems.EditOrRemoveCustomer, "Redigera eller ta bort kund" }
    };

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.ListCustomers:
                await ListAllCustomersAsync();
                MessageHelper.ShowSuccess("Alla kunder listade!");
                break;
            case MenuItems.AddCustomer:
                await AddCustomerAsync();
                break;
            case MenuItems.EditOrRemoveCustomer:
                await EditOrRemoveCustomerAsync();
                break;
        }
    }

    internal enum MenuItems
    {
        ListCustomers = 1,
        AddCustomer,
        EditOrRemoveCustomer
    }

    internal async Task EditOrRemoveCustomerAsync()
    {
        try
        {
            var customers = await AdminApp.Database.GetAllCustomersForAdminAsync();

            Console.Clear();
            Console.WriteLine();

            for (int i = 0; i < customers.Count; i++)
            {
                var customer = customers[i];
                Console.WriteLine($"""
                    {i + 1}.    {customer.FirstName} {customer.LastName}
                    Telefon:    {customer.Phone}
                    Mejl:       {customer.Email}

                    """);
            }

            Console.Write("Välj en kund att ta bort eller redigera (eller Q för att avbryta): ");

            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input) || input.Equals("Q", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            if (int.TryParse(input, out int choice) && (choice >= 1 && choice <= customers.Count))
            {
                var selectedCustomer = customers[choice - 1];

                Console.Clear();
                Console.WriteLine($"""
                    Vill du [T]a bort eller [R]edigera kunden ({selectedCustomer.FirstName} {selectedCustomer.LastName})?

                    OBS! Går endast att ta bort om kunden om de inte har ordrar!
                    Vill du 'ta bort' en kund, fundera istället på en 'soft-delete' genom att sätta kunden till inaktiv!

                    Q för att avbryta.

                    """);

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Q:
                        return;
                    case ConsoleKey.T:
                        await AdminApp.Database.RemoveCustomerForAdminAsync(selectedCustomer);
                        MessageHelper.ShowSuccess($"""
                    Kunden: {selectedCustomer.FirstName} {selectedCustomer.LastName}

                    Har tagits bort!

                    Tryck på valfri tangent för att fortsätta.
                    """);
                        return;
                    case ConsoleKey.R:
                        await EditCustomerAsync(selectedCustomer);
                        return;
                }
            }
            else
            {
                MessageHelper.ShowError("Ogiltigt val. Försök igen!");
            }
        }
        catch (Exception e)
        {
            MessageHelper.ShowError($"Ett fel inträffade: {e.Message}.");
        }
    }

    private async Task EditCustomerAsync(Models.Customer customer)
    {
        while (true)
        {
            DisplayCustomerEditMenu(customer);

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.Q:
                    return;
                case ConsoleKey.M:
                    await SaveCustomerChanges(customer);
                    return;
                case ConsoleKey.N:
                    customer.FirstName = InputHelper.GetTextInput("Nytt förnamn");
                    break;
                case ConsoleKey.K:
                    customer.LastName = InputHelper.GetTextInput("Nytt efternamn");
                    break;
                case ConsoleKey.D:
                    customer.Email = InputHelper.GetTextInput("Ny mejl");
                    break;
                case ConsoleKey.S:
                    customer.Phone = InputHelper.GetTextInput("Nytt telefonnummer");
                    break;
                case ConsoleKey.P:
                    customer.Age = InputHelper.GetIntInput("Ny ålder");
                    break;
                case ConsoleKey.O:
                    customer.Username = InputHelper.GetTextInput("Nytt användarnamn");
                    break;
                case ConsoleKey.R:
                    customer.Password = InputHelper.GetTextInput("Nytt lösenord");
                    break;
                case ConsoleKey.V:
                    customer.IsActive = !customer.IsActive;
                    break;
                case ConsoleKey.A:
                    await EditCustomerAddress(customer.Id);
                    break;
            }
        }
    }

    private void DisplayCustomerEditMenu(Models.Customer customer)
    {
        Console.Clear();

        MessageHelper.ShowHeader("Redigera kund");

        Console.WriteLine($"""
            Tryck på en av följande knappar för att ändra:
            [N] Förnamn
            [K] Efternamn
            [D] Mejl
            [S] Telefon
            [P] Ålder
            [O] Användarnamn
            [R] Lösenord
            [A] Adress

            Toggla true/false:
            [V] Aktiv

            [Q] Avbryt [M] Spara ändringar

            {new string('─', 50)}

            Förnamn:        {customer.FirstName}
            Efternamn:      {customer.LastName}
            Mejl:           {customer.Email}
            Telefon:        {customer.Phone}
            Ålder:          {customer.Age}
            Användarnamn:   {customer.Username}
            Lösenord:       {customer.Password}
            Aktiv:          {(customer.IsActive ? "Ja" : "Nej")}

            {new string('─', 50)}

            """);
    }

    private async Task SaveCustomerChanges(Models.Customer customer)
    {
        Console.Clear();

        customer.Address = null;

        await AdminApp.Database.UpdateCustomerForAdminAsync(customer);

        MessageHelper.ShowSuccess("Ändringar sparade!");
    }

    private async Task EditCustomerAddress(int customerId)
    {
        var address = await AdminApp.Database.GetCustomerAddressForAdminAsync(customerId);

        while (true)
        {
            if (address == null)
            {
                MessageHelper.ShowError("Ingen adress hittades för kunden.");
                return;
            }

            DisplayAddressEditMenu(address);

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.Q:
                    return;
                case ConsoleKey.M:
                    await SaveAddressChanges(address);
                    return;
                case ConsoleKey.N:
                    address.City = InputHelper.GetTextInput("Ny stad");
                    break;
                case ConsoleKey.K:
                    address.PostalCode = InputHelper.GetIntInput("Ny postkod");
                    break;
                case ConsoleKey.D:
                    address.Street = InputHelper.GetTextInput("Ny gata");
                    break;
                case ConsoleKey.C:
                    address.HouseNumber = InputHelper.GetTextInput("Nytt husnummer");
                    break;
            }
        }
    }

    private async Task SaveAddressChanges(Models.Address address)
    {
        Console.Clear();

        address.Customer = null;

        await AdminApp.Database.UpdateCustomerAddressForAdminAsync(address);

        MessageHelper.ShowSuccess("Ändringar sparade!");
    }

    private void DisplayAddressEditMenu(Models.Address address)
    {
        Console.Clear();

        MessageHelper.ShowHeader("Redigera address");

        Console.WriteLine($"""
            Tryck på en av följande knappar för att ändra:
            [N] Stad
            [K] Postkod
            [D] Gata
            [C] Husnummer

            [Q] Avbryt [M] Spara ändringar

            {new string('─', 50)}

            Stad:       {address.City}
            Postkod:    {address.PostalCode}
            Gata:       {address.Street}
            Husnummer:  {address.HouseNumber}

            {new string('─', 50)}

            """);
    }

    private async Task ListAllCustomersAsync()
    {
        var customers = await AdminApp.Database.GetAllCustomersForAdminAsync();

        foreach (var customer in customers)
        {
            Console.WriteLine($"""
                {new string('─', 50)}

                Namn:       {customer.FirstName} {customer.LastName}
                Telefon:    {customer.Phone}
                Melj:       {customer.Email}
                Ålder:      {customer.Age}
                Aktiv:      {(customer.IsActive ? "Ja" : "Nej")}

                ============== Adress ==============

                Land:       {customer.Address!.Country!.Name}
                Stad:       {customer.Address.City}
                Postkod:    {customer.Address.PostalCode}
                Gata:       {customer.Address.Street}
                Husnummer:  {customer.Address.HouseNumber}
                
                """);
        }
    }

    private async Task AddCustomerAsync()
    {
        try
        {
            Console.Clear();

            MessageHelper.ShowHeader("Lägg till ny kund");

            var address = GetNewAddress();

            var customer = GetNewCustomer();

            customer.Address = address;

            if (!InputHelper.GetConfirmation($"""
                En ny kund {customer.FirstName} med addressen:

                {customer.Address.City}
                {customer.Address.PostalCode}
                {customer.Address.Street}
                {customer.Address.HouseNumber}

                Är detta okej?
                """))
            {
                return;
            }

            await AdminApp.Database.AddNewCustomerAsync(customer);

            MessageHelper.ShowSuccess("Ny kund och address tillagd!");
        }
        catch (Exception e)
        {
            MessageHelper.ShowError($"Något gick fel. {e.InnerException}.");
        }
    }

    private Models.Address GetNewAddress()
    {
        var city = InputHelper.GetTextInput("Ange stad");
        var postalCode = InputHelper.GetIntInput("Ange postkod");
        var street = InputHelper.GetTextInput("Ange gata");
        var houseNumber = InputHelper.GetTextInput("Ange husnummer");

        var address = new Models.Address()
        {
            CountryId = 1,
            City = city,
            PostalCode = postalCode,
            Street = street,
            HouseNumber = houseNumber
        };

        return address;
    }

    private Models.Customer GetNewCustomer()
    {
        var firstName = InputHelper.GetTextInput("Ange förnamn");
        var lastName = InputHelper.GetTextInput("Ange efternamn");
        var email = InputHelper.GetTextInput("Ange email");
        var phone = InputHelper.GetTextInput("Ange telefonnummer");
        var age = InputHelper.GetIntInput("Ange ålder");
        var username = InputHelper.GetTextInput("Ange användarnamn");
        var password = InputHelper.GetTextInput("Ange lösenord");


        var customer = new Models.Customer
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone,
            Age = age,
            Username = username,
            Password = password,
            IsActive = true
        };

        return customer;
    }
}
