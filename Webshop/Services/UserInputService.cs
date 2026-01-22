namespace Webshop.Services;

internal class UserInputService
{
    internal static string GetUserInput(string prompt, string contextHeader)
    {
        string? input;

        while (true)
        {
            Console.Clear();
            Console.WriteLine(contextHeader);
            Console.WriteLine();
            Console.Write(prompt);
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine();
                Console.WriteLine("Var god mata in ett värde. Tryck på valfri tangent för att fortsätta.");
                Console.ReadKey(true);
            }
            else
            {
                return input;
            }
        }
    }

    internal static int ValidateUserInputInt(string prompt, string contextHeader)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(contextHeader);
            Console.WriteLine();
            Console.Write(prompt);
            if (!int.TryParse(Console.ReadLine(), out int validatedInput))
            {
                Console.WriteLine("Var god mata in ett giltigt värde. Tryck på valfri tangent för att fortsätta.");
                Console.ReadKey(true);
                continue;
            }
            return validatedInput;
        }
    }
}