namespace Webshop;

internal class Program
{
    static async Task Main()
    {
        //Services.DatabaseSeederService databaseSeederService = new();
        //await databaseSeederService.SeedDatabaseAsync();

        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("sv-SE");

        var webshop = new WebshopApplication();

        await webshop.GoToMainMenuAsync();
    }
}