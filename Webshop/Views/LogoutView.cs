using Webshop.Services;

namespace Webshop.Views;

internal class LogoutView(WebshopApplication app)
{
    protected WebshopApplication App { get; } = app;

    public void Activate()
    {
        Console.Clear();
        Console.WriteLine("""
            Är du säker på att du vill logga ut?
            
            [J]a
            [N]ej

            """);

        if (Console.ReadKey().Key == ConsoleKey.J)
            new LogoutService(App).Logout();

        return;
    }
}