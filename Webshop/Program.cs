namespace Webshop;

internal class Program
{
    static async Task Main()
    {
        //Services.DatabaseSeederService databaseSeederService = new();
        //await databaseSeederService.SeedDatabaseAsync();

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

        await webshop.GoToMainMenuAsync();
    }
}