using Webshop.Models;
using Webshop.Views;

namespace Webshop;

internal class Program
{
    static void Main()
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("sv-SE");

        Console.CursorVisible = false;

        try
        {
            Console.SetWindowSize(20, 20);
            Console.SetBufferSize(160, 1000);
        }
        catch (ArgumentOutOfRangeException e)
        {
            File.WriteAllText("Log.txt", $"{e}\n");
        }

        var webshop = new WebshopApplication();

        webshop.Run();
    }
}
