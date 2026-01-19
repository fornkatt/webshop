using Webshop.Models;
using Webshop.Views;

namespace Webshop;

internal class Program
{
    static void Main()
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("sv-SE");

        Console.CursorVisible = false;

        var webshop = new WebshopApplication();

        webshop.Run();
    }
}
