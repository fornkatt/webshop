namespace Webshop.Views.Admin;

internal sealed class AdminLogsView(string headerText, AdminApplication app)
{
    internal async Task DisplayLogs()
    {
        Console.Clear();
        Helpers.MessageHelper.ShowHeader(headerText);

        var logs = await app.MongoLogService.GetRecentLogsAsync();

        foreach (var log in logs)
        {
            Console.WriteLine($"""
                {new string('─', 50)}

                ID:         {log.UserId}
                Namn:       {log.Name}
                Mejl:       {log.Email}

                Händelse:   {log.ActionType}
                Detaljer:   {log.Details}

                """);
        }
        Console.WriteLine(new string('─', 50));

        Helpers.MessageHelper.ShowSuccess("Loggar visade!");
    }
}