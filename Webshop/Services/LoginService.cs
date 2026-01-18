namespace Webshop.Services;

internal class LoginService
{
    // Get admin details from database
    private string AdminUsername { get; }
    private string AdminPassword { get; }
    // Normal user details
    private string Username { get; set; }
    private string Password { get; set; }
}
