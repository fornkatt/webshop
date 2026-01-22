namespace Webshop.Views;

internal class LoginView(string headerText, WebshopApplication app)
{
    protected WebshopApplication App { get; } = app;
    private string HeaderText { get; } = headerText;

    internal async Task ActivateAsync()
    {
        Console.Clear();
        Console.WriteLine(HeaderText);
        Console.WriteLine();

        var loginService = new Services.LoginService(App);
        await loginService.LoginAsync();
    }
}
