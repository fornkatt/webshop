namespace Webshop;

internal class Program
{
    static void Main()
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("sv-SE");

        Console.CursorVisible = false;

        Views.MainMenu mainMenu = new("Välkommen till min webshop!");

        mainMenu.Activate();
    }
}
