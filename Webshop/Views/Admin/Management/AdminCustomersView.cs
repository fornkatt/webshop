using Webshop.Helpers;

namespace Webshop.Views.Admin.Management;

internal sealed class AdminCustomersView(string headerText, AdminApplication adminApp) : 
    AdminMenuBase<AdminCustomersView.MenuItems>(headerText, adminApp)
{
    private protected override Dictionary<MenuItems, string> MenuItemLocalizedNames => new()
    {
        { MenuItems.ListCustomers, "Orderhistorik" },
        { MenuItems.AddCustomer, "Lägg till ny kund" },
        { MenuItems.EditOrRemoveCustomer, "Redigera eller ta bort kund" }
    };

    private protected override async Task ExecuteUserMenuChoiceAsync(int choice)
    {
        switch ((MenuItems)choice)
        {
            case MenuItems.ListCustomers:
                await ViewOrderHistoryAsync();
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

    private async Task ViewOrderHistoryAsync()
    {
        try
        {
            Console.Clear();
            MessageHelper.ShowHeader("Se orderhistorik");

            var customers = await AdminApp.Database.GetAllCustomersAsync();

            ListAllCustomers(customers);

            var input = InputHelper.GetIntInput("Välj en kund att visa") - 1;
            Console.WriteLine();

            if (input > customers.Count || input < 0)
            {
                MessageHelper.ShowError("Ogiltigt val");
                return;
            }

            var customer = customers[input];

            Console.Clear();
            DisplaySelectedCustomer(customer);

            var orders = await AdminApp.Database.GetCustomerOrdersAsync(customer.Id);

            ListCustomerOrders(orders);

            MessageHelper.ShowSuccess("Ordrar listade!");

            if (!InputHelper.GetConfirmation("Vill du även se ordervaror?")) return;

            var choice = InputHelper.GetIntInput("Välj order (Ej order-ID)") - 1;
            Console.WriteLine();

            if (choice >= 0 && choice < orders.Count)
            {
                var order = orders[choice];

                var orderItems = order.OrderItems;

                ListOrderItems(orderItems);

                MessageHelper.ShowSuccess("Produkter listade!");
                return;
            }
            MessageHelper.ShowError("Ogiltigt val!");
            return;
        }
        catch (Exception e)
        {
            MessageHelper.ShowError($"Något gick fel: {e.Message}");
        }
    }

    private void DisplaySelectedCustomer(Models.Customer customer)
    {
        Console.WriteLine($"""
                ================ {customer.FirstName} {customer.LastName} ================
                
                Telefon:    {customer.Phone}
                Melj:       {customer.Email}
                Ålder:      {customer.Age}
                Aktiv:      {(customer.IsActive ? "Ja" : "Nej")}

                {new string('─', 50)}

                Land:       {customer.Address!.Country!.Name}
                Län:        {customer.Address.Region}
                Stad:       {customer.Address.City}
                Postkod:    {customer.Address.PostalCode}
                Gata:       {customer.Address.Street}
                Husnummer:  {customer.Address.HouseNumber}
                
                """);
    }

    private void ListOrderItems(List<Models.OrderItem> orderItems)
    {
        foreach (var item in orderItems)
        {
            Console.WriteLine($"""
                Produkt:        {item.ProductName}
                Mängd:          {item.Quantity}
                Styckpris:      {item.UnitPrice:C}

                Totalt pris:    {item.Subtotal:C}

                """);
        }
    }

    private void ListCustomerOrders(List<Models.Order> orders)
    {
        for (int i = 0; i < orders.Count; i++)
        {
            var order = orders[i];

            Console.WriteLine($"""
                {i + 1}.
                Order-ID:            {order.Id}
                Orderdatum:         {order.OrderDate:f}
                Betalningsmetod:    {order.PaymentMethod?.Name ?? "Okänd"}
                Transaktionsavgift: {order.PaymentMethodCost:C}
                Fraktmetod:         {order.ShippingMethod?.Name ?? "Okänd"}
                Fraktkostnad:       {order.ShippingCost:C}

                Total kostnad:      {order.TotalAmount:C}

                """);
        }
    }

    private void ListAllCustomers(List<Models.Customer> customers)
    {
        for (int i = 0; i < customers.Count; i++)
        {
            var customer = customers[i];

            Console.WriteLine($"""
                ================ {customer.FirstName} {customer.LastName} ================
                
                {i + 1}.

                Telefon:    {customer.Phone}
                Melj:       {customer.Email}
                Ålder:      {customer.Age}
                Aktiv:      {(customer.IsActive ? "Ja" : "Nej")}

                {new string('─', 50)}

                Land:       {customer.Address!.Country!.Name}
                Län:        {customer.Address.Region}
                Stad:       {customer.Address.City}
                Postkod:    {customer.Address.PostalCode}
                Gata:       {customer.Address.Street}
                Husnummer:  {customer.Address.HouseNumber}
                
                """);
        }
        Console.WriteLine(new string('─', 50));
    }

    private async Task EditOrRemoveCustomerAsync()
    {
        try
        {
            var customers = await AdminApp.Database.GetAllCustomersAsync();

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

                    OBS! Går endast att ta bort kunden om de inte har ordrar!
                    Vill du 'ta bort' en kund, fundera istället på en 'soft-delete' genom att sätta kunden till inaktiv i redigeringsläget!

                    Q för att avbryta.

                    """);

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Q:
                        return;
                    case ConsoleKey.T:
                        await AdminApp.Database.RemoveCustomerAsync(selectedCustomer);
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
                    await SaveCustomerChangesAsync(customer);
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
                    await EditCustomerAddressAsync(customer.Id);
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

    private async Task SaveCustomerChangesAsync(Models.Customer customer)
    {
        Console.Clear();

        customer.Address = null;

        await AdminApp.Database.UpdateCustomerAsync(customer);

        MessageHelper.ShowSuccess("Ändringar sparade!");
    }

    private async Task EditCustomerAddressAsync(int customerId)
    {
        var address = await AdminApp.Database.GetCustomerAddressAsync(customerId);

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
                    await SaveAddressChangesAsync(address);
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
                case ConsoleKey.R:
                    address.Region = InputHelper.GetTextInput("Nytt län");
                    break;
            }
        }
    }

    private async Task SaveAddressChangesAsync(Models.Address address)
    {
        Console.Clear();

        address.Customer = null;

        await AdminApp.Database.UpdateCustomerAddressAsync(address);

        MessageHelper.ShowSuccess("Ändringar sparade!");
    }

    private void DisplayAddressEditMenu(Models.Address address)
    {
        Console.Clear();

        MessageHelper.ShowHeader("Redigera address");

        Console.WriteLine($"""
            Tryck på en av följande knappar för att ändra:
            [R] Län
            [N] Stad
            [K] Postkod
            [D] Gata
            [C] Husnummer

            [Q] Avbryt [M] Spara ändringar

            {new string('─', 50)}

            Län:        {address.Region}
            Stad:       {address.City}
            Postkod:    {address.PostalCode}
            Gata:       {address.Street}
            Husnummer:  {address.HouseNumber}

            {new string('─', 50)}

            """);
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

                {customer.Address.Region}
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
        var region = InputHelper.GetTextInput("Ange län");
        var city = InputHelper.GetTextInput("Ange stad");
        var postalCode = InputHelper.GetIntInput("Ange postkod");
        var street = InputHelper.GetTextInput("Ange gata");
        var houseNumber = InputHelper.GetTextInput("Ange husnummer");

        var address = new Models.Address()
        {
            CountryId = 1,
            Region = region,
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
