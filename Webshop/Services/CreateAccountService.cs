namespace Webshop.Services;

internal sealed class CreateAccountService(WebshopApplication app)
{
    private readonly WebshopApplication _app = app;
    internal async Task<bool> SetUsernameAndPassword(string username, string password)
    {
        try
        {
            _app.CurrentUser.Username = username;
            _app.CurrentUser.Password = password;
            _app.CurrentUser.IsActive = true;
            _app.CurrentUser.IsGuest = false;

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
            CustomerId = _app.CurrentUser.Id,
            CountryId = 1,
            Region = region,
            City = city,
            PostalCode = postalCode,
            Street = street,
            HouseNumber = houseNumber,
        };

        _app.CurrentUser.Address = address;

        _app.CurrentUser.FirstName = firstName;
        _app.CurrentUser.LastName = lastName;
        _app.CurrentUser.Age = age;
        _app.CurrentUser.Phone = phone;
        _app.CurrentUser.Email = email;
    }
}
