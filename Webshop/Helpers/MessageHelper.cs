namespace Webshop.Helpers;

internal class MessageHelper
{
    internal static void ShowSuccess(string message)
    {
        Console.WriteLine($"""

            {message}

            Tryck på valfri tangent för att fortsätta...
            """);
        Console.ReadKey(true);
    }

    internal static void ShowError(string message)
    {
        Console.WriteLine($"""
            {message}

            Tryck på valfi tangent för att fortsätta.
            """);
        Console.ReadKey(true);
    }

    internal static void ShowHeader(string title)
    {
        Console.WriteLine($"=== {title.ToUpper()} ===");
        Console.WriteLine();
    }
}
