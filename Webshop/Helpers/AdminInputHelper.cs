namespace Webshop.Helpers;

internal static class AdminInputHelper
{
    internal static string GetTextInput(string prompt)
    {
        while (true)
        {
            Console.WriteLine();
            Console.Write($"{prompt}: ");

            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            Console.WriteLine("Ogiltigt värde! Försök igen.");
            Console.ReadKey(true);
            continue;
        }
    }

    internal static int GetIntInput(string prompt)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine($"{prompt}: ");

            var input = Console.ReadLine();
            if (int.TryParse(input, out int value))
            {
                return value;
            }
            ShowError("Ogiltigt värde!");
            continue;
        }
    }

    internal static decimal GetDecimalInput(string prompt)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine($"{prompt}: ");

            var input = Console.ReadLine();
            if (decimal.TryParse(input, out decimal value) && value > 0)
            {
                return value;
            }
            ShowError("Ogiltigt värde!");
            continue;
        }
    }

    internal static bool GetConfirmation(string prompt)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine($"{prompt} (Y/n): ");

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Y) return true;
            if (key == ConsoleKey.N) return false;

            ShowError("Vänligen välj Y eller N!");
        }
    }

    internal static void ShowHeader(string title)
    {
        Console.Clear();
        Console.WriteLine($"=== {title.ToUpper()} ===");
        Console.WriteLine();
    }

    internal static void ShowSuccess(string message)
    {
        Console.WriteLine();
        Console.WriteLine(message);
        Console.WriteLine();
        Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
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
}
