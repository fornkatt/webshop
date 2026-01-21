namespace Webshop.Views;

internal class LoginView(string headerText, WebshopApplication app)
{
    protected WebshopApplication App { get; } = app;
    private string HeaderText { get; set; } = headerText;

    internal void Activate()
    {
        Console.Clear();
        Console.WriteLine(HeaderText);
        Console.WriteLine();

        var loginService = new Services.LoginService(App);
        loginService.Login();
    }
}
