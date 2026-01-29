using Webshop.Helpers;

namespace Webshop.Views.Customer;

// I opted for not extending MenuBase in this view, the reasoning behind this is that while it could technically do that
// this view is heavy on data collection, display and confirmation dialogs which block user input anyway,
// so the user can't navigate the menu items.
internal class CheckoutView(WebshopApplication app)
{
    private readonly WebshopApplication _app = app;
    private readonly Services.CheckoutService _checkoutService = new(app);
    private readonly Services.CreateAccountService _createAccountService = new(app);
    private readonly CustomerRegistrationHelper _customerRegistrationHelper = new();

    internal async Task ActivateAsync()
    {
        try
        {
            Console.CursorVisible = true;

            if (_app.CurrentUser.Address != null)
            {
                await ShowCurrentAddressAsync();

                if (!InputHelper.GetConfirmation("Stämmer detta?"))
                {
                    var (firstName, lastName, age, region, city, postalCode, street, houseNumber, phone, email) = _customerRegistrationHelper.CollectAddressInput();

                    _createAccountService.SetCustomerAddress(age, region, city, postalCode, street, houseNumber, firstName, lastName, phone, email);
                }
            }
            else
            {
                var (firstName, lastName, age, region, city, postalCode, street, houseNumber, phone, email) = _customerRegistrationHelper.CollectAddressInput();

                _createAccountService.SetCustomerAddress(age, region, city, postalCode, street, houseNumber, firstName, lastName, phone, email);
            }

            var shippingMethod = await _checkoutService.GetShippingMethodAsync();
            if (shippingMethod == null) return;

            var paymentMethod = await _checkoutService.GetPaymentMethodAsync();
            if (paymentMethod == null) return;

            await RenderOrderAsync(shippingMethod, paymentMethod);

            if (!InputHelper.GetConfirmation("Stämmer detta?"))
            {
                return;
            }

            if (await _checkoutService.CompleteOrder(shippingMethod, paymentMethod))
            {
                Console.Clear();

                MessageHelper.ShowSuccess("""
                    Grattis till ditt köp!
                    
            Dina varor kommer skickas till dig omgående, senast nästkommande vardag.

            Vi hoppas du är nöjd med din upplevelse hos oss!
            """);

                if (_app.CurrentUser.IsGuest && InputHelper.GetConfirmation("Vill du skapa ett konto?"))
                {
                    await new CreateAccountView(_app).ActivateAsync();
                }
            }
            else
            {
                MessageHelper.ShowError("Kunde inte slutföra beställningen eller beställning avbruten.");
            }
        }
        catch (Exception e)
        {
            MessageHelper.ShowError($"""
                Ett fel inträffade under utcheckningen: {e.Message}

                Din beställning har inte genomförts.
                """);
        }
        finally
        {
            Console.CursorVisible = false;
        }
    }

    private async Task RenderOrderAsync(Models.ShippingMethod shippingMethod, Models.PaymentMethod paymentMethod)
    {
        string countryName = await _app.Database.GetCountryName(_app.CurrentUser.Address!.CountryId);

        Console.Clear();
        Console.WriteLine($"""
            Dina uppgifter är:

            Namn:               {_app.CurrentUser.FirstName} {_app.CurrentUser.LastName}
            Mejl:               {_app.CurrentUser.Email}
            Telefon             {_app.CurrentUser.Phone}

            Land:               {countryName}
            Stad:               {_app.CurrentUser.Address.City}
            Län:                {_app.CurrentUser.Address.Region}
            Postkod:            {_app.CurrentUser.Address.PostalCode}
            Gata:               {_app.CurrentUser.Address.Street}
            Husnummer:          {_app.CurrentUser.Address.HouseNumber}

            Ditt valda leveransalternativ är:

            Fraktmetod:         {shippingMethod.Name}
            Kostnad:            {shippingMethod.Price}

            Ditt valda betalsätt är:

            Betalmetod:         {paymentMethod.Name}
            Kostnad:            {paymentMethod.TransactionFee}

            Din order inklusive kostnad:

            """);

        _app.Basket.ListBasketItems();

        Console.WriteLine($"""
            Kostnad:            {_app.Basket.GetTotal() + paymentMethod.TransactionFee + shippingMethod.Price}
            Varav moms:         {_app.Basket.GetTotalTax()}
            """);
    }

    private async Task ShowCurrentAddressAsync()
    {
        Console.Clear();

        string countryName = await _app.Database.GetCountryName(_app.CurrentUser.Address!.CountryId);

        var address = _app.CurrentUser.Address;

        Console.WriteLine($"""
                Din sparade adress och kontaktuppgifter är:

                Land:       {countryName}
                Län:        {address.Region}
                Stad:       {address.City}
                Postkod:    {address.PostalCode}
                Gata:       {address.Street}
                Husnummer:  {address.HouseNumber}

                Telefon:    {_app.CurrentUser.Phone}
                Mejl:       {_app.CurrentUser.Email}
                """);
    }
}