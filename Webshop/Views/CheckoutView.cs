using Webshop.Helpers;

namespace Webshop.Views;

// I opted for not extending MenuBase in this view, the reasoning behind this is that while it could technically do that
// this view is heavy on data collection, display and confirmation dialogs which block user input anyway,
// so the user can't navigate the menu items.
internal class CheckoutView(WebshopApplication app)
{
    private readonly Services.CheckoutService _checkoutService = new(app);
    protected WebshopApplication App { get; } = app;

    internal async Task ActivateAsync()
    {
        try
        {
            Console.CursorVisible = true;

            if (App.CurrentUser.Address != null)
            {
                await ShowCurrentAddressAsync();

                if (!GetUserConfirmation())
                {
                    CollectAddressInput();
                }
            }
            else
            {
                CollectAddressInput();
            }

            var shippingMethod = await _checkoutService.GetShippingMethodAsync();
            if (shippingMethod == null) return;

            var paymentMethod = await _checkoutService.GetPaymentMethodAsync();
            if (paymentMethod == null) return;

            await RenderOrderAsync(shippingMethod, paymentMethod);

            if (!GetUserConfirmation())
            {
                return;
            }

            if (await _checkoutService.CompleteOrder(shippingMethod, paymentMethod))
            {
                ShowSuccessMessage();
            }
            else
            {
                ShowErrorMessage("Kunde inte slutföra beställningen eller beställning avbruten.");
            }
        }
        finally
        {
            Console.CursorVisible = false;
        }
    }

    private bool GetUserConfirmation()
    {
        while (true)
        {
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Y)
            {
                return true;
            }
            if (key == ConsoleKey.N)
            {
                return false;
            }
        }
    }

    private void ShowErrorMessage(string message)
    {
        Console.Clear();
        Console.WriteLine($"""
            {message}

            Tryck på valfri tangent för att fortsätta.
            """);
        Console.ReadKey(true);
    }

    private void CollectAddressInput()
    {
        Console.Clear();

        MessageHelper.ShowHeader("Ange ny adress");

        var firstName = InputHelper.GetTextInput("Förnamn");
        var lastName = InputHelper.GetTextInput("Efternamn");
        var city = InputHelper.GetTextInput("Stad");
        var postalCode = InputHelper.GetIntInput("Postkod");
        var street = InputHelper.GetTextInput("Gata");
        var houseNumber = InputHelper.GetTextInput("Husnummer");
        var phone = InputHelper.GetTextInput("Telefonnummer");
        var email = InputHelper.GetTextInput("Mejl");

        _checkoutService.SetCustomerAddress(city, postalCode, street, houseNumber, firstName, lastName, phone, email);
    }

    private void ShowSuccessMessage()
    {
        Console.Clear();
        Console.WriteLine("""
            Grattis till ditt köp!

            Dina varor kommer skickas till dig omgående, senast nästkommande vardag.

            Vi hoppas du är nöjd med din upplevelse hos oss!

            Tryck på valfri tangent för att fortsätta.
            """);

        Console.ReadKey(true);
    }

    private async Task RenderOrderAsync(Models.ShippingMethod shippingMethod, Models.PaymentMethod paymentMethod)
    {
        string countryName = await App.Database.GetCountryName(App.CurrentUser.Address!.CountryId);

        Console.Clear();
        Console.WriteLine($"""
            Din address är:

            Land:               {countryName}
            Stad:               {App.CurrentUser.Address.City}
            Postkod:            {App.CurrentUser.Address.PostalCode}
            Gata:               {App.CurrentUser.Address.Street}
            Husnummer:          {App.CurrentUser.Address.HouseNumber}

            Ditt valda leveransalternativ är:

            Fraktmetod:         {shippingMethod.Name}
            Kostnad:            {shippingMethod.Price}

            Ditt valda betalsätt är:

            Betalmetod:         {paymentMethod.Name}
            Eventuell kostnad:  {paymentMethod.TransactionFee}

            Din order inklusive kostnad:

            """);

        App.Basket.ListBasketItems();

        Console.WriteLine($"""
            Kostnad:            {App.Basket.GetTotal() + paymentMethod.TransactionFee + shippingMethod.Price}
            Varav moms:         {App.Basket.GetTotalTax()}
            
            Stämmer detta? Y/n
            """);
    }

    private async Task ShowCurrentAddressAsync()
    {
        Console.Clear();

        string countryName = await App.Database.GetCountryName(App.CurrentUser.Address!.CountryId);

        var address = App.CurrentUser.Address;

        Console.WriteLine($"""
                Din sparade adress och kontaktuppgifter är:

                Land:       {countryName}
                Stad:       {address.City}
                Postkod:    {address.PostalCode}
                Gata:       {address.Street}
                Husnummer:  {address.HouseNumber}

                Telefon:    {App.CurrentUser.Phone}
                Mejl:       {App.CurrentUser.Email}

                Stämmer detta? Y/n:
                """);
    }
}