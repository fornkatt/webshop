using Webshop.Helpers;

namespace Webshop.Views.Customer;

// I feel this is it's own entity, separated from MenuBase so I didn't extend it for this view.
// It displays some information and calls on data collection methods to collect user information to make accounts
// which are blocking and don't need a menu to navigate.
internal class CreateAccountView(WebshopApplication app)
{
    private WebshopApplication App { get; } = app;
    private readonly Services.CreateAccountService _createAccountService = new(app);
    private readonly CustomerRegistrationHelper _customerRegistrationHelper = new();

    internal async Task ActivateAsync()
    {
        try
        {
            Console.CursorVisible = true;

            Console.Clear();
            MessageHelper.ShowHeader("Skapa konto");

            if (App.CurrentUser.Id != 0)
            {
                await ConvertGuestAccountAsync();
            }
            else
            {
                await CreateNewAccountAsync();
            }
        }
        catch (Exception e)
        {
            MessageHelper.ShowError($"Något gick fel: {e.Message}");
        }
        finally
        {
            Console.CursorVisible = false;
        }
    }

    private async Task ConvertGuestAccountAsync()
    {
        var (username, password) = _customerRegistrationHelper.GetNewUsernameAndPassword();

        if (await _createAccountService.SetUsernameAndPassword(username, password))
        {
            await App.Database.UpdateCustomerAsync(App.CurrentUser);
            await App.MongoLogService.LogActionAsync("Nytt konto", App.CurrentUser.Id, 
                App.CurrentUser.FirstName, App.CurrentUser.Email, "Konverterat från gästkonto");

            MessageHelper.ShowSuccess("Konto skapat!");
            return;
        }
    }
    private async Task CreateNewAccountAsync()
    {
        var (firstName, lastName, age, region, city, postalCode, street, houseNumber, phone, email) = _customerRegistrationHelper.CollectAddressInput();
        var (username, password) = _customerRegistrationHelper.GetNewUsernameAndPassword();

        _createAccountService.SetCustomerAddress(age, region, city, postalCode, street, houseNumber, firstName, lastName, phone, email);

        if (await _createAccountService.SetUsernameAndPassword(username, password))
        {
            await App.Database.AddNewCustomerAsync(App.CurrentUser);
            await App.MongoLogService.LogActionAsync("Nytt konto", App.CurrentUser.Id,
                App.CurrentUser.FirstName, App.CurrentUser.Email, "Ny användare");

            MessageHelper.ShowSuccess("Konto skapat!");
            return;
        }
    }
}