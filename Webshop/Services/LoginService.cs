namespace Webshop.Services;

internal class LoginService(WebshopApplication app)
{
    protected WebshopApplication App { get; } = app;
    // Get admin details from database
    private string? AdminUsername { get; }
    private string? AdminPassword { get; }
    // Normal user details
    private string? Username { get; set; }
    private string? Password { get; set; }

    public void Login()
    {
        App.CurrentUser.IsGuest = false;
    }
}
