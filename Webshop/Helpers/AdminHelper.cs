namespace Webshop.Helpers;

internal static class AdminHelper
{
    internal static int? SelectItem<T>(string prompt, List<T> items, Func<T, int> idSelector, Func<T, string> nameSelector)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"""
                {prompt}

                """);

            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {nameSelector(items[i])}");
            }
            Console.WriteLine();

            Console.Write("Ditt val (eller Q för att avbryta): ");
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input) || input.Equals("Q", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= items.Count)
            {
                return idSelector(items[choice - 1]);
            }

            MessageHelper.ShowError("""

                Ogiltigt val. Försök igen.
                """);
        }
    }
}
