namespace Webshop.Services;

internal sealed class CreateAccountService(WebshopApplication app)
{
    private WebshopApplication App { get; } = app;
    internal async Task<bool> SetUsernameAndPassword(string username, string password)
    {
        try
        {
            App.CurrentUser.Username = username;
            App.CurrentUser.Password = password;
            App.CurrentUser.IsActive = true;
            App.CurrentUser.IsGuest = false;

            return true;
        }
        catch (Exception e)
        {
            Helpers.MessageHelper.ShowError($"Något gick fel: {e.Message}");
            return false;
        }
    }
    internal void SetCustomerAddress(
        int age,
        string region,
        string city,
        int postalCode,
        string street,
        string houseNumber,
        string firstName,
        string lastName,
        string phone,
        string email)
    {
        var address = new Models.Address()
        {
            CustomerId = App.CurrentUser.Id,
            CountryId = 1,
            Region = region,
            City = city,
            PostalCode = postalCode,
            Street = street,
            HouseNumber = houseNumber,
        };

        App.CurrentUser.Address = address;

        App.CurrentUser.FirstName = firstName;
        App.CurrentUser.LastName = lastName;
        App.CurrentUser.Age = age;
        App.CurrentUser.Phone = phone;
        App.CurrentUser.Email = email;
    }
}
