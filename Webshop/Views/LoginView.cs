using Webshop.Services;

namespace Webshop.Views;

internal class LoginView(string headerText, WebshopApplication app)
{
    protected WebshopApplication App { get; } = app;
    private string HeaderText { get; set; } = headerText;

    public void Activate()
    {
        Console.Clear();
        Console.WriteLine(HeaderText);
        Console.WriteLine();
    }
}
